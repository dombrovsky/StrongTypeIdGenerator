# Entity Framework Core Integration

The `StrongTypeIdGenerator.EntityFrameworkCore` package wires up EF Core value converters and complex type conventions for all generated identifier types automatically.

## Requirements

- .NET 8 or later
- EF Core 8 or later

## Installation

```bash
dotnet add package StrongTypeIdGenerator.EntityFrameworkCore
```

## Registration

Call `UseStrongTypeIds()` in two places.

### On `DbContextOptionsBuilder`

Registers the custom `IValueConverterSelector` so EF Core picks up converters for `GuidId` and `StringId` properties automatically.

```csharp
services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseStrongTypeIds();
});
```

### On `ModelConfigurationBuilder`

Registers the convention that maps `CombinedId` types as [EF Core complex types](https://learn.microsoft.com/en-us/ef/core/modeling/complex-types).

```csharp
public sealed class AppDbContext : DbContext
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.UseStrongTypeIds();
    }
}
```

Both calls are independent — use only the one(s) you need.

## When another library replaces IValueConverterSelector

`UseStrongTypeIds()` on `DbContextOptionsBuilder` replaces EF Core's `IValueConverterSelector`. If another library already replaced that service with a different implementation, `UseStrongTypeIds()` throws.

Workaround:

- Keep `configurationBuilder.UseStrongTypeIds()` for `CombinedId` complex type mapping.
- Configure scalar strong IDs (`GuidId`/`StringId`) explicitly in `OnModelCreating` with `ModelBuilder` and `HasStrongTypeIdConversion(...)`.

Example:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Order>()
        .Property(x => x.Id)
        .HasStrongTypeIdConversion<OrderId, Guid>();
}
```

## What is configured automatically

| ID type | EF Core mapping | Provider column type |
|---|---|---|
| `GuidId` | Value converter | `uniqueidentifier` / `TEXT` (SQLite) |
| `StringId` | Value converter | `nvarchar` / `TEXT` (SQLite) |
| `CombinedId` | EF Core complex type | Multiple columns, one per component |

No manual `HasConversion<T>()` or `OwnsOne()` calls are needed.

## HasStrongTypeIdConversion and CombinedId

`HasStrongTypeIdConversion(...)` is intended for scalar strong IDs (`GuidId`/`StringId`) only.

- Use it directly for scalar properties (for example, `Order.Id`, `Invoice.RelatedOrderId`).
- Do not apply it to the `CombinedId` property itself.
- If you are in the workaround path (another library owns `IValueConverterSelector`), map the `CombinedId` as a complex type and apply `HasStrongTypeIdConversion(...)` only to scalar strong-ID components inside that complex type.

Example (component mapping in workaround path):

```csharp
var keyBuilder = modelBuilder.Entity<Subscription>()
    .ComplexProperty(x => x.Key);

keyBuilder.Property(x => x.TenantId)
    .HasStrongTypeIdConversion<TenantId, Guid>();

keyBuilder.Property(x => x.ExternalRef);
```

## GuidId and StringId as primary keys

```csharp
[GuidId]
public partial class OrderId { }

public class Order
{
    public required OrderId Id { get; set; }
    public required string Description { get; set; }
}

// In OnModelCreating:
modelBuilder.Entity<Order>().HasKey(x => x.Id);
```

## CombinedId as an owned value object

`CombinedId` is mapped as a complex type and stored as multiple columns on the owning entity's table. It cannot be used as a primary key (EF Core complex types do not support this).

```csharp
[CombinedId(typeof(TenantId), "TenantId", typeof(string), "ExternalRef")]
public partial class ExternalKey { }

public class Subscription
{
    public int Id { get; set; }
    public required ExternalKey Key { get; set; }
}
```

The columns produced follow EF Core's default complex type naming convention: `Key_TenantId`, `Key_ExternalRef`.

## Nullable strong ID columns

Reference-type strong IDs can be declared nullable on an entity and EF Core maps them as nullable columns:

```csharp
public class Invoice
{
    public int Id { get; set; }
    public OrderId? RelatedOrderId { get; set; }
}
```

## Private constructor IDs

IDs generated with `GenerateConstructorPrivate = true` are fully supported. The value converter uses the generated implicit operator for materialisation, so no public constructor is required.

## Validation interaction

Materialisation flows through the generated implicit conversion operator, which calls the constructor (and therefore `CheckValue`) just as direct construction does. Invalid stored values will surface as exceptions during entity materialisation.

Return to [docs index](README.md).
