namespace StrongTypeIdGenerator
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Globalization;

    internal sealed class TypedIdentifierConverter<TIdentifier> : TypeConverter
        where TIdentifier : IEquatable<TIdentifier>
    {
        private readonly Type _type;

        public TypedIdentifierConverter(Type type)
        {
            Argument.NotNull(type);

            _type = type;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(TIdentifier) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(TIdentifier) || base.CanConvertTo(context, destinationType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is TIdentifier idValue)
            {
                var factory = TypedIdentifierConverterHelper.GetFactory<TIdentifier>(_type);
                return factory(idValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

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
    /// <see href="https://thomaslevesque.com/2020/11/23/csharp-9-records-as-strongly-typed-ids-part-2-aspnet-core-route-and-query-parameters/"/>
    /// </summary>
    public class TypedIdentifierConverter : TypeConverter
    {
        private static readonly ConcurrentDictionary<Type, TypeConverter> ActualConverters = new();

        private readonly TypeConverter _innerConverter;

        public TypedIdentifierConverter(Type stronglyTypedIdType)
        {
            _innerConverter = ActualConverters.GetOrAdd(stronglyTypedIdType, CreateActualConverter);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return _innerConverter.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return _innerConverter.CanConvertTo(context, destinationType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            return _innerConverter.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,
            Type destinationType)
        {
            return _innerConverter.ConvertTo(context, culture, value, destinationType);
        }

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