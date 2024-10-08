namespace StrongTypeIdGenerator.Tests
{
    public sealed partial class DerivedTestStringId : TestStringId
    {
        public DerivedTestStringId(string value)
            : base(value)
        {
        }
    }
}