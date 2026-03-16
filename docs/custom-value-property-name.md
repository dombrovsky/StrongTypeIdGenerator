# Custom Value Property Name

For scalar identifiers (`[StringId]` and `[GuidId]`), you can rename the generated value property using `ValuePropertyName`.

## Example with StringId

```csharp
[StringId(ValuePropertyName = "Code")]
public sealed partial class ProductCode
{
}
```

Generated class exposes `Code` instead of `Value`.

## Example with GuidId

```csharp
[GuidId(ValuePropertyName = "Uuid")]
public sealed partial class ExternalReferenceId
{
}
```

## Notes

- This option is available because `StringIdAttribute` and `GuidIdAttribute` inherit from `BaseScalarIdAttribute`.
- The generated type still implements `ITypedIdentifier<TSelf, TValue>`.
- Combined IDs do not use `ValuePropertyName` because they expose one property per component.

Related: [String IDs](string-id.md), [Guid IDs](guid-id.md), [Combined IDs](combined-id.md).
