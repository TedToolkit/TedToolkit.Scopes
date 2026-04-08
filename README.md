# TedToolkit.Scopes

A lightweight, high-performance ambient context (scope) library for .NET. Provides multiple scope implementations for tracking contextual state through synchronous and asynchronous call chains, with built-in Roslyn analyzers for performance guidance.

## Installation

```shell
dotnet add package TedToolkit.Scopes
```

The package includes Roslyn analyzers that automatically suggest performance improvements.

## Supported Frameworks

`net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`, `net472`, `net48`, `netstandard2.0`, `netstandard2.1`

## Quick Start

Implement the `IScope` interface to define your scope:

```csharp
using TedToolkit.Scopes;

// Class-based scope (reference type)
public class LogScope(string context) : IScope
{
    public string Context => context;

    public void OnEntry() { /* called when scope is entered */ }
    public void OnExit()  { /* called when scope is disposed */ }
}

// Struct-based scope (value type)
public struct TraceScope(string traceId) : IScope
{
    public string TraceId => traceId;

    public void OnEntry() { }
    public void OnExit()  { }
}
```

Push a scope with `using` and read it anywhere in the call chain:

```csharp
// In an async method — use Push()
using (new LogScope("request-handler").Push())
{
    var current = Scope<LogScope>.Current; // LogScope instance
    await HandleAsync();
}

// In a sync method — use FastPush() for best performance
using (new LogScope("sync-handler").FastPush())
{
    var current = FastScope<LogScope>.Current;
    Process();
}
```

## Scope Types

### `Scope<TScope>` — Async-safe scope for class types

Uses `AsyncLocal<T>` to flow through `async`/`await` and `Task.Run`. Supports nesting — disposing restores the parent scope.

```csharp
using (new LogScope("outer").Push())
{
    // Scope<LogScope>.Current == "outer"
    await Task.Run(async () =>
    {
        using (new LogScope("inner").Push())
        {
            // Scope<LogScope>.Current == "inner"
        }
        // Scope<LogScope>.Current == "outer"
    });
}
```

### `ValueScope<TScope>` — Async-safe scope for struct types

Same `AsyncLocal` behavior as `Scope<T>`, but accepts `struct` types implementing `IScope`.

```csharp
using (new TraceScope("abc-123").Push())
{
    // ValueScope<TraceScope>.Current.TraceId == "abc-123"
    await DoWorkAsync();
}
```

### `FastScope<TScope>` — Thread-local fast scope

A `ref struct` backed by a thread-static stack. Zero-allocation push/pop with the best performance, but **cannot cross `await` boundaries**. Use only in synchronous methods.

```csharp
using (new TraceScope("abc-123").FastPush())
{
    // FastScope<TraceScope>.Current.TraceId == "abc-123"
    DoWork();
}
```

### `ScopeBase<TScope>` — Abstract base class

An async-safe base class using `AsyncLocal`. Creating an instance automatically enters the scope; disposing it restores the parent. Implements the full `IDisposable` pattern with a finalizer.

```csharp
public class MyScope : ScopeBase<MyScope>
{
    public string Name { get; }

    public MyScope(string name) => Name = name;
}

// Usage
using (new MyScope("hello"))
{
    // MyScope.Current?.Name == "hello"
    await DoWorkAsync();
}
```

### `ScopeRecord<TScope>` — Abstract record base class

Same behavior as `ScopeBase<T>`, but inherits from `record` for value equality and `with` expression support.

```csharp
public record MyRecordScope(string Name) : ScopeRecord<MyRecordScope>;

using (new MyRecordScope("hello"))
{
    // MyRecordScope.Current?.Name == "hello"
}
```

## Extension Methods

### For class types (`ScopeExtensions`)

| Method | Returns | Description |
|---|---|---|
| `.Push()` | `Scope<TScope>` | Async-safe push |
| `.FastPush()` | `FastScope<TScope>` | Thread-local fast push |

### For struct types (`ValueScopeExtensions`)

| Method | Returns | Description |
|---|---|---|
| `.Push()` | `ValueScope<TScope>` | Async-safe push |
| `.FastPush()` | `FastScope<TScope>` | Thread-local fast push |

## Unified Access with `ScopeValues`

`ScopeValues` provides a single API to read the current scope regardless of whether it was pushed with `Push()` or `FastPush()`. It checks `FastScope` first, then falls back to the regular scope.

```csharp
// For struct scopes
ref readonly TraceScope current = ref ScopeValues.Struct<TraceScope>.Current;
bool hasScope = ScopeValues.Struct<TraceScope>.HasCurrent;

// For class scopes
LogScope? current = ScopeValues.Class<LogScope>.Current;
bool hasScope = ScopeValues.Class<LogScope>.HasCurrent;
```

## Roslyn Analyzers

The package ships with two diagnostics that guide you toward optimal performance:

| ID | Severity | Title | Description |
|---|---|---|---|
| **SCOPE001** | Info | Use FastScope in non-async contexts | Fires when `new Scope<T>()` or `new ValueScope<T>()` is used in a synchronous method. Suggests `FastScope<T>` instead. |
| **SCOPE002** | Info | Use FastPush in non-async contexts | Fires when `.Push()` is called in a synchronous method. Suggests `.FastPush()` instead. |

Both diagnostics include automatic code fixes.

## Choosing a Scope Type

| Need | Type | Notes |
|---|---|---|
| Async-safe, class type | `Scope<T>` + `.Push()` | Uses `AsyncLocal`, flows through `await` |
| Async-safe, struct type | `ValueScope<T>` + `.Push()` | Uses `AsyncLocal`, avoids boxing |
| Maximum performance, sync only | `FastScope<T>` + `.FastPush()` | Thread-static stack, `ref struct` |
| Scope as base class | `ScopeBase<T>` | Auto-enters on construction |
| Scope as record | `ScopeRecord<T>` | Auto-enters on construction, value equality |
| Read from any push method | `ScopeValues.Class<T>` / `ScopeValues.Struct<T>` | Checks FastScope first |

## Project Structure

| Directory | Description |
|---|---|
| `TedToolkit.Scopes/` | Core library |
| `TedToolkit.Scopes.Analyzers/` | Roslyn analyzers and code fixes |
| `TedToolkit.Scopes.Tests/` | Unit tests (TUnit) |
| `TedToolkit.Scopes.Benchmark/` | BenchmarkDotNet benchmarks |
| `Build/` | Build automation |

## License

This project is licensed under the [LGPL-3.0](COPYING.LESSER).
