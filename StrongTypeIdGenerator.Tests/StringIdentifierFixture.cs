namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class StringIdentifierFixture : TypedIdentifierFixture<TestStringId, DerivedTestStringId, string>
    {
        protected override (string Id1, string Id2, string Unspecified) ProvideIdentifiers()
        {
            return ("1", "2", string.Empty);
        }

        protected override TestStringId CreateSut(string id)
        {
            return new TestStringId(id);
        }

        protected override DerivedTestStringId CreateSubclassSut(string id)
        {
            return new DerivedTestStringId(id);
        }
    }
}