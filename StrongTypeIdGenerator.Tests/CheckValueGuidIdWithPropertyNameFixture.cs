namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    public class CheckValueGuidIdWithPropertyNameFixture : CustomCheckValueFixtureBase<CheckValueGuidIdWithPropertyName, Guid>
    {
        protected override CheckValueGuidIdWithPropertyName CreateId(Guid value) => new CheckValueGuidIdWithPropertyName(value);
        protected override Guid GetValidValue() => CheckValueGuidIdWithPropertyName.ValidValue;
        protected override Guid GetInvalidValue() => CheckValueGuidIdWithPropertyName.InvalidValue;
        protected override Guid GetExpectedValidValue() => GetValidValue();
        protected override Guid GetIdValue(CheckValueGuidIdWithPropertyName id) => id.Guid;
        protected override CheckValueGuidIdWithPropertyName ImplicitCast(Guid value) => value;
        protected override string ConvertToString(Guid value) => value.ToString();
    }
}