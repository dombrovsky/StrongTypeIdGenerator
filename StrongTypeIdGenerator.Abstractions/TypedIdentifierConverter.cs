namespace StrongTypeIdGenerator
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Generic type converter for strongly-typed identifiers that converts between the identifier and its underlying value type.
    /// </summary>
    /// <typeparam name="TIdentifier">The type of the underlying identifier value.</typeparam>
    /// <remarks>
    /// This internal class provides type conversion capabilities for strongly-typed identifiers,
    /// allowing conversion between the identifier type and its underlying value type.
    /// It uses reflection and factory methods to create instances of the strongly-typed identifier.
    /// </remarks>
    internal sealed class TypedIdentifierConverter<TIdentifier> : TypeConverter
        where TIdentifier : IEquatable<TIdentifier>
    {
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedIdentifierConverter{TIdentifier}"/> class.
        /// </summary>
        /// <param name="type">The strongly-typed identifier type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is <c>null</c>.</exception>
        public TypedIdentifierConverter(Type type)
        {
            Argument.NotNull(type);

            _type = type;
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="Type"/> that represents the type you want to convert from.</param>
        /// <returns>
        /// <c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.
        /// This implementation returns <c>true</c> for the underlying identifier type <typeparamref name="TIdentifier"/>.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(TIdentifier) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="Type"/> that represents the type you want to convert to.</param>
        /// <returns>
        /// <c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.
        /// This implementation returns <c>true</c> for the underlying identifier type <typeparamref name="TIdentifier"/>.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(TIdentifier) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        /// <remarks>
        /// This method converts from the underlying identifier type to the strongly-typed identifier
        /// using a factory method.
        /// </remarks>
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is TIdentifier idValue)
            {
                var factory = TypedIdentifierConverterHelper.GetFactory<TIdentifier>(_type);
                return factory(idValue);
            }

            return base.ConvertFrom(context, culture, value);
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
        /// This method converts from the strongly-typed identifier to its underlying value type or string representation.
        /// </remarks>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value == null)
            {
                return null;
            }

            var typedIdentifier = (ITypedIdentifier<TIdentifier>)value;
            var idValue = typedIdentifier.Value;
            if (destinationType == typeof(string))
            {
                return idValue.ToString();
            }

            if (destinationType == typeof(TIdentifier))
            {
                return idValue;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// A general-purpose type converter for strongly-typed identifiers that automatically determines the appropriate
    /// converter based on the identifier type.
    /// </summary>
    /// <remarks>
    /// This class serves as a factory for creating specific <see cref="TypedIdentifierConverter{TIdentifier}"/>
    /// instances based on the strongly-typed identifier type. It uses caching to improve performance for repeated
    /// conversions of the same type.
    /// 
    /// Based on the implementation from:
    /// <see href="https://thomaslevesque.com/2020/11/23/csharp-9-records-as-strongly-typed-ids-part-2-aspnet-core-route-and-query-parameters/"/>
    /// </remarks>
    public class TypedIdentifierConverter : TypeConverter
    {
        private static readonly ConcurrentDictionary<Type, TypeConverter> ActualConverters = new();

        private readonly TypeConverter _innerConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedIdentifierConverter"/> class.
        /// </summary>
        /// <param name="stronglyTypedIdType">The strongly-typed identifier type.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified type is not a valid strongly-typed identifier.
        /// </exception>
        public TypedIdentifierConverter(Type stronglyTypedIdType)
        {
            _innerConverter = ActualConverters.GetOrAdd(stronglyTypedIdType, CreateActualConverter);
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="Type"/> that represents the type you want to convert from.</param>
        /// <returns><c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return _innerConverter.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="Type"/> that represents the type you want to convert to.</param>
        /// <returns><c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return _innerConverter.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            return _innerConverter.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to the specified type.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">A <see cref="CultureInfo"/>. If <c>null</c> is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <param name="destinationType">The <see cref="Type"/> to convert the <paramref name="value"/> parameter to.</param>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,
            Type destinationType)
        {
            return _innerConverter.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Creates the appropriate <see cref="TypedIdentifierConverter{TIdentifier}"/> for the specified strongly-typed identifier type.
        /// </summary>
        /// <param name="stronglyTypedIdType">The strongly-typed identifier type.</param>
        /// <returns>A <see cref="TypeConverter"/> instance for the specified type.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified type is not a valid strongly-typed identifier.
        /// </exception>
        private static TypeConverter CreateActualConverter(Type stronglyTypedIdType)
        {
            if (!TypedIdentifierConverterHelper.IsTypedIdentifier(stronglyTypedIdType, out var argumentTypes))
            {
                throw new InvalidOperationException($"The type '{stronglyTypedIdType}' is not a strongly typed id");
            }

            var actualConverterType = typeof(TypedIdentifierConverter<>).MakeGenericType(argumentTypes);
            return (TypeConverter)Activator.CreateInstance(actualConverterType, stronglyTypedIdType)!;
        }
    }
}