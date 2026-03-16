# StrongTypeIdGenerator

StrongTypeIdGenerator is a C# source generator for strongly typed identifiers.
It helps prevent primitive ID mix-ups by generating domain-specific ID types for `string`, `Guid`, and combined (composite) keys.

[![NuGet Core](https://img.shields.io/nuget/v/StrongTypeIdGenerator.svg?label=NuGet%20Core)](https://www.nuget.org/packages/StrongTypeIdGenerator/)
[![NuGet Json](https://img.shields.io/nuget/v/StrongTypeIdGenerator.Json.svg?label=NuGet%20Json)](https://www.nuget.org/packages/StrongTypeIdGenerator.Json/)
[![NuGet EF Core](https://img.shields.io/nuget/v/StrongTypeIdGenerator.EntityFrameworkCore.svg?label=NuGet%20EF%20Core)](https://www.nuget.org/packages/StrongTypeIdGenerator.EntityFrameworkCore/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Why this library

- Removes boilerplate for strongly typed IDs.
- Prevents accidental mixing of identifiers across boundaries.
- Keeps validation close to the ID type via `CheckValue` hooks.
- Supports factory-oriented design with optional private constructors.
- Works with `System.ComponentModel.TypeConverter` out of the box.

Design decisions:

- **Reference types by design.** This project prioritizes invariant safety and controlled construction over minimizing allocations, so invalid IDs are harder to create and propagate.
- **Built-in precondition hooks.** If an ID class defines `CheckValue(...)`, the method is called from the generated constructor and can validate or normalize input.
- **Serializer-agnostic core.** The main package only relies on `System.ComponentModel.TypeConverter` and has no direct dependency on `System.Text.Json`, Newtonsoft.Json, or EF Core converters.
- **netstandard2.0-friendly usage.** IDs can live in `netstandard2.0` libraries without extra serialization dependencies. For `System.Text.Json`, use the optional `StrongTypeIdGenerator.Json` package. For EF Core, use the optional `StrongTypeIdGenerator.EntityFrameworkCore` package.
- **First-class composite identifiers.** `CombinedId` exists for real-world composite business keys, avoiding ad-hoc wrapper implementations.

## Main features

- `StringId` and `GuidId` generation with value semantics.
- `CombinedId` generation for composite identifiers.
- Generated equality, comparison, formatting, and operators.
- Optional custom value property name for scalar identifiers.
- Optional constructor privacy (`GenerateConstructorPrivate = true`).
- Optional `System.Text.Json` integration package.
- Optional EF Core integration package.

## Quick start

Install package:

```bash
dotnet add package StrongTypeIdGenerator
```

Define identifiers:

```csharp
using StrongTypeIdGenerator;

[StringId]
public sealed partial class OrderId
{
}

[GuidId]
public sealed partial class CustomerId
{
}

[CombinedId(typeof(CustomerId), "CustomerId", typeof(OrderId), "OrderId")]
public sealed partial class CustomerOrderId
{
}
```

<details>
<summary>Generated structure (example for <code>OrderId</code>)</summary>

```csharp
[TypeConverter(typeof(OrderIdConverter))]
public sealed partial class OrderId : ITypedIdentifier<OrderId, string>
{
	public OrderId(string value) { ... }
	public static OrderId Unspecified { get; } = ...;

	public string Value { get; }

	public static implicit operator OrderId?(string? value) { ... }
	public static implicit operator string?(OrderId? value) { ... }

	public bool Equals(OrderId? other) { ... }
	public int CompareTo(OrderId? other) { ... }
	public override bool Equals(object? obj) { ... }
	public override int GetHashCode() { ... }
	public override string ToString() { ... }
	public string ToString(string? format, IFormatProvider? provider) { ... }

	public static bool operator ==(OrderId left, OrderId right) { ... }
	public static bool operator !=(OrderId left, OrderId right) { ... }
	public static bool operator <(OrderId left, OrderId right) { ... }
	public static bool operator <=(OrderId left, OrderId right) { ... }
	public static bool operator >(OrderId left, OrderId right) { ... }
	public static bool operator >=(OrderId left, OrderId right) { ... }

	private sealed partial class OrderIdConverter : TypeToStringConverter<OrderId>
	{
		protected override string? InternalConvertToString(OrderId value) { ... }
		protected override OrderId? InternalConvertFromString(string value) { ... }
	}
}
```

</details>

The generator creates immutable reference-type identifiers with:

- constructor (public or private based on attribute options)
- typed value/component properties
- `Unspecified`
- `Equals`, `GetHashCode`, comparison operators
- `ToString` and format overloads
- implicit conversion operators
- nested `TypeConverter`

## Optional JSON and EF Core integration

For `System.Text.Json`, install:

```bash
dotnet add package StrongTypeIdGenerator.Json
```

Configure serializer:

```csharp
using StrongTypeIdGenerator.Json;

var options = new JsonSerializerOptions();
options.Converters.Add(new TypeConverterJsonConverterFactory());
```

For EF Core integration, install `StrongTypeIdGenerator.EntityFrameworkCore` and see the [EF Core Integration](docs/ef-core.md) guide.

## Documentation

Detailed docs are in the docs folder:

- [Docs Index](docs/README.md)
- [Getting Started](docs/getting-started.md)
- [String IDs](docs/string-id.md)
- [Guid IDs](docs/guid-id.md)
- [Combined IDs](docs/combined-id.md)
- [Custom Validation (CheckValue)](docs/custom-validation-checkvalue.md)
- [Custom Value Property Name](docs/custom-value-property-name.md)
- [Private Constructors and Factories](docs/private-constructors-and-factories.md)
- [TypeConverter and System.Text.Json](docs/typeconverter-and-json.md)
- [EF Core Integration](docs/ef-core.md)
- [Design Decisions](docs/design-decisions.md)
- [FAQ](docs/faq.md)

## Acknowledgements

Inspired by [StronglyTypedId](https://github.com/andrewlock/StronglyTypedId).
