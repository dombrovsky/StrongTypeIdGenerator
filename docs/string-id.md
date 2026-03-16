# String IDs

Use `[StringId]` to generate a strongly typed identifier backed by `string`.

## Basic example

```csharp
using StrongTypeIdGenerator;

[StringId]
public sealed partial class OrderId
{
}
```

## Generated behavior

A generated `OrderId` includes:

- Constructor that accepts `string`
- `Value` property (or custom name)
- `Unspecified` initialized with `string.Empty`
- Equality/comparison members and operators
- `ToString()` and `ToString(string?, IFormatProvider?)`
- Implicit conversions to and from `string`
- Nested `TypeConverter`

## Custom validation

```csharp
[StringId]
public sealed partial class OrderId
{
    private static string CheckValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Order ID cannot be empty", nameof(value));

        return value.Trim().ToUpperInvariant();
    }
}
```

`CheckValue` is called from the generated constructor.

## Custom value property name

```csharp
[StringId(ValuePropertyName = "Code")]
public sealed partial class ProductCode
{
}
```

The generated property name becomes `Code` instead of `Value`.

## Private constructor mode

```csharp
[StringId(GenerateConstructorPrivate = true)]
public sealed partial class SessionToken
{
    public static SessionToken Create(string raw) => new SessionToken(raw);
}
```

Continue with [custom validation](custom-validation-checkvalue.md) and [private constructors](private-constructors-and-factories.md).
