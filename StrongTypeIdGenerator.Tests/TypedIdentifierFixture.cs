namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    public abstract class TypedIdentifierFixture<TSut, TSutSubclass, TIdentifier> : TypedIdentifierFixtureNoCast<TSut, TSutSubclass, TIdentifier>
        where TSut : ITypedIdentifier<TSut, TIdentifier>
        where TSutSubclass : TSut
        where TIdentifier : IEquatable<TIdentifier>
    {
#if NET7_0_OR_GREATER
        [Test]
        public void Equals_WhenSameIdButOneImplicitlyCasted_ReturnsTrue()
        {
            var (id1, _, _) = ProvideIdentifiers();

            TSut sut1 = id1;
            var sut2 = CreateSut(id1);

            Assert.That(sut1, Is.EqualTo(sut2).And.Not.SameAs(sut2));
        }
#endif
    }
}