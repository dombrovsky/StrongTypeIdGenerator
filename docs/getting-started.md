# Getting Started

This guide shows how to install StrongTypeIdGenerator and generate your first strongly typed identifier.

## Install

Install the main package:

```bash
dotnet add package StrongTypeIdGenerator
```

Optional JSON integration package:

```bash
dotnet add package StrongTypeIdGenerator.Json
```

## Create your first ID type

```csharp
using StrongTypeIdGenerator;

[StringId]
public sealed partial class ProductId
{
}
```

Build the project. The source generator creates members on `ProductId`.

## What gets generated

For scalar IDs (`[StringId]` and `[GuidId]`), generated members include:

- Constructor
- `Value` property (or custom property name)
- `Unspecified`
- Equality and comparison support
- `ToString` and formatting overload
- Implicit conversions
- Nested `TypeConverter`

## Guid example

```csharp
[GuidId]
public sealed partial class UserId
{
}
```

## Combined ID example

```csharp
[CombinedId(typeof(UserId), "UserId", typeof(ProductId), "ProductId")]
public sealed partial class UserProductId
{
}
```

Combined IDs produce one property per component, comparison/equality, formatting, and `TypeConverter` support.

## Next steps

- [String IDs](string-id.md)
- [Guid IDs](guid-id.md)
- [Combined IDs](combined-id.md)
- [Custom validation](custom-validation-checkvalue.md)
