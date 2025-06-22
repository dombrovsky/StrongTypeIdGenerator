namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class CustomCheckValueCombinedIdFixture : CustomCheckValueFixtureBase<CheckValueCombinedId, (TestGuidId TestGuid, string StringId, Guid GuidId)>
    {
        protected override CheckValueCombinedId CreateId((TestGuidId TestGuid, string StringId, Guid GuidId) value)
        {
            return new CheckValueCombinedId(value);
        }

        protected override (TestGuidId TestGuid, string StringId, Guid GuidId) GetValidValue()
        {
            return (new TestGuidId(Guid.Parse("3A891E7E-506A-4A05-BF81-2F8D6544CBAB")), "3", Guid.Parse("3A891E7E-506A-4A05-BF81-2F8D6544CBAB"));
        }

        protected override (TestGuidId TestGuid, string StringId, Guid GuidId) GetInvalidValue()
        {
            return CheckValueCombinedId.InvalidValue;
        }

        protected override (TestGuidId TestGuid, string StringId, Guid GuidId) GetExpectedValidValue()
        {
            return CheckValueCombinedId.ValidValue;
        }

        protected override (TestGuidId TestGuid, string StringId, Guid GuidId) GetIdValue(CheckValueCombinedId id)
        {
            return id.Value;
        }

        [Ignore("Not applicable for combined id")]
        public override void ImplicitCast_CheckValueIsCalled() { }
        [Ignore("Not applicable for combined id")]
        public override void TypeConverter_CheckValueIsCalled() { }

        protected override CheckValueCombinedId ImplicitCast((TestGuidId TestGuid, string StringId, Guid GuidId) value)
        {
            // Combined IDs don't have implicit cast operators
            return null!;
        }

        protected override string ConvertToString((TestGuidId TestGuid, string StringId, Guid GuidId) value)
        {
            // Combined IDs don't have direct TypeConverter conversion from string
            return null!;
        }
    }
}