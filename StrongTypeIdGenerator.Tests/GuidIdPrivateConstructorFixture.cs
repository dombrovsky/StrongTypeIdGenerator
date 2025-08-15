namespace StrongTypeIdGenerator.Tests
{
    using NUnit.Framework;
    using System;
    using System.Reflection;

    [TestFixture]
    internal sealed class GuidIdPrivateConstructorFixture
    {
        [Test]
        public void Constructor_IsPrivate()
        {
            // Arrange
            var constructors = typeof(TestGuidIdPrivateConstructor).GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.That(constructors, Is.Empty, "Public constructors should not exist");
        }

        [Test]
        public void PrivateConstructor_Exists()
        {
            // Arrange
            var privateConstructors = typeof(TestGuidIdPrivateConstructor).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.That(privateConstructors, Has.Length.EqualTo(1), "Should have exactly one private constructor");
            
            var constructor = privateConstructors[0];
            Assert.That(constructor.IsPrivate, Is.True, "Constructor should be private");
            
            var parameters = constructor.GetParameters();
            Assert.That(parameters, Has.Length.EqualTo(1), "Constructor should have one parameter");
            Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(Guid)), "Parameter should be of type Guid");
        }

        [Test]
        public void Create_WithValidGuid_ReturnsInstance()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var result = TestGuidIdPrivateConstructor.Create(guid);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(guid));
        }

        [Test]
        public void CreateRandom_ReturnsInstanceWithNewGuid()
        {
            // Act
            var result1 = TestGuidIdPrivateConstructor.CreateRandom();
            var result2 = TestGuidIdPrivateConstructor.CreateRandom();

            // Assert
            Assert.That(result1, Is.Not.Null);
            Assert.That(result2, Is.Not.Null);
            Assert.That(result1.Value, Is.Not.EqualTo(result2.Value), "Should generate different GUIDs");
            Assert.That(result1.Value, Is.Not.EqualTo(Guid.Empty), "Should not be empty GUID");
            Assert.That(result2.Value, Is.Not.EqualTo(Guid.Empty), "Should not be empty GUID");
        }

        [Test]
        public void Unspecified_IsAccessible()
        {
            // Act
            var unspecified = TestGuidIdPrivateConstructor.Unspecified;

            // Assert
            Assert.That(unspecified, Is.Not.Null);
            Assert.That(unspecified.Value, Is.EqualTo(Guid.Empty));
        }

        [Test]
        public void ImplicitConversion_StillWorks()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            TestGuidIdPrivateConstructor implicitlyConverted = guid;

            // Assert
            Assert.That(implicitlyConverted, Is.Not.Null);
            Assert.That(implicitlyConverted.Value, Is.EqualTo(guid));
        }

        [Test]
        public void TypeConverter_StillWorks()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(TestGuidIdPrivateConstructor));

            // Act
            var result = converter.ConvertFromString(guid.ToString());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<TestGuidIdPrivateConstructor>());
            Assert.That(((TestGuidIdPrivateConstructor)result!).Value, Is.EqualTo(guid));
        }
    }

    [TestFixture]
    internal sealed class GuidIdPrivateConstructorWithPropertyNameFixture
    {
        [Test]
        public void Constructor_IsPrivate()
        {
            // Arrange
            var constructors = typeof(TestGuidIdPrivateConstructorWithPropertyName).GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.That(constructors, Is.Empty, "Public constructors should not exist");
        }

        [Test]
        public void Create_WithValidGuid_ReturnsInstanceWithCustomPropertyName()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var result = TestGuidIdPrivateConstructorWithPropertyName.Create(guid);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Uuid, Is.EqualTo(guid), "Should use custom property name 'Uuid'");
        }

        [Test]
        public void ExplicitInterfaceImplementation_Works()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var result = TestGuidIdPrivateConstructorWithPropertyName.Create(guid);

            // Act
            var interfaceValue = ((ITypedIdentifier<Guid>)result).Value;

            // Assert
            Assert.That(interfaceValue, Is.EqualTo(guid));
        }
    }

    [TestFixture]
    internal sealed class CheckValueGuidIdPrivateConstructorFixture
    {
        [Test]
        public void Constructor_IsPrivate()
        {
            // Arrange
            var constructors = typeof(CheckValueGuidIdPrivateConstructor).GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.That(constructors, Is.Empty, "Public constructors should not exist");
        }

        [Test]
        public void Create_WithValidValue_AppliesCheckValue()
        {
            // Arrange
            var inputValue = Guid.NewGuid();

            // Act
            var result = CheckValueGuidIdPrivateConstructor.Create(inputValue);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(CheckValueGuidIdPrivateConstructor.ValidValue), "CheckValue should transform the input");
        }

        [Test]
        public void Create_WithInvalidValue_ThrowsException()
        {
            // Arrange
            var invalidValue = CheckValueGuidIdPrivateConstructor.InvalidValue;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => CheckValueGuidIdPrivateConstructor.Create(invalidValue));
        }

        [Test]
        public void ImplicitConversion_WithInvalidValue_ThrowsException()
        {
            // Arrange
            var invalidValue = CheckValueGuidIdPrivateConstructor.InvalidValue;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                CheckValueGuidIdPrivateConstructor converted = invalidValue;
            });
        }

        [Test]
        public void TypeConverter_WithInvalidValue_ThrowsException()
        {
            // Arrange
            var invalidValue = CheckValueGuidIdPrivateConstructor.InvalidValue;
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(CheckValueGuidIdPrivateConstructor));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => converter.ConvertFromString(invalidValue.ToString()));
        }
    }
}