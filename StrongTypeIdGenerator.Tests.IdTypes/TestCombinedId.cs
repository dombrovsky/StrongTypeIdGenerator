namespace StrongTypeIdGenerator.Tests
{
    [CombinedId(typeof(TestGuidId), "TestGuid", typeof(string), "StringId", typeof(Guid), "GuidId", typeof(int), "IntId")]
    public partial class TestCombinedId
    {
    }
}