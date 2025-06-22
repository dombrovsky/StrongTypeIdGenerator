namespace StrongTypeIdGenerator.Tests
{
    [GuidId(ValuePropertyName = "Uuid")]
    public partial class TestGuidIdWithPropertyName
    {
    }

    public partial class DerivedTestGuidIdWithPropertyName : TestGuidIdWithPropertyName
    {
        public DerivedTestGuidIdWithPropertyName(Guid id) : base(id)
        {
        }
    }
}