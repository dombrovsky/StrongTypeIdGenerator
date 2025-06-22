namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class TestCombinedIdWithPropertyNameFixture : TypedIdentifierFixtureNoCast<TestCombinedIdWithPropertyName, DerivedTestCombinedIdWithPropertyName, (TestGuidIdWithPropertyName TestGuid, string StringId)>
    {
        protected override DerivedTestCombinedIdWithPropertyName CreateSubclassSut((TestGuidIdWithPropertyName TestGuid, string StringId) id)
        {
            return new DerivedTestCombinedIdWithPropertyName(id);
        }

        protected override TestCombinedIdWithPropertyName CreateSut((TestGuidIdWithPropertyName TestGuid, string StringId) id)
        {
            return new TestCombinedIdWithPropertyName(id);
        }

        protected override ((TestGuidIdWithPropertyName TestGuid, string StringId) Id1, (TestGuidIdWithPropertyName TestGuid, string StringId) Id2, (TestGuidIdWithPropertyName TestGuid, string StringId) Unspecified) ProvideIdentifiers()
        {
            return (
                (new TestGuidIdWithPropertyName(Guid.Parse("30704DE1-8F21-460B-B3F1-9027F4D5F7B6")), "1"),
                (new TestGuidIdWithPropertyName(Guid.Parse("ECA00066-B8BF-4BAF-8370-3240E7F64356")), "2"),
                (new TestGuidIdWithPropertyName(Guid.Empty), string.Empty)
            );
        }

        protected override (TestGuidIdWithPropertyName TestGuid, string StringId) GetIdentifier(TestCombinedIdWithPropertyName sut)
        {
            return sut.Data;
        }
    }
}