namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class CustomCheckValueCombinedIdFixture
    {
        [Test]
        public void Constructor_ValidValue_CheckValueModifiesValue()
        {
            // Arrange
            CheckValueCombinedId id = null!;
            (TestGuidId TestGuid, string StringId, Guid GuidId) value = (new TestGuidId(Guid.Parse("3A891E7E-506A-4A05-BF81-2F8D6544CBAB")), "3", Guid.Parse("3A891E7E-506A-4A05-BF81-2F8D6544CBAB"));

            // Act
            id = new CheckValueCombinedId(value);

            // Assert
            Assert.That(id.Value, Is.EqualTo(CheckValueCombinedId.ValidValue));
        }

        [Test]
        public void Constructor_CheckValueIsCalled()
        {
            // Arrange
            CheckValueCombinedId id = null!;
            var value = CheckValueCombinedId.InvalidValue;

            // Act
            void Act() => id = new CheckValueCombinedId(value);

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }
    }
}