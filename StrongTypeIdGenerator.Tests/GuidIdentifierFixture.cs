namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class GuidIdentifierFixture : TypedIdentifierFixture<TestGuidId, DerivedTestGuidId, Guid>
    {
        protected override (Guid Id1, Guid Id2, Guid Unspecified) ProvideIdentifiers()
        {
            return (Guid.Parse("30704DE1-8F21-460B-B3F1-9027F4D5F7B6"), Guid.Parse("ECA00066-B8BF-4BAF-8370-3240E7F64356"), Guid.Empty);
        }

        protected override TestGuidId CreateSut(Guid id)
        {
            return new TestGuidId(id);
        }

        protected override DerivedTestGuidId CreateSubclassSut(Guid id)
        {
            return new DerivedTestGuidId(id);
        }
    }
}