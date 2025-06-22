namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class TestTestStringIdWithPropertyNameFixture : TypedIdentifierFixture<TestStringIdWithPropertyName, DerivedTestStringIdWithPropertyName, string>
    {
        protected override (string Id1, string Id2, string Unspecified) ProvideIdentifiers()
        {
            return ("1", "2", string.Empty);
        }

        protected override TestStringIdWithPropertyName CreateSut(string id)
        {
            return new TestStringIdWithPropertyName(id);
        }

        protected override DerivedTestStringIdWithPropertyName CreateSubclassSut(string id)
        {
            return new DerivedTestStringIdWithPropertyName(id);
        }

        protected override string GetIdentifier(TestStringIdWithPropertyName sut)
        {
            return sut.Text;
        }
    }
}