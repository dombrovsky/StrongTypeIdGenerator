namespace StrongTypeIdGenerator.Json
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Adapter between <see cref="System.ComponentModel.TypeConverter"/> 
    /// and <see cref="JsonConverter"/>
    /// </summary>
    public class TypeConverterJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            var hasConverter = typeToConvert.GetCustomAttributes<TypeConverterAttribute>().Any();
            return hasConverter;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converter = TypeDescriptor.GetConverter(typeToConvert);
            return (JsonConverter?)Activator.CreateInstance(
                typeof(TypeConverterJsonAdapter<>).MakeGenericType(typeToConvert),
                new object?[] { converter });
        }

        private sealed class TypeConverterJsonAdapter<T> : JsonConverter<T>
        {
            private readonly TypeConverter _typeConverter;

            public TypeConverterJsonAdapter(TypeConverter typeConverter)
            {
                _typeConverter = typeConverter;
            }

            public override T Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                var text = reader.GetString();
                return (T)_typeConverter.ConvertFromString(text!)!;
            }

            public override void Write(
                Utf8JsonWriter writer,
                T objectToWrite,
                JsonSerializerOptions options)
            {

                if (objectToWrite == null)
                {
                    writer.WriteNullValue();
                    return;
                }

                var text = _typeConverter.ConvertToString(objectToWrite);
                writer.WriteStringValue(text);
            }

            public override T ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var text = reader.GetString();
                return (T)_typeConverter.ConvertFromString(text!)!;
            }

            public override void WriteAsPropertyName(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                var text = _typeConverter.ConvertToString(value);
                writer.WritePropertyName(text!);
            }
        }
    }
}