# Design Decisions

This document explains the key design choices in StrongTypeIdGenerator.

## Reference types by default

Generated identifiers are reference types. The project prioritizes protecting invariants over minimizing allocations.

Benefits:

- Controlled instantiation and validation
- Consistent null semantics in common .NET application code
- Flexible extension through partial classes and factory methods

## Validation is part of the ID type

The `CheckValue` hook allows ID-local validation and normalization. This keeps correctness close to the type itself.

## No hard dependency on a serializer

The core package does not depend directly on `System.Text.Json` or Newtonsoft.Json converters. Instead, generated IDs expose `TypeConverter` support, and optional integration is provided by `StrongTypeIdGenerator.Json`.

## Generated APIs aim for practical ergonomics

Generated classes include conversions, formatting, equality, and comparison to reduce repetitive handwritten boilerplate.

## Package split

- Main package: generation and abstractions.
- JSON package: optional serializer bridge.

This helps use cases targeting broad frameworks, including shared libraries.

Continue with [FAQ](faq.md) or return to [docs index](README.md).
