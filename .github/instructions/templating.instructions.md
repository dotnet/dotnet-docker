---
applyTo: "eng/dockerfile-templates/**/*,eng/readme-templates/**/*"
---

# Instructions for modifying Dockerfile templates

All Dockerfiles and Readmes are generated from [Cottle](https://github.com/r3c/cottle) templates in
the `eng/dockerfile-templates` and `eng/readme-templates` directories respectively.

## Cottle syntax

### Templating

All text is plain text by default.  Text inside double curly braces `{{ }}` is template code.
Writing just `{{value}}` emits the evaluated value.
You can also have expressions inside template blocks which do more complicated logic.
Template code can have multiple lines in it, separated by the line ending `^` character.
The final line in a template block should not use the line ending character.
Comments inside a template block start with `_` and follow the same line ending rules.
Comments can span multiple lines.

### Template data sources

- `VARIABLES`: Variables from `manifest.versions.json`. Access with `VARIABLES["name"]`.
- `ARGS`: Passed into template via `InsertTemplate(...)` function. Access with `ARGS["name"]`

### Common functions

- Assign value: `set name to <expression>`.
- Assign function `set func(arg1, arg2) to:{{ ... }}` defines an inline function; use `return <expr>` inside.
- String functions:
  - `cat(a, b, ...)`
  - `join(list, sep)`
  - `split(str, sep)`
  - `slice(list, start, end)`.
- Collections:
  - `len(list)`,
  - `find(haystack, needle)` returns index or -1.
- Ternary expression: `when(condition, valueIfTrue, valueIfFalse)`.

### Conditionals

Boolean expressions:

- Operators: `=`, `!=`, `>`, `<`, `>=`, `<=`, logical `&&`, `||`, and negation `!`.
- Equality uses single `=` inside expressions (context-specific to Cottle) rather than `==`.

If statement:

```cottle
{{if condition:VALUE}}
```

Compound conditionals:

```cottle
{{if ((condition1 && condition2) || condition3):VALUE}}
```

Multiple conditions:

```cottle
{{if condition1:VALUE1^
elif condition2:VALUE2^
else:VALUE3}}
```

After the colon `:`, all whitespace is treated as literal until the next line ending character `^`.

You can also nest template blocks:

```cottle
{{if condition1:{{if condition2:VALUE}}}}
```
