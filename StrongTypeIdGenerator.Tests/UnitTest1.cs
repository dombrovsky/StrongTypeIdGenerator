namespace StrongTypeIdGenerator.Tests
{
    using System.ComponentModel;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class StrongTypeIdTests
    {
        [SetUp]
        public void Setup()
        {
            var a = TestStringId1.Unspecified;
            var b = new TestStringId1("1");
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }

    public sealed class StringIdSystemTextJsonConversionFixture : SystemTextJsonConversionFixture<TestStringId1, string>
    {
        protected override string GetValue()
        {
            return Guid.NewGuid().ToString();
        }

        protected override TestStringId1 CreateId(string value)
        {
            return new TestStringId1(value);
        }
    }

    public sealed class GuidIdSystemTextJsonConversionFixture : SystemTextJsonConversionFixture<TestGuidId1, Guid>
    {
        protected override Guid GetValue()
        {
            return Guid.NewGuid();
        }

        protected override TestGuidId1 CreateId(Guid value)
        {
            return new TestGuidId1(value);
        }
    }

    public abstract class SystemTextJsonConversionFixture<TId, TIdentifier>
        where TId : ITypedIdentifier<TId, TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        [Test]
        public void CanSerializeJson()
        {
            var id = CreateId(GetValue());
            var dto = new Dto
            {
                Foo = "foo",
                Bar = new Dictionary<TId, string>
                {
                    { id, "tostring" + id.ToString() }
                },
                Id = id,
            };

            var serializedDto = JsonSerializer.Serialize(dto, new JsonSerializerOptions{ Converters = { new TypeConverterJsonConverterFactory() } });

            var deserializedDto = JsonSerializer.Deserialize<Dto>(serializedDto, new JsonSerializerOptions { Converters = { new TypeConverterJsonConverterFactory() } });

            Assert.That(deserializedDto!.Id, Is.EqualTo(dto.Id));
            Assert.That(deserializedDto.Bar[dto.Id], Is.EqualTo(dto.Bar[dto.Id]));
            Assert.That(deserializedDto.Foo, Is.EqualTo(dto.Foo));
        }

        protected abstract TId CreateId(TIdentifier value);

        protected abstract TIdentifier GetValue();

        private sealed class Dto
        {
            public required string Foo { get; set; }

            public required Dictionary<TId, string> Bar { get; set; }

            public required TId Id { get; set; }
        }
    }

    [StringId]
    public sealed partial class TestStringId1
    {
    }

    [GuidId]
    public sealed partial class TestGuidId1
    {
    }

    public sealed class TypeConverterJsonConverterFactory : JsonConverterFactory
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
    }

    /// <summary>
    /// Adapter between <see cref="System.ComponentModel.TypeConverter"/> 
    /// and <see cref="JsonConverter"/>
    /// </summary>
    public class TypeConverterJsonAdapter<T> : JsonConverter<T>
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