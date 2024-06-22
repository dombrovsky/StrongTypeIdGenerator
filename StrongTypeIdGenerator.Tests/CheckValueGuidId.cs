namespace StrongTypeIdGenerator.Tests
{
    [GuidId]
    internal partial class CheckValueGuidId
    {
        public static readonly Guid InvalidValue = Guid.Parse("90BC12B3-F9FA-44FF-AEEF-D30688D9B1FC");

        private static void CheckValue(Guid value)
        {
            if (value == InvalidValue)
            {
                throw new ArgumentException("Invalid value", nameof(value));
            }
        }
    }
}