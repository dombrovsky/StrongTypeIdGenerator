namespace StrongTypeIdGenerator.Tests
{
    [CombinedId(typeof(TestGuidId), "TestGuid", typeof(string), "StringId", typeof(Guid), "GuidId", ValuePropertyName = "Data")]
    public partial class CheckValueCombinedIdWithPropertyName
    {
        public static readonly (TestGuidId TestGuid, string StringId, Guid GuidId) InvalidValue = 
            (new TestGuidId(Guid.Parse("30704DE1-8F21-460B-B3F1-9027F4D5F7B6")), "1", Guid.Parse("30704DE1-8F21-460B-B3F1-9027F4D5F7B6"));

        public static readonly (TestGuidId TestGuid, string StringId, Guid GuidId) ValidValue = 
            (new TestGuidId(Guid.Parse("ECA00066-B8BF-4BAF-8370-3240E7F64356")), "2", Guid.Parse("ECA00066-B8BF-4BAF-8370-3240E7F64356"));

        private static (TestGuidId TestGuid, string StringId, Guid GuidId) CheckValue((TestGuidId TestGuid, string StringId, Guid GuidId) value)
        {
            if (value == InvalidValue)
            {
                throw new ArgumentException("Invalid value", nameof(value));
            }

            return ValidValue;
        }
    }
}