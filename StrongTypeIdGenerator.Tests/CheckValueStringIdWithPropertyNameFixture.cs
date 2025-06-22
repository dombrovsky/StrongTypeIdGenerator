namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    public class CheckValueStringIdWithPropertyNameFixture : CustomCheckValueFixtureBase<CheckValueStringIdWithPropertyName, string>
    {
        protected override CheckValueStringIdWithPropertyName CreateId(string value) => new CheckValueStringIdWithPropertyName(value);
        protected override string GetValidValue() => "123456";
        protected override string GetInvalidValue() => "StringLengthMoreThan10";
        protected override string GetExpectedValidValue() => GetValidValue();
        protected override string GetIdValue(CheckValueStringIdWithPropertyName id) => id.Text;
        protected override CheckValueStringIdWithPropertyName ImplicitCast(string value) => value;
        protected override string ConvertToString(string value) => value;
    }
}