# Combined IDs

Use `[CombinedId]` for composite identifiers made from multiple components.

## Basic example

```csharp
using StrongTypeIdGenerator;

[CombinedId(typeof(Guid), "TenantId", typeof(string), "UserCode")]
public sealed partial class TenantUserId
{
}
```

This generates a class with component properties `TenantId` and `UserCode`.

## Supported component counts

`CombinedIdAttribute` supports 2 to 8 components.

## Components can be primitive or typed IDs

You can combine primitives (for example `string`, `Guid`, `int`) and other StrongTypeId-generated types.

```csharp
[GuidId]
public sealed partial class CompanyId
{
}

[StringId]
public sealed partial class DepartmentId
{
}

[CombinedId(typeof(CompanyId), "CompanyId", typeof(DepartmentId), "DepartmentId", typeof(int), "Revision")]
public sealed partial class CompanyDepartmentRevisionId
{
}
```

## CheckValue for combined IDs

Define `CheckValue` with parameters matching constructor components. Return a tuple with validated or transformed values.

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

        return (tenantId, userCode.Trim().ToUpperInvariant());
    }
}
```

## Private constructor mode

`GenerateConstructorPrivate = true` also works for combined IDs.

See [custom validation](custom-validation-checkvalue.md) and [private constructors](private-constructors-and-factories.md).
