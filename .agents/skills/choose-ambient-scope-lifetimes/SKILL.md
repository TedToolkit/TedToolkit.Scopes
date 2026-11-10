---
name: choose-ambient-scope-lifetimes
description: >-
  Use TedToolkit.Scopes for ambient context that must flow through async calls or remain in a synchronous scope.
---

# Choose Ambient Scope Lifetimes

- Use `Push()` when context must flow through `await`. Use `FastPush()` only for a synchronous lifetime and read the matching `FastScope<T>.Current`.
- Wrap the lifetime in `using` so disposal restores the previous scope even if an exception exits the block.
- Check the analyzer’s recommendation in context. Test nested restoration and include an asynchronous continuation when the caller depends on context flow.
