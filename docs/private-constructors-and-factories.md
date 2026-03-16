# Private Constructors and Factory Methods

Set `GenerateConstructorPrivate = true` to force controlled creation through methods on your partial class.

## String ID example

```csharp
[StringId(GenerateConstructorPrivate = true)]
public sealed partial class SecureToken
{
    public static SecureToken Create(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token is required", nameof(token));

        return new SecureToken(token);
    }
}
```

## Guid ID example

```csharp
[GuidId(GenerateConstructorPrivate = true)]
public sealed partial class UserId
{
    public static UserId CreateNew() => new UserId(Guid.NewGuid());

    public static UserId FromString(string value) => new UserId(Guid.Parse(value));
}
```

## Combined ID example

```csharp
[CombinedId(typeof(Guid), "TenantId", typeof(string), "UserCode", GenerateConstructorPrivate = true)]
public sealed partial class TenantUserId
{
    public static TenantUserId Create(Guid tenantId, string userCode)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty", nameof(tenantId));

        return new TenantUserId(tenantId, userCode);
    }
}
```

## Behavior notes

- Generated implicit operators still work and use the private constructor.
- `Unspecified` remains available.
- `CheckValue` is still executed during construction.
- `TypeConverter` support remains available.

See [custom validation](custom-validation-checkvalue.md).
