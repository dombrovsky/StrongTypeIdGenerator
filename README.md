# StrongTypeIdGenerator

StrongTypeIdGenerator is a source generator that helps you create strongly-typed identifiers in your C# projects. It supports Guid and string-based identifiers.

Define this:
```csharp
[StringId]
partial class FooId
{
}
```
and get this generated:
```csharp
[System.ComponentModel.TypeConverter(typeof(FooIdConverter))]
partial class FooId : IEquatable<FooId>, IComparable<FooId>, IFormattable
{
    public FooId(string value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        Value = value;
    }

    public static FooId Unspecified { get; } = new FooId(string.Empty);

    public string Value { get; }

    public static implicit operator FooId(string value) { ... }

    public static implicit operator string(FooId value) { ... }

    public bool Equals(FooId? other) { ... }

    public int CompareTo(FooId? other) { ... }

    public override bool Equals(object? obj) { ... }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

    public string ToString(string? format, IFormatProvider? formatProvider) => Value;

    public static bool operator ==(FooId left, FooId right) { ... }

    public static bool operator !=(FooId left, FooId right) { ... }

    public static bool operator <(FooId left, FooId right) { ... }

    public static bool operator <=(FooId left, FooId right) { ... }

    public static bool operator >(FooId left, FooId right) { ... }

    public static bool operator >=(FooId left, FooId right) { ... }

    private sealed partial class FooIdConverter : TypeToStringConverter<FooId>
    {
        protected override string? InternalConvertToString(FooId value)
        {
            return value.Value;
        }

        protected override FooId? InternalConvertFromString(string value)
        {
            return new FooId(value);
        }
    }
}
```

## Design decisions
There are a few opinionated principles regarding what strong type identifiers should and should not do, which may be different from similar libraries and are reasons this project existence.
#### Idenifier type should be a reference type, not a value type.
Being able to protect invariants and not allow instance of id with invalid value to exist, is chosen over avoiding additional object allocation.
#### No dependency on serialization libraries.
StrongTypeIdGenerator only defines `System.ComponentModel.TypeConverter` that can convert to and from `string`. No `System.Text.Json` or `Newtonsoft.Json` or EF Core converters defined.

This way Id types can be defined in `netstandard2.0` libraries with no additional dependencies.

The proposed way to use generated Id classes in serialization e.g. with `System.Text.Json` is to provide [custom JsonConverterFactory](https://github.com/dombrovsky/StrongTypeIdGenerator/blob/main/StrongTypeIdGenerator.Json/TypeConverterJsonConverterFactory.cs) to serializer, that would utilize generated `TypeConverter`.
#### Ability to define custom id precondition checks.
If Id class defines method `static string CheckValue(string value)`, that method would be called from generated constructor.
```csharp
[StringId]
partial class FooId
{
    private static string CheckValue(string value)
    {
        if (value.Length > 10)
        {
            throw new ArgumentException("Value is too long", nameof(value));
        }

        return value;
    }
}
```

## Installation
Add nuget package https://www.nuget.org/packages/StrongTypeIdGenerator.
```
<PackageReference Include="StrongTypeIdGenerator" Version="1.0.0" />
```

## Usage
Just define your Id type like that
```csharp
[StringId]
public sealed partial class FooId
{
}

[GuidId]
public sealed partial class BarId
{
}
```

## Acknowledgements
Inspired by a great library https://github.com/andrewlock/StronglyTypedId.
