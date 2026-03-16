namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class GuidIdEntity
    {
        public required TestGuidId Id { get; set; }

        public required string Name { get; set; }
    }

    internal sealed class StringIdEntity
    {
        public required TestStringId Id { get; set; }

        public required string Value { get; set; }
    }

    internal sealed class CombinedIdEntity
    {
        public int Id { get; set; }

        public required TestCombinedId CompositeId { get; set; }
    }

    internal sealed class NullableGuidIdEntity
    {
        public int Id { get; set; }

        public TestGuidId? OptionalId { get; set; }
    }

    internal sealed class NullableStringIdEntity
    {
        public int Id { get; set; }

        public TestStringId? OptionalId { get; set; }
    }

    internal sealed class CheckValueGuidIdEntity
    {
        public int Id { get; set; }

        public required CheckValueGuidId StrongId { get; set; }
    }

    internal sealed class CheckValueStringIdEntity
    {
        public int Id { get; set; }

        public required CheckValueStringId StrongId { get; set; }
    }

    internal sealed class PrivateGuidIdEntity
    {
        public int Id { get; set; }

        public required TestGuidIdPrivateConstructor StrongId { get; set; }
    }

    internal sealed class GuidIdWithPropertyNameEntity
    {
        public required TestGuidIdWithPropertyName Id { get; set; }

        public required string Name { get; set; }
    }

    internal sealed class StringIdWithPropertyNameEntity
    {
        public required TestStringIdWithPropertyName Id { get; set; }

        public required string Name { get; set; }
    }

    internal sealed class NonKeyStrongIdEntity
    {
        public int Id { get; set; }

        public required TestGuidId ExternalId { get; set; }

        public required string Name { get; set; }
    }
}
