namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class CustomCheckValueStringIdWithPropertyNameFixture : CustomCheckValueFixtureBase<CheckValueStringIdWithPropertyName, string>
    {
        protected override CheckValueStringIdWithPropertyName CreateId(string value)
        {
            return new CheckValueStringIdWithPropertyName(value);
        }

        protected override string GetValidValue()
        {
            return new string('a', 9);
        }

        protected override string GetInvalidValue()
        {
            return new string('a', 11);
        }

        protected override string GetExpectedValidValue()
        {
            return new string('A', 9);
        }

        protected override string GetIdValue(CheckValueStringIdWithPropertyName id)
        {
            return id.Text;
        }

        protected override CheckValueStringIdWithPropertyName ImplicitCast(string value)
        {
            return value;
        }

        protected override string ConvertToString(string value)
        {
            return value;
        }

        [Test]
        public void InterfaceValue_AccessibleAndMatchesCustomProperty()
        {
            // Arrange
            var text = "TestValue";
            var id = new CheckValueStringIdWithPropertyName(text);

            // Act
            var customPropertyValue = id.Text;
            var interfaceValue = ((ITypedIdentifier<string>)id).Value;

            // Assert
            Assert.That(interfaceValue, Is.EqualTo(customPropertyValue));
            Assert.That(interfaceValue, Is.EqualTo("TESTVALUE")); // Uppercase due to CheckValue
        }
    }
}