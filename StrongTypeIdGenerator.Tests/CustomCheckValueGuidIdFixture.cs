namespace StrongTypeIdGenerator.Tests
{
    [TestFixture]
    internal sealed class CustomCheckValueGuidIdFixture : CustomCheckValueFixtureBase<CheckValueGuidId, Guid>
    {
        protected override CheckValueGuidId CreateId(Guid value)
        {
            return new CheckValueGuidId(value);
        }

        protected override Guid GetValidValue()
        {
            return Guid.Parse("3A891E7E-506A-4A05-BF81-2F8D6544CBAB");
        }

        protected override Guid GetInvalidValue()
        {
            return CheckValueGuidId.InvalidValue;
        }

        protected override Guid GetExpectedValidValue()
        {
            return CheckValueGuidId.ValidValue;
        }

        protected override Guid GetIdValue(CheckValueGuidId id)
        {
            return id.Value;
        }

        protected override CheckValueGuidId ImplicitCast(Guid value)
        {
            return value;
        }

        protected override string ConvertToString(Guid value)
        {
            return value.ToString();
        }
    }
}