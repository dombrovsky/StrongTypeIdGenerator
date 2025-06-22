namespace StrongTypeIdGenerator.SourceGenerator
{
    using System.Globalization;

    internal static class StringExtensions
    {
        public static string Decapitalize(this string input, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return char.ToLower(input[0], cultureInfo).ToString() + input.Substring(1);
        }
    }
}
