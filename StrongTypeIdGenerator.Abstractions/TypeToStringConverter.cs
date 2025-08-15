namespace StrongTypeIdGenerator
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Abstract base class for type converters that convert between strongly-typed identifiers and string representations.
    /// </summary>
    /// <typeparam name="T">The strongly-typed identifier type.</typeparam>
    /// <remarks>
    /// This abstract class provides a foundation for implementing type converters that can convert strongly-typed
    /// identifiers to and from their string representations. It handles the basic conversion operations and
    /// delegates the actual conversion logic to derived classes through abstract methods.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class CustomerIdConverter : TypeToStringConverter&lt;CustomerId&gt;
    /// {
    ///     protected override string InternalConvertToString(CustomerId value)
    ///     {
    ///         return value.Value.ToString();
    ///     }
    ///     
    ///     protected override CustomerId InternalConvertFromString(string value)
    ///     {
    ///         return new CustomerId(value);
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class TypeToStringConverter<T> : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="Type"/> that represents the type you want to convert from.</param>
        /// <returns>
        /// <c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.
        /// This implementation returns <c>true</c> for <see cref="string"/> types.
        /// </returns>
        public sealed override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="Type"/> that represents the type you want to convert to.</param>
        /// <returns>
        /// <c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.
        /// This implementation returns <c>true</c> for <see cref="string"/> types.
        /// </returns>
        public sealed override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        /// <remarks>
        /// This method calls <see cref="InternalConvertFromString"/> when the input value is a string,
        /// otherwise delegates to the base implementation.
        /// </remarks>
        public sealed override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            return value is string stringValue ? InternalConvertFromString(stringValue) : base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to the specified type.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">A <see cref="CultureInfo"/>. If <c>null</c> is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <param name="destinationType">The <see cref="Type"/> to convert the <paramref name="value"/> parameter to.</param>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        /// <remarks>
        /// This method calls <see cref="InternalConvertToString"/> when the destination type is string and the value is of type <typeparamref name="T"/>,
        /// otherwise delegates to the base implementation.
        /// </remarks>
        public sealed override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            return value is T typedValue ? InternalConvertToString(typedValue) : base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// When overridden in a derived class, converts the strongly-typed identifier to its string representation.
        /// </summary>
        /// <param name="value">The strongly-typed identifier value to convert.</param>
        /// <returns>A string representation of the strongly-typed identifier, or <c>null</c> if the value cannot be converted.</returns>
        protected abstract string? InternalConvertToString(T value);

        /// <summary>
        /// When overridden in a derived class, converts a string representation to the strongly-typed identifier.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <returns>A strongly-typed identifier instance, or <c>null</c> if the string cannot be converted.</returns>
        protected abstract T? InternalConvertFromString(string value);
    }
}