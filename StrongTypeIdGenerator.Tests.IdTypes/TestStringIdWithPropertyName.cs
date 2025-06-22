namespace StrongTypeIdGenerator.Tests
{
    [StringId(ValuePropertyName = "Text")]
    public partial class TestStringIdWithPropertyName
    {
    }

    public partial class DerivedTestStringIdWithPropertyName : TestStringIdWithPropertyName
    {
        public DerivedTestStringIdWithPropertyName(string id) : base(id)
        {
        }
    }
}