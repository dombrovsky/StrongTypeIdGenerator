# StrongTypeIdGenerator

Source generator that helps you create strongly-typed identifiers in your C# projects. It supports Guid, string-based, and combined identifiers.

[![NuGet](https://img.shields.io/nuget/v/TaskFlow.svg)](https://www.nuget.org/packages/StrongTypeIdGenerator/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

---

## Getting Started
Define your ID type:
```csharp
[StringId]
partial class FooId
{
}
```
The generator will produce:
```csharp
[System.ComponentModel.TypeConverter(typeof(FooIdConverter))]
partial class FooId : ITypedIdentifier<FooId, string>
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

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator FooId?(string? value) { ... }

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator string?(FooId? value) { ... }

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
### Idenifier type should be a reference type, not a value type.
Being able to protect invariants and not allow instance of id with invalid value to exist, is chosen over avoiding additional object allocation.
### Ability to define custom id precondition checks.
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
### No dependency on serialization libraries.
StrongTypeIdGenerator only defines `System.ComponentModel.TypeConverter` that can convert to and from `string`. No `System.Text.Json` or `Newtonsoft.Json` or EF Core converters defined.

This way Id types can be defined in `netstandard2.0` libraries with no additional dependencies.

The proposed way to use generated Id classes in serialization e.g. with `System.Text.Json` is to provide [custom JsonConverterFactory](https://github.com/dombrovsky/StrongTypeIdGenerator/blob/main/StrongTypeIdGenerator.Json/TypeConverterJsonConverterFactory.cs) to serializer, that would utilize generated `TypeConverter`.

## Usage
Define ID types easily:
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
Or use `[CombinedId]` to create a composite identifier:
```csharp
[CombinedId(typeof(BarId), "BarId", typeof(string), "StringId", typeof(Guid), "GuidId", typeof(int), "IntId")]
public partial class FooBarCombinedId
{
}
```
Combined indentifier supports other StrongTypeId generated types and primitives e.g. `string`, `int`, `Guid`.
### Custom Validation
You can add custom validation logic to your ID types by defining a `CheckValue` method. The method will be called from the constructor and can validate (throw exceptions) or modify the input value.
#### String and Guid IDs
For `StringId` and `GuidId`, define a method with this signature:
```csharp
private static string CheckValue(string value)
private static Guid CheckValue(Guid value)
{
    // Validation logic here
    return value;
}
```
#### Combined IDs
For `CombinedId`, the `CheckValue` method should accept individual parameters matching the constructor and return a tuple with the validated values:
```csharp
private static (BarId, string, Guid, int) CheckValue(BarId barId, string stringId, Guid guidId, int intId)
{
    // Validation logic here
    return (barId, stringId, guidId, intId);
}
```
The `CheckValue` method is called from the constructor and its result is used to set the properties of the ID class.
### Custom Value Property Name
You can customize the name of the property that holds the identifier's value by setting the `ValuePropertyName` property on the attribute:
```csharp
[GuidId(ValuePropertyName = "Uuid")]
public sealed partial class BarId
{
}
```
And generated class fill get 'Uuid' property instead of 'Value':
```csharp
public sealed partial class BarId
{
  ...
  public Guid Uuid { get; }
  ...
}
```
When using a custom property name, the generated class will still implement the `ITypedIdentifier<T>` interface by providing an explicit implementation for the `Value` property that forwards to your custom property.
## Acknowledgements
Inspired by a great library https://github.com/andrewlock/StronglyTypedId.
