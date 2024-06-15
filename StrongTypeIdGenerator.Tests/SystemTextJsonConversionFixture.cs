namespace StrongTypeIdGenerator.Tests
{
    using StrongTypeIdGenerator.Json;
    using System.Text.Json;

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

            var serializerOptions = new JsonSerializerOptions { Converters = { new TypeConverterJsonConverterFactory() } };

            var serializedDto = JsonSerializer.Serialize(dto, serializerOptions);

            var deserializedDto = JsonSerializer.Deserialize<DtoNotTyped>(serializedDto, serializerOptions);

            Assert.That(deserializedDto!.Id, Is.EqualTo(dto.Id.Value));
            Assert.That(deserializedDto.Bar[dto.Id.Value], Is.EqualTo(dto.Bar[dto.Id]));
            Assert.That(deserializedDto.Foo, Is.EqualTo(dto.Foo));
        }

        [Test]
        public void CanDeserializeJson()
        {
            var id = GetValue();
            var dto = new DtoNotTyped
            {
                Foo = "foo",
                Bar = new Dictionary<TIdentifier, string>
                {
                    { id, "tostring" + id.ToString() }
                },
                Id = id,
            };

            var serializerOptions = new JsonSerializerOptions { Converters = { new TypeConverterJsonConverterFactory() } };

            var serializedDto = JsonSerializer.Serialize(dto, serializerOptions);

            var deserializedDto = JsonSerializer.Deserialize<Dto>(serializedDto, serializerOptions);

            Assert.That(deserializedDto!.Id.Value, Is.EqualTo(dto.Id));
            Assert.That(deserializedDto.Bar[CreateId(id)], Is.EqualTo(dto.Bar[dto.Id]));
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

        private sealed class DtoNotTyped
        {
            public required string Foo { get; set; }

            public required Dictionary<TIdentifier, string> Bar { get; set; }

            public required TIdentifier Id { get; set; }
        }
    }
}