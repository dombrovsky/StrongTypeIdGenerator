namespace StrongTypeIdGenerator.Tests
{
    [GuidId(ValuePropertyName = "Guid")]
    public partial class CheckValueGuidIdWithPropertyName
    {
        public static readonly Guid InvalidValue = Guid.Parse("90BC12B3-F9FA-44FF-AEEF-D30688D9B1FC");

        public static readonly Guid ValidValue = Guid.Parse("2348B258-DB8F-4F88-B5CA-20AF04E34729");

        private static Guid CheckValue(Guid value)
        {
            if (value == InvalidValue)
            {
                throw new ArgumentException("Invalid value", nameof(value));
            }

            return ValidValue;
        }
    }
}