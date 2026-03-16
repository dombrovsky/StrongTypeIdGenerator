# FAQ

## Why use strongly typed IDs instead of primitive values?

They prevent accidental mixing of unrelated IDs (for example `OrderId` vs `CustomerId`) and make intent explicit in APIs.

## Does this support composite keys?

Yes. Use `[CombinedId(...)]` with 2 to 8 components.

## Can I validate ID values?

Yes. Define a `private static CheckValue(...)` method on your partial class.

See [custom validation guide](custom-validation-checkvalue.md).

## Can I force factory-only creation?

Yes. Set `GenerateConstructorPrivate = true` on the attribute and expose your own static methods.

See [private constructors and factories](private-constructors-and-factories.md).

## How do I serialize IDs with System.Text.Json?

Install `StrongTypeIdGenerator.Json` and add `TypeConverterJsonConverterFactory` to `JsonSerializerOptions.Converters`.

See [TypeConverter and JSON guide](typeconverter-and-json.md).

## Is this tied to DDD only?

No. It is useful anywhere type-safe identifiers improve correctness, including APIs, data pipelines, and internal services.

## Is this similar to Andrew Lock's StronglyTypedId?

Yes, and this project acknowledges that inspiration. StrongTypeIdGenerator emphasizes reference-type identifiers and includes built-in combined identifier generation.

Back to [docs index](README.md).
