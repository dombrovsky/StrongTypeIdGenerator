namespace StrongTypeIdGenerator.Tests
{
    using System.ComponentModel;

    [TestFixture]
    internal sealed class CustomCheckValueGuidIdFixture
    {
        [Test]
        public void Constructor_ValidValue_CheckValueModifiesValue()
        {
            // Arrange
            CheckValueGuidId id = null!;
            var value = Guid.Parse("3A891E7E-506A-4A05-BF81-2F8D6544CBAB");

            // Act
            id = new CheckValueGuidId(value);

            // Assert
            Assert.That(id.Value, Is.EqualTo(CheckValueGuidId.ValidValue));
        }

        [Test]
        public void Constructor_CheckValueIsCalled()
        {
            // Arrange
            CheckValueGuidId id = null!;
            var value = CheckValueGuidId.InvalidValue;

            // Act
            void Act() => id = new CheckValueGuidId(value);

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }

        [Test]
        public void ImplicitCast_CheckValueIsCalled()
        {
            // Arrange
            CheckValueGuidId id = null!;
            var value = CheckValueGuidId.InvalidValue;

            // Act
            void Act() => id = value;

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }

        [Test]
        public void TypeConverter_CheckValueIsCalled()
        {
            // Arrange
            var value = CheckValueGuidId.InvalidValue;

            // Act
            void Act() => TypeDescriptor.GetConverter(typeof(CheckValueGuidId)).ConvertFromString(value.ToString());

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }
    }
}