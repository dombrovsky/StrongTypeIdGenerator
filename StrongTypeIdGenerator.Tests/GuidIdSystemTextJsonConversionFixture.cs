namespace StrongTypeIdGenerator.Tests
{
    internal sealed class GuidIdSystemTextJsonConversionFixture : SystemTextJsonConversionFixture<TestGuidId, Guid>
    {
        protected override Guid GetValue()
        {
            return Guid.NewGuid();
        }

        protected override TestGuidId CreateId(Guid value)
        {
            return new TestGuidId(value);
        }
    }
}