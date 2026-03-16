# Custom Validation with CheckValue

`CheckValue` lets you enforce invariants and normalize values in generated identifiers.

## Scalar IDs

For `[StringId]`:

```csharp
private static string CheckValue(string value)
```

For `[GuidId]`:

```csharp
private static Guid CheckValue(Guid value)
```

Example:

```csharp
[StringId]
public sealed partial class InvoiceNumber
{
    private static string CheckValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invoice number is required", nameof(value));

        return value.Trim().ToUpperInvariant();
    }
}
```

## Combined IDs

For `[CombinedId]`, define parameters matching constructor components and return a tuple of the same shape.

```csharp
private static (Guid TenantId, string UserCode) CheckValue(Guid tenantId, string userCode)
```

Example:

```csharp
[CombinedId(typeof(Guid), "TenantId", typeof(string), "UserCode")]
public sealed partial class TenantUserId
{
    private static (Guid TenantId, string UserCode) CheckValue(Guid tenantId, string userCode)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty", nameof(tenantId));

        if (string.IsNullOrWhiteSpace(userCode))
            throw new ArgumentException("UserCode is required", nameof(userCode));

        return (tenantId, userCode.Trim());
    }
}
```

## Behavior notes

- `CheckValue` is called by the generated constructor.
- Throwing exceptions prevents invalid IDs from being created.
- Returned values are used to initialize generated properties.

See also [private constructors and factories](private-constructors-and-factories.md).
