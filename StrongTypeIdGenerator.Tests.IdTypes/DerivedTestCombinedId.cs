namespace StrongTypeIdGenerator.Tests
{
    public sealed partial class DerivedTestCombinedId : TestCombinedId
    {
        public DerivedTestCombinedId((TestGuidId TestGuid, string StringId, System.Guid GuidId, int IntId) value)
            : base(value)
        {
        }
    }
}