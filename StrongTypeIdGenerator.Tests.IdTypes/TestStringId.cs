namespace StrongTypeIdGenerator.Tests
{
    [StringId]
    public partial class TestStringId
    {
    }

    public sealed partial class DerivedTestStringId : TestStringId
    {
        public DerivedTestStringId(string value)
            : base(value)
        {
        }
    }

    [StringId]
    public partial class CheckValueStringId
    {
        private static string CheckValue(string value)
        {
            if (value.Length > 10)
            {
                throw new ArgumentException("Value is too long", nameof(value));
            }

            return value.ToUpperInvariant();
        }
    }
}