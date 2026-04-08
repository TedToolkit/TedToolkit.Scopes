# TedToolkit.Scopes

A lightweight, high-performance ambient context (scope) library for .NET. Track contextual state through synchronous and asynchronous call chains with built-in Roslyn analyzers for performance guidance.

## Quick Start

Implement `IScope` and push it onto the ambient context:

```csharp
using TedToolkit.Scopes;

public class LogScope(string context) : IScope
{
    public string Context => context;
    public void OnEntry() { }
    public void OnExit()  { }
}
```

```csharp
// Async method — use Push()
using (new LogScope("request").Push())
{
    var current = Scope<LogScope>.Current; // LogScope instance
    await HandleAsync();
}

// Sync method — use FastPush() for best performance
using (new LogScope("sync").FastPush())
{
    var current = FastScope<LogScope>.Current;
    Process();
}
```

## Scope Types

| Type | Async-safe | Constraint | Backing |
|---|---|---|---|
| `Scope<T>` | Yes | `class, IScope` | `AsyncLocal` |
| `ValueScope<T>` | Yes | `struct, IScope` | `AsyncLocal` |
| `FastScope<T>` | No | `IScope` | Thread-static stack |
| `ScopeBase<T>` | Yes | Inherit from it | `AsyncLocal` |
| `ScopeRecord<T>` | Yes | Inherit from it | `AsyncLocal` |

- **`Scope<T>`** / **`ValueScope<T>`** — Push via `.Push()`. Flows through `async`/`await` and `Task.Run`.
- **`FastScope<T>`** — Push via `.FastPush()`. `ref struct`, cannot cross `await` boundaries. Best performance.
- **`ScopeBase<T>`** / **`ScopeRecord<T>`** — Abstract base classes that auto-enter the scope on construction.

## Unified Access

Use `ScopeValues` to read the current scope regardless of how it was pushed:

```csharp
// Class scopes
LogScope? current = ScopeValues.Class<LogScope>.Current;

// Struct scopes
ref readonly TraceScope current = ref ScopeValues.Struct<TraceScope>.Current;
```

## Analyzers

| ID | Title |
|---|---|
| **SCOPE001** | Suggests `FastScope<T>` instead of `Scope<T>`/`ValueScope<T>` in sync methods |
| **SCOPE002** | Suggests `.FastPush()` instead of `.Push()` in sync methods |

Both include automatic code fixes.

## License

[LGPL-3.0](https://github.com/TedToolkit/TedToolkit.Scopes/blob/development/COPYING.LESSER)
