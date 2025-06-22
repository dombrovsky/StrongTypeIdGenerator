namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class TestGuidIdWithPropertyNameFixture : TypedIdentifierFixture<TestGuidIdWithPropertyName, DerivedTestGuidIdWithPropertyName, Guid>
    {
        protected override (Guid Id1, Guid Id2, Guid Unspecified) ProvideIdentifiers()
        {
            return (Guid.Parse("30704DE1-8F21-460B-B3F1-9027F4D5F7B6"), Guid.Parse("ECA00066-B8BF-4BAF-8370-3240E7F64356"), Guid.Empty);
        }

        protected override TestGuidIdWithPropertyName CreateSut(Guid id)
        {
            return new TestGuidIdWithPropertyName(id);
        }

        protected override DerivedTestGuidIdWithPropertyName CreateSubclassSut(Guid id)
        {
            return new DerivedTestGuidIdWithPropertyName(id);
        }

        protected override Guid GetIdentifier(TestGuidIdWithPropertyName sut)
        {
            return sut.Uuid;
        }
    }
}