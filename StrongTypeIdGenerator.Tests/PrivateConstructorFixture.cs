namespace StrongTypeIdGenerator.Tests
{
    using NUnit.Framework;
    using System;
    using System.Reflection;

    [TestFixture]
    internal sealed class StringIdPrivateConstructorFixture
    {
        [Test]
        public void Constructor_IsPrivate()
        {
            // Arrange
            var constructors = typeof(TestStringIdPrivateConstructor).GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.That(constructors, Is.Empty, "Public constructors should not exist");
        }

        [Test]
        public void PrivateConstructor_Exists()
        {
            // Arrange
            var privateConstructors = typeof(TestStringIdPrivateConstructor).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.That(privateConstructors, Has.Length.EqualTo(1), "Should have exactly one private constructor");
            
            var constructor = privateConstructors[0];
            Assert.That(constructor.IsPrivate, Is.True, "Constructor should be private");
            
            var parameters = constructor.GetParameters();
            Assert.That(parameters, Has.Length.EqualTo(1), "Constructor should have one parameter");
            Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(string)), "Parameter should be of type string");
        }

        [Test]
        public void Create_WithValidString_ReturnsInstance()
        {
            // Arrange
            var value = "test-value";

            // Act
            var result = TestStringIdPrivateConstructor.Create(value);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(value));
        }

        [Test]
        public void CreateEmpty_ReturnsInstanceWithEmptyString()
        {
            // Act
            var result = TestStringIdPrivateConstructor.CreateEmpty();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Unspecified_IsAccessible()
        {
            // Act
            var unspecified = TestStringIdPrivateConstructor.Unspecified;

            // Assert
            Assert.That(unspecified, Is.Not.Null);
            Assert.That(unspecified.Value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ImplicitConversion_StillWorks()
        {
            // Arrange
            var value = "test-value";

            // Act
            TestStringIdPrivateConstructor implicitlyConverted = value;

            // Assert
            Assert.That(implicitlyConverted, Is.Not.Null);
            Assert.That(implicitlyConverted.Value, Is.EqualTo(value));
        }

        [Test]
        public void TypeConverter_StillWorks()
        {
            // Arrange
            var value = "test-value";
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(TestStringIdPrivateConstructor));

            // Act
            var result = converter.ConvertFromString(value);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<TestStringIdPrivateConstructor>());
            Assert.That(((TestStringIdPrivateConstructor)result!).Value, Is.EqualTo(value));
        }
    }

    [TestFixture]
    internal sealed class CombinedIdPrivateConstructorFixture
    {
        [Test]
        public void Constructor_IsPrivate()
        {
            // Arrange
            var constructors = typeof(TestCombinedIdPrivateConstructor).GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.That(constructors, Is.Empty, "Public constructors should not exist");
        }

        [Test]
        public void PrivateConstructor_Exists()
        {
            // Arrange
            var privateConstructors = typeof(TestCombinedIdPrivateConstructor).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.That(privateConstructors, Has.Length.EqualTo(1), "Should have exactly one private constructor");
            
            var constructor = privateConstructors[0];
            Assert.That(constructor.IsPrivate, Is.True, "Constructor should be private");
            
            var parameters = constructor.GetParameters();
            Assert.That(parameters, Has.Length.EqualTo(2), "Constructor should have two parameters");
            Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(Guid)), "First parameter should be of type Guid");
            Assert.That(parameters[1].ParameterType, Is.EqualTo(typeof(string)), "Second parameter should be of type string");
        }

        [Test]
        public void Create_WithValidValues_ReturnsInstance()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var userId = "user123";

            // Act
            var result = TestCombinedIdPrivateConstructor.Create(tenantId, userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TenantId, Is.EqualTo(tenantId));
            Assert.That(result.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void CreateForTenant_WithValidValues_ReturnsInstance()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var userId = "user123";

            // Act
            var result = TestCombinedIdPrivateConstructor.CreateForTenant(tenantId, userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TenantId, Is.EqualTo(tenantId));
            Assert.That(result.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void Unspecified_IsAccessible()
        {
            // Act
            var unspecified = TestCombinedIdPrivateConstructor.Unspecified;

            // Assert
            Assert.That(unspecified, Is.Not.Null);
            Assert.That(unspecified.TenantId, Is.EqualTo(Guid.Empty));
            Assert.That(unspecified.UserId, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Deconstruct_Works()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var userId = "user123";
            var result = TestCombinedIdPrivateConstructor.Create(tenantId, userId);

            // Act
            var (actualTenantId, actualUserId) = result;

            // Assert
            Assert.That(actualTenantId, Is.EqualTo(tenantId));
            Assert.That(actualUserId, Is.EqualTo(userId));
        }
    }

    [TestFixture]
    internal sealed class CheckValueCombinedIdPrivateConstructorFixture
    {
        [Test]
        public void Constructor_IsPrivate()
        {
            // Arrange
            var constructors = typeof(CheckValueCombinedIdPrivateConstructor).GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.That(constructors, Is.Empty, "Public constructors should not exist");
        }

        [Test]
        public void Create_WithValidValue_AppliesCheckValue()
        {
            // Arrange
            var testGuid = new TestGuidId(Guid.NewGuid());
            var stringId = "any-value";

            // Act
            var result = CheckValueCombinedIdPrivateConstructor.Create(testGuid, stringId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TestGuid, Is.EqualTo(CheckValueCombinedIdPrivateConstructor.ValidValue.TestGuid), "CheckValue should transform TestGuid");
            Assert.That(result.StringId, Is.EqualTo(CheckValueCombinedIdPrivateConstructor.ValidValue.StringId), "CheckValue should transform StringId");
        }

        [Test]
        public void Create_WithInvalidValue_ThrowsException()
        {
            // Arrange
            var invalidValue = CheckValueCombinedIdPrivateConstructor.InvalidValue;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                CheckValueCombinedIdPrivateConstructor.Create(invalidValue.TestGuid, invalidValue.StringId));
        }

        [Test]
        public void ExplicitInterfaceImplementation_Works()
        {
            // Arrange
            var testGuid = new TestGuidId(Guid.NewGuid());
            var stringId = "any-value";
            var result = CheckValueCombinedIdPrivateConstructor.Create(testGuid, stringId);

            // Act
            var interfaceValue = ((ITypedIdentifier<(TestGuidId TestGuid, string StringId)>)result).Value;

            // Assert
            Assert.That(interfaceValue.TestGuid, Is.EqualTo(CheckValueCombinedIdPrivateConstructor.ValidValue.TestGuid));
            Assert.That(interfaceValue.StringId, Is.EqualTo(CheckValueCombinedIdPrivateConstructor.ValidValue.StringId));
        }
    }
}