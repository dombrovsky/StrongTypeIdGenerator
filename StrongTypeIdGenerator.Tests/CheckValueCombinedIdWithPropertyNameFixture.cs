namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    public class CheckValueCombinedIdWithPropertyNameFixture : CustomCheckValueFixtureBase<CheckValueCombinedIdWithPropertyName, (TestGuidId, string, Guid)>
    {
        [Ignore("Not applicable for combined id")]
        public override void ImplicitCast_CheckValueIsCalled() { }
        [Ignore("Not applicable for combined id")]
        public override void TypeConverter_CheckValueIsCalled() { }

        protected override CheckValueCombinedIdWithPropertyName CreateId((TestGuidId, string, Guid) value) => new CheckValueCombinedIdWithPropertyName((value.Item1, value.Item2, value.Item3));
        protected override (TestGuidId, string, Guid) GetValidValue() => CheckValueCombinedIdWithPropertyName.ValidValue;
        protected override (TestGuidId, string, Guid) GetInvalidValue() => CheckValueCombinedIdWithPropertyName.InvalidValue;
        protected override (TestGuidId, string, Guid) GetExpectedValidValue() => GetValidValue();
        protected override (TestGuidId, string, Guid) GetIdValue(CheckValueCombinedIdWithPropertyName id) => (id.Data.Item1, id.Data.Item2, id.Data.Item3);
        protected override CheckValueCombinedIdWithPropertyName ImplicitCast((TestGuidId, string, Guid) value) => new CheckValueCombinedIdWithPropertyName((value.Item1, value.Item2, value.Item3));
        protected override string ConvertToString((TestGuidId, string, Guid) value) => $"{value.Item1},{value.Item2},{value.Item3}";
    }
}