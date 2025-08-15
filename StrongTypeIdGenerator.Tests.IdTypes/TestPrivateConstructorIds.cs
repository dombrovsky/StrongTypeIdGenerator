using StrongTypeIdGenerator;

namespace StrongTypeIdGenerator.Tests
{
    [StringId(GenerateConstructorPrivate = true)]
    public partial class TestStringIdPrivateConstructor
    {
        public static TestStringIdPrivateConstructor Create(string value)
        {
            return new TestStringIdPrivateConstructor(value);
        }

        public static TestStringIdPrivateConstructor CreateEmpty()
        {
            return new TestStringIdPrivateConstructor(string.Empty);
        }
    }

    [CombinedId(typeof(System.Guid), "TenantId", typeof(string), "UserId", GenerateConstructorPrivate = true)]
    public partial class TestCombinedIdPrivateConstructor
    {
        public static TestCombinedIdPrivateConstructor Create(System.Guid tenantId, string userId)
        {
            return new TestCombinedIdPrivateConstructor(tenantId, userId);
        }

        public static TestCombinedIdPrivateConstructor CreateForTenant(System.Guid tenantId, string userId)
        {
            // Business logic validation could go here
            return new TestCombinedIdPrivateConstructor(tenantId, userId);
        }
    }

    [CombinedId(typeof(TestGuidId), "TestGuid", typeof(string), "StringId", GenerateConstructorPrivate = true)]
    public partial class CheckValueCombinedIdPrivateConstructor
    {
        public static readonly (TestGuidId TestGuid, string StringId) InvalidValue = (new TestGuidId(System.Guid.Parse("30704DE1-8F21-460B-B3F1-9027F4D5F7B6")), "invalid");
        public static readonly (TestGuidId TestGuid, string StringId) ValidValue = (new TestGuidId(System.Guid.Parse("ECA00066-B8BF-4BAF-8370-3240E7F64356")), "valid");

        public static CheckValueCombinedIdPrivateConstructor Create(TestGuidId testGuid, string stringId)
        {
            return new CheckValueCombinedIdPrivateConstructor(testGuid, stringId);
        }

        private static (TestGuidId TestGuid, string StringId) CheckValue(TestGuidId testGuid, string stringId)
        {
            if (testGuid == InvalidValue.TestGuid && stringId == InvalidValue.StringId)
            {
                throw new System.ArgumentException("Invalid value combination", nameof(testGuid));
            }

            return ValidValue;
        }
    }
}