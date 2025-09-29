---
applyTo: "**/*.cs"
---

# C# Guidelines

- Make only high confidence suggestions when reviewing code changes.
- Always use the latest C# version features (currently C# 14).
- Always use file-scoped namespaces.
- New classes/records should be `internal` and `sealed` unless otherwise necessary.
- Never change NuGet.config files unless explicitly asked to.

## Coding Style

Always use collection expressions to initialize collections:

```csharp
List<int> numbers = [1, 2, 3, 4, 5];
```

Always use immutable records instead of classes for DTOs:

```csharp
public record Person(string FirstName, string LastName, ImmutableList<Address> Addresses);
```

Prefer to use `var` when the type of a variable is apparent:

```csharp
var myInstance = new MyClass();
var name = "Copilot";
var foo = Foo.Create();
```

Other patterns:

- Prefer LINQ over for/foreach loops for managing collections.
- Use pattern matching and switch expressions wherever possible.
- Use nameof instead of string literals when referring to member names.
- Place private class declarations at the bottom of the file.
- Copy existing style in nearby files for class/method/member names.
- Apply code-formatting style defined in `.editorconfig`.

### Nullable reference types

- Always use `is null` or `is not null` instead of `== null` or `!= null`.
- Trust the C# nullable annotations and don't add null checks when the type system says a value cannot be null.

## Comments

### Documentation comments

- Use XML documentation comments for types and members.
- Comments that simply restate the member name do not provide value.
- If it's exceptionally obvious what a type or member does, a doc comment is not necessary.
- Comments should provide additional context or explain non-obvious behavior.

### Code comments

- Comments inside methods should explain "why," not "what".
- Comments should provide context and rationale behind the code.
- Avoid redundant comments that restate the code - not all code needs comments.
