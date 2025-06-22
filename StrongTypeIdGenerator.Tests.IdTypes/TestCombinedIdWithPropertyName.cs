namespace StrongTypeIdGenerator.Tests
{
    [CombinedId(typeof(TestGuidIdWithPropertyName), "TestGuid", typeof(string), "StringId", ValuePropertyName = "Data")]
    public partial class TestCombinedIdWithPropertyName
    {
    }

    public partial class DerivedTestCombinedIdWithPropertyName : TestCombinedIdWithPropertyName
    {
        public DerivedTestCombinedIdWithPropertyName((TestGuidIdWithPropertyName TestGuid, string StringId) value)
            : base(value)
        {
        }
    }
}