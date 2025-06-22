namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class CustomCheckValueGuidIdWithPropertyNameFixture : CustomCheckValueFixtureBase<CheckValueGuidIdWithPropertyName, Guid>
    {
        protected override CheckValueGuidIdWithPropertyName CreateId(Guid value)
        {
            return new CheckValueGuidIdWithPropertyName(value);
        }

        protected override Guid GetValidValue()
        {
            return Guid.Parse("3A891E7E-506A-4A05-BF81-2F8D6544CBAB");
        }

        protected override Guid GetInvalidValue()
        {
            return CheckValueGuidIdWithPropertyName.InvalidValue;
        }

        protected override Guid GetExpectedValidValue()
        {
            return CheckValueGuidIdWithPropertyName.ValidValue;
        }

        protected override Guid GetIdValue(CheckValueGuidIdWithPropertyName id)
        {
            return id.Guid;
        }

        protected override CheckValueGuidIdWithPropertyName ImplicitCast(Guid value)
        {
            return value;
        }

        protected override string ConvertToString(Guid value)
        {
            return value.ToString();
        }

        [Test]
        public void InterfaceValue_AccessibleAndMatchesCustomProperty()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var id = new CheckValueGuidIdWithPropertyName(guid);

            // Act
            var customPropertyValue = id.Guid;
            var interfaceValue = ((ITypedIdentifier<Guid>)id).Value;

            // Assert
            Assert.That(interfaceValue, Is.EqualTo(customPropertyValue));
        }
    }
}