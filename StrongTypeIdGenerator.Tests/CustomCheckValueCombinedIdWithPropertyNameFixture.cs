namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class CustomCheckValueCombinedIdWithPropertyNameFixture 
        : CustomCheckValueFixtureBase<CheckValueCombinedIdWithPropertyName, (TestGuidId TestGuid, string StringId, Guid GuidId)>
    {
        protected override CheckValueCombinedIdWithPropertyName CreateId((TestGuidId TestGuid, string StringId, Guid GuidId) value)
        {
            return new CheckValueCombinedIdWithPropertyName(value);
        }

        protected override (TestGuidId TestGuid, string StringId, Guid GuidId) GetValidValue()
        {
            return (new TestGuidId(Guid.Parse("3A891E7E-506A-4A05-BF81-2F8D6544CBAB")), "3", Guid.Parse("3A891E7E-506A-4A05-BF81-2F8D6544CBAB"));
        }

        protected override (TestGuidId TestGuid, string StringId, Guid GuidId) GetInvalidValue()
        {
            return CheckValueCombinedIdWithPropertyName.InvalidValue;
        }

        protected override (TestGuidId TestGuid, string StringId, Guid GuidId) GetExpectedValidValue()
        {
            return CheckValueCombinedIdWithPropertyName.ValidValue;
        }

        protected override (TestGuidId TestGuid, string StringId, Guid GuidId) GetIdValue(CheckValueCombinedIdWithPropertyName id)
        {
            return id.Data;
        }

        // Override the base test methods that aren't applicable for combined IDs
        public override void ImplicitCast_CheckValueIsCalled() { }
        public override void TypeConverter_CheckValueIsCalled() { }

        protected override CheckValueCombinedIdWithPropertyName ImplicitCast((TestGuidId TestGuid, string StringId, Guid GuidId) value)
        {
            // Combined IDs don't have implicit cast operators
            return null!;
        }

        protected override string ConvertToString((TestGuidId TestGuid, string StringId, Guid GuidId) value)
        {
            // Combined IDs don't have direct TypeConverter conversion from string
            return null!;
        }

        [Test]
        public void InterfaceValue_AccessibleAndMatchesCustomProperty()
        {
            // Arrange
            var tupleValue = (
                new TestGuidId(Guid.NewGuid()), 
                "TestString", 
                Guid.NewGuid()
            );
            
            var id = new CheckValueCombinedIdWithPropertyName(tupleValue);
            var tupleType = typeof(CheckValueCombinedIdWithPropertyName).GetProperty("Data")!.PropertyType;
            var interfaceType = typeof(ITypedIdentifier<>).MakeGenericType(tupleType);

            // Act
            var customPropertyValue = id.Data;
            var interfaceValue = interfaceType.GetProperty("Value")!.GetValue(id);

            // Assert
            Assert.That(interfaceValue, Is.EqualTo(customPropertyValue));
        }
    }
}