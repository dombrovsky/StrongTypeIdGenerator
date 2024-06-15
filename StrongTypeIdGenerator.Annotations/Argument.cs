namespace StrongTypeIdGenerator
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    internal static class Argument
    {
        public static void NotNull<T>([NotNull]T? argument, [CallerArgumentExpression("argument")] string? parameterName = null)
            where T : class
        {
#if NET6_0
            ArgumentNullException.ThrowIfNull(argument, parameterName);
#else
            if (argument is null)
            {
                throw new ArgumentNullException(parameterName);
            }
#endif
        }

        public static void Assert<T>(in T argument, Func<T, bool> predicate, string message, [CallerArgumentExpression("argument")] string? parameterName = null)
        {
            Argument.NotNull(predicate);
            Argument.NotNull(message);

            if (!predicate(argument))
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        public static void NotEmpty(string? argument, [CallerArgumentExpression("argument")] string? parameterName = null)
        {
            if (string.IsNullOrEmpty(argument))
            {
                Argument.NotNull(argument, parameterName);

                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Argument {0} is empty.", parameterName));
            }
        }
    }
}