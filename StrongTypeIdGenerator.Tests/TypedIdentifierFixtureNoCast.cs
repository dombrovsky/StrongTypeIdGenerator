namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    public abstract class TypedIdentifierFixtureNoCast<TSut, TSutSubclass, TIdentifier>
        where TSut : ITypedIdentifierNoCast<TSut, TIdentifier>
        where TSutSubclass : TSut
        where TIdentifier : IEquatable<TIdentifier>
    {
        [Test]
        public void Equals_WhenSameId_ReturnsTrue()
        {
            var (id1, _, _) = ProvideIdentifiers();

            var sut1 = CreateSut(id1);
            var sut2 = CreateSut(id1);

            Assert.That(sut1, Is.EqualTo(sut2).And.Not.SameAs(sut2));
        }

        [Test]
        public void GetHashCode_WhenSameId_ReturnsSameValue()
        {
            var (id1, _, _) = ProvideIdentifiers();

            var sut1 = CreateSut(id1);
            var sut2 = CreateSut(id1);

            Assert.That(sut1.GetHashCode(), Is.EqualTo(sut2.GetHashCode()));
        }

        [Test]
        public void Equals_WhenNotSameId_ReturnsFalse()
        {
            var (id1, id2, _) = ProvideIdentifiers();

            var sut1 = CreateSut(id1);
            var sut2 = CreateSut(id2);

            Assert.That(sut1, Is.Not.EqualTo(sut2));
        }

        [Test]
        public void Equals_WhenOneNull_ReturnsFalse()
        {
            var (id1, _, idNull) = ProvideIdentifiers();

            var sut1 = CreateSut(id1);
            var sut2 = CreateSut(idNull);

            Assert.That(sut1, Is.Not.EqualTo(sut2));
        }

        [Test]
        public void Equals_WhenBothNull_ReturnsTrue()
        {
            var (_, _, idNull) = ProvideIdentifiers();

            var sut1 = CreateSut(idNull);
            var sut2 = CreateSut(idNull);

            Assert.That(sut1, Is.EqualTo(sut2));
#if NET7_0_OR_GREATER
            Assert.That(sut1, Is.EqualTo(TSut.Unspecified));
#endif
        }

        [Test]
        public void Equals_WithSubclass_ReturnsFalse()
        {
            var (id1, _, _) = ProvideIdentifiers();

            var sut1 = CreateSut(id1);
            var sut2 = CreateSubclassSut(id1);

            Assert.That(sut1, Is.Not.EqualTo(sut2));
        }

        [Test]
        public void Equals_WithBaseClass_ReturnsFalse()
        {
            var (id1, _, _) = ProvideIdentifiers();

            var sut1 = CreateSubclassSut(id1);
            var sut2 = CreateSut(id1);

            Assert.That(sut1, Is.Not.EqualTo(sut2));
        }

        [Test]
        public void Value_EqualsToInputValue()
        {
            var (id1, id2, unspecified) = ProvideIdentifiers();

            var sut1 = CreateSut(id1);
            var sut2 = CreateSut(id2);
            var sutUnspecified = CreateSut(unspecified);

            Assert.That(GetIdentifier(sut1), Is.EqualTo(id1));
            Assert.That(GetIdentifier(sut2), Is.EqualTo(id2));
            Assert.That(GetIdentifier(sutUnspecified), Is.EqualTo(unspecified));
        }

        protected abstract (TIdentifier Id1, TIdentifier Id2, TIdentifier Unspecified) ProvideIdentifiers();

        protected abstract TSut CreateSut(TIdentifier id);

        protected abstract TSutSubclass CreateSubclassSut(TIdentifier id);

        protected virtual TIdentifier GetIdentifier(TSut sut)
        {
            return sut.Value;
        }
    }
}