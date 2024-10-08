namespace StrongTypeIdGenerator.Json.Tests
{
    using StrongTypeIdGenerator.Tests;

    internal sealed class StringIdSystemTextJsonConversionFixture : SystemTextJsonConversionFixture<TestStringId, string>
    {
        protected override string GetValue()
        {
            return Guid.NewGuid().ToString();
        }

        protected override TestStringId CreateId(string value)
        {
            return new TestStringId(value);
        }
    }
}