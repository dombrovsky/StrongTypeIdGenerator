namespace StrongTypeIdGenerator
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public abstract class TypeToStringConverter<T> : TypeConverter
    {
        public sealed override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public sealed override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public sealed override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            return value is string stringValue ? InternalConvertFromString(stringValue) : base.ConvertFrom(context, culture, value);
        }

        public sealed override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            return value is T typedValue ? InternalConvertToString(typedValue) : base.ConvertTo(context, culture, value, destinationType);
        }

        protected abstract string? InternalConvertToString(T value);

        protected abstract T? InternalConvertFromString(string value);
    }
}