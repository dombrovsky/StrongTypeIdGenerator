# TypeConverter and System.Text.Json Integration

Generated identifiers are decorated with a `TypeConverter`, which enables string conversion and integration scenarios.

## TypeConverter in generated IDs

Each generated identifier type includes an inner converter implementation and `TypeConverterAttribute` wiring.

This enables:

- conversion from string to identifier
- conversion from identifier to string
- use in frameworks relying on `TypeConverter`

## System.Text.Json package

Install optional package:

```bash
dotnet add package StrongTypeIdGenerator.Json
```

Configure serializer:

```csharp
using System.Text.Json;
using StrongTypeIdGenerator.Json;

var options = new JsonSerializerOptions();
options.Converters.Add(new TypeConverterJsonConverterFactory());
```

## ASP.NET Core registration example

```csharp
services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new TypeConverterJsonConverterFactory());
});
```

## Dictionary key support

`TypeConverterJsonConverterFactory` supports reading/writing IDs as JSON property names, so strongly typed IDs can be used as dictionary keys.

## Validation interaction

During deserialization, conversion flows through your generated constructor, so `CheckValue` validations and normalizations are applied.

Return to [docs index](README.md).
