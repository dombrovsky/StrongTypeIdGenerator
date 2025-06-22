namespace StrongTypeIdGenerator.Tests
{
    using System.ComponentModel;

    [TestFixture]
    internal sealed class CustomCheckValueStringIdFixture : CustomCheckValueFixtureBase<CheckValueStringId, string>
    {
        protected override CheckValueStringId CreateId(string value)
        {
            return new CheckValueStringId(value);
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

        protected override string GetIdValue(CheckValueStringId id)
        {
            return id.Value;
        }

        protected override CheckValueStringId ImplicitCast(string value)
        {
            return value;
        }

        protected override string ConvertToString(string value)
        {
            return value;
        }
    }
}