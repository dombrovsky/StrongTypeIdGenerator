namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class CombinedIdentifierFixture : TypedIdentifierFixtureNoCast<TestCombinedId, DerivedTestCombinedId, (TestGuidId TestGuid, string StringId, Guid GuidId, int IntId)>
    {
        [Test]
        public void Deconstruct_ReturnsValues()
        {
            var (id1, _, _) = ProvideIdentifiers();

            var sut = CreateSut(id1);

            var (testGuid, stringId, guidId, intId) = sut;

            Assert.That(testGuid, Is.EqualTo(id1.TestGuid));
            Assert.That(stringId, Is.EqualTo(id1.StringId));
            Assert.That(guidId, Is.EqualTo(id1.GuidId));
            Assert.That(intId, Is.EqualTo(id1.IntId));
        }

        protected override DerivedTestCombinedId CreateSubclassSut((TestGuidId TestGuid, string StringId, Guid GuidId, int IntId) id)
        {
            return new DerivedTestCombinedId(id);
        }

        protected override TestCombinedId CreateSut((TestGuidId TestGuid, string StringId, Guid GuidId, int IntId) id)
        {
            return new TestCombinedId(id);
        }

        protected override ((TestGuidId TestGuid, string StringId, Guid GuidId, int IntId) Id1, (TestGuidId TestGuid, string StringId, Guid GuidId, int IntId) Id2, (TestGuidId TestGuid, string StringId, Guid GuidId, int IntId) Unspecified) ProvideIdentifiers()
        {
            return (
                (new TestGuidId(Guid.Parse("30704DE1-8F21-460B-B3F1-9027F4D5F7B6")), "1", Guid.Parse("30704DE1-8F21-460B-B3F1-9027F4D5F7B6"), 1),
                (new TestGuidId(Guid.Parse("ECA00066-B8BF-4BAF-8370-3240E7F64356")), "2", Guid.Parse("ECA00066-B8BF-4BAF-8370-3240E7F64356"), 2),
                (new TestGuidId(Guid.Empty), string.Empty, Guid.Empty, 0)
            );
        }
    }
}