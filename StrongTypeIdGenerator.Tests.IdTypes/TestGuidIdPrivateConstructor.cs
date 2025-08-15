using StrongTypeIdGenerator;
using System;

namespace StrongTypeIdGenerator.Tests
{
    [GuidId(GenerateConstructorPrivate = true)]
    public partial class TestGuidIdPrivateConstructor
    {
        public static TestGuidIdPrivateConstructor Create(Guid value)
        {
            return new TestGuidIdPrivateConstructor(value);
        }

        public static TestGuidIdPrivateConstructor CreateRandom()
        {
            return new TestGuidIdPrivateConstructor(Guid.NewGuid());
        }
    }

    [GuidId(GenerateConstructorPrivate = true, ValuePropertyName = "Uuid")]
    public partial class TestGuidIdPrivateConstructorWithPropertyName
    {
        public static TestGuidIdPrivateConstructorWithPropertyName Create(Guid value)
        {
            return new TestGuidIdPrivateConstructorWithPropertyName(value);
        }

        public static TestGuidIdPrivateConstructorWithPropertyName CreateRandom()
        {
            return new TestGuidIdPrivateConstructorWithPropertyName(Guid.NewGuid());
        }
    }

    [GuidId(GenerateConstructorPrivate = true)]
    public partial class CheckValueGuidIdPrivateConstructor
    {
        public static readonly Guid InvalidValue = Guid.Parse("90BC12B3-F9FA-44FF-AEEF-D30688D9B1FC");
        public static readonly Guid ValidValue = Guid.Parse("2348B258-DB8F-4F88-B5CA-20AF04E34729");

        public static CheckValueGuidIdPrivateConstructor Create(Guid value)
        {
            return new CheckValueGuidIdPrivateConstructor(value);
        }

        private static Guid CheckValue(Guid value)
        {
            if (value == InvalidValue)
            {
                throw new ArgumentException("Invalid value", nameof(value));
            }

            return ValidValue;
        }
    }
}