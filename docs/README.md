# Documentation

This folder contains detailed documentation for every feature of StrongTypeIdGenerator.

## Start here

- [Getting Started](getting-started.md)
- [String IDs](string-id.md)
- [Guid IDs](guid-id.md)
- [Combined IDs](combined-id.md)

## Feature guides

- [Custom Validation with CheckValue](custom-validation-checkvalue.md)
- [Custom Value Property Name](custom-value-property-name.md)
- [Private Constructors and Factory Methods](private-constructors-and-factories.md)
- [TypeConverter and System.Text.Json Integration](typeconverter-and-json.md)
- [Entity Framework Core Integration](ef-core.md)

## Additional details

- [Design Decisions](design-decisions.md)
- [FAQ](faq.md)

## Package overview

- `StrongTypeIdGenerator` contains attributes, abstractions, and the source generator package wiring.
- `StrongTypeIdGenerator.Json` provides optional `System.Text.Json` support through `TypeConverterJsonConverterFactory`.
- `StrongTypeIdGenerator.EntityFrameworkCore` provides optional EF Core 8+ support via value converters and complex type conventions.

Back to [repository README](../README.md).
