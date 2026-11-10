---
name: choose-ambient-scope-lifetimes
description: >-
  Use TedToolkit.Scopes for ambient context that must flow through async calls or remain in a synchronous scope.
---

# Choose Ambient Scope Lifetimes

Decide whether the context must flow across an `await` before selecting a scope API. Use the matching reader for the chosen storage and avoid widening the lifetime to unrelated work.

## Workflow

- Use `Push()` when context must flow through `await`. Use `FastPush()` only for a synchronous lifetime and read the matching `FastScope<T>.Current`.
- Wrap the lifetime in `using` so disposal restores the previous scope even if an exception exits the block.
- Check the analyzer’s recommendation in context. Test nested restoration and include an asynchronous continuation when the caller depends on context flow.

## Example

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

## Check the result

Verify both nested restoration and exception unwinding. For async context, check the value after an actual continuation. For a synchronous caller, inspect analyzer guidance such as `SCOPE002` before choosing `FastPush()`.
