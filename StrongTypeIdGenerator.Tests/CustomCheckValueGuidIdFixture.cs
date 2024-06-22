namespace StrongTypeIdGenerator.Tests
{
    using System.ComponentModel;

    [TestFixture]
    internal sealed class CustomCheckValueGuidIdFixture
    {
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