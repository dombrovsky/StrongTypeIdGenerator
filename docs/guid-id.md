# Guid IDs

Use `[GuidId]` to generate a strongly typed identifier backed by `Guid`.

## Basic example

```csharp
using StrongTypeIdGenerator;

[GuidId]
public sealed partial class CustomerId
{
}
```

## Generated behavior

A generated `CustomerId` includes:

- Constructor that accepts `Guid`
- `Value` property (or custom name)
- `Unspecified` initialized with `Guid.Empty`
- Equality/comparison members and operators
- `ToString()` and formatting overload
- Implicit conversions to and from `Guid`
- Nested `TypeConverter`

## Validation example

```csharp
[GuidId]
public sealed partial class CustomerId
{
    private static Guid CheckValue(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Empty GUID is not allowed", nameof(value));

        return value;
    }
}
```

## Custom property name

```csharp
[GuidId(ValuePropertyName = "Uuid")]
public sealed partial class ExternalReferenceId
{
}
```

## Private constructor mode

```csharp
[GuidId(GenerateConstructorPrivate = true)]
public sealed partial class UserId
{
    public static UserId CreateNew() => new UserId(Guid.NewGuid());
    public static UserId FromString(string value) => new UserId(Guid.Parse(value));
}
```

Next: [combined IDs](combined-id.md).
