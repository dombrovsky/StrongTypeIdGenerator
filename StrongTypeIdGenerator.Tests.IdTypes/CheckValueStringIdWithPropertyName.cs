namespace StrongTypeIdGenerator.Tests
{
    [StringId(ValuePropertyName = "Text")]
    public partial class CheckValueStringIdWithPropertyName
    {
        private static string CheckValue(string value)
        {
            if (value.Length > 10)
            {
                throw new ArgumentException("Value is too long", nameof(value));
            }

            return value.ToUpperInvariant();
        }
    }
}