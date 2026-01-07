// -----------------------------------------------------------------------
// <copyright file="Scope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// The Scope
/// </summary>
/// <typeparam name="TScope">scope value</typeparam>
#pragma warning disable CA1815
public readonly struct Scope<TScope> : IDisposable
#pragma warning restore CA1815
    where TScope : struct
{
    private sealed class ScopeItem
    {
        private const int STEP = 8;

        private readonly
#if NET9_0_OR_GREATER
            Lock
#else
            object
#endif
            _locker = new();

        private int _returnedPoolCount;

        private int[] _returnedPool = CreateArray<int>(STEP);

        private int _poolCount;

        private TScope[] _pool = CreateArray<TScope>(STEP);

        public ref readonly TScope TryGet(int index)
            => ref _pool[index];

        public void Remove(int index)
        {
            lock (_locker)
            {
                if (_returnedPoolCount >= _returnedPool.Length)
                {
                    var newPool = CreateArray<int>(_returnedPool.Length + STEP);
                    _returnedPool.CopyTo(newPool, 0);
                    _returnedPool = newPool;
                }

                _returnedPool[_returnedPoolCount] = index;
                _returnedPoolCount++;
            }
        }

        public int Add(scoped in TScope value)
        {
            lock (_locker)
            {
                if (_returnedPoolCount > 0)
                {
                    var index = _returnedPool[--_returnedPoolCount];
                    _pool[index] = value;
                    return index;
                }

                if (_poolCount >= _pool.Length)
                {
                    var newPool = CreateArray<TScope>(_pool.Length + STEP);
                    _pool.CopyTo(newPool, 0);
                    _pool = newPool;
                }

                _pool[_poolCount] = value;
                var result = _poolCount;
                _poolCount++;
                return result;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T[] CreateArray<T>(int count)
        {
#if NET6_0_OR_GREATER
            return GC.AllocateUninitializedArray<T>(count);
#else
            return new T[count];
#endif
        }
    }

#pragma warning disable S2743
    private static readonly ScopeItem _item = new();
    private static readonly AsyncLocal<int> _current = new();
#pragma warning restore S2743

    static Scope()
        => _current.Value = -1;

    private readonly int _parent;

    /// <summary>
    ///  Current Value
    /// </summary>
    public static TScope? Current
    {
        get
        {
            var index = _current.Value;
            if (index < 0)
                return null;

            return _item.TryGet(index);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_current.Value >= 0)
            _item.Remove(_current.Value);

        _current.Value = _parent;
    }

    /// <summary>
    /// Create a scope
    /// </summary>
    /// <param name="value">the value</param>
    public Scope(scoped in TScope value)
    {
        _parent = _current.Value;
        _current.Value = _item.Add(value);
    }
}