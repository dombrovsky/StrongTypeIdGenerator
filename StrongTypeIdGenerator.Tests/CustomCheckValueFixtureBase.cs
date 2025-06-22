namespace StrongTypeIdGenerator.Tests
{
    using System.ComponentModel;

    [TestFixture]
    public abstract class CustomCheckValueFixtureBase<TId, TValue>
        where TId : class
    {
        [Test]
        public void Constructor_ValidValue_CheckValueModifiesValue()
        {
            // Arrange
            TId id = null!;
            var validValue = GetValidValue();

            // Act
            id = CreateId(validValue);

            // Assert
            Assert.That(GetIdValue(id), Is.EqualTo(GetExpectedValidValue()));
        }

        [Test]
        public void Constructor_CheckValueIsCalled()
        {
            // Arrange
            TId id = null!;
            var invalidValue = GetInvalidValue();

            // Act
            void Act() => id = CreateId(invalidValue);

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }

        [Test]
        public virtual void ImplicitCast_CheckValueIsCalled()
        {
            // Arrange
            TId id = null!;
            var invalidValue = GetInvalidValue();

            // Act
            void Act() => id = ImplicitCast(invalidValue);

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }

        [Test]
        public virtual void TypeConverter_CheckValueIsCalled()
        {
            // Arrange
            var invalidValue = GetInvalidValue();

            // Act
            void Act() => TypeDescriptor.GetConverter(typeof(TId)).ConvertFromString(ConvertToString(invalidValue));

            // Assert
            Assert.Throws<ArgumentException>(Act);
        }

        protected abstract TId CreateId(TValue value);
        protected abstract TValue GetValidValue();
        protected abstract TValue GetInvalidValue();
        protected abstract TValue GetExpectedValidValue();
        protected abstract TValue GetIdValue(TId id);
        protected abstract TId ImplicitCast(TValue value);
        protected abstract string ConvertToString(TValue value);
    }
}