namespace StrongTypeIdGenerator.Tests
{
    using System.ComponentModel;

    [TestFixture]
    internal sealed class CustomCheckValueStringIdFixture
    {
        [Test]
        public void Constructor_CheckValueIsCalled()
        {
            // Arrange
            CheckValueStringId id = null!;
            var value = new string('a', 11);

            // Act
            void Act() => id = new CheckValueStringId(value);

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }

        [Test]
        public void Constructor_ValidValue_CheckValueModifiesValue()
        {
            // Arrange
            CheckValueStringId id = null!;
            var value = new string('a', 9);

            // Act
            id = new CheckValueStringId(value);

            // Assert
            Assert.That(id.Value, Is.EqualTo(new string('A', 9)));
        }

        [Test]
        public void ImplicitCast_CheckValueIsCalled()
        {
            // Arrange
            CheckValueStringId id = null!;
            var value = new string('a', 11);

            // Act
            void Act() => id = value;

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }

        [Test]
        public void TypeConverter_CheckValueIsCalled()
        {
            // Arrange
            var value = new string('a', 11);

            // Act
            void Act() => TypeDescriptor.GetConverter(typeof(CheckValueStringId)).ConvertFromString(value);

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }
    }
}