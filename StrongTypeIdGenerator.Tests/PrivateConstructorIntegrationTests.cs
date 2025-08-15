namespace StrongTypeIdGenerator.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    internal sealed class PrivateConstructorIntegrationTests
    {
        [Test]
        public void GuidId_PrivateConstructor_Works()
        {
            // Arrange & Act
            var guid = Guid.NewGuid();
            var result = TestGuidIdPrivateConstructor.Create(guid);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(guid));
        }

        [Test]
        public void StringId_PrivateConstructor_Works()
        {
            // Arrange & Act
            var value = "test-value";
            var result = TestStringIdPrivateConstructor.Create(value);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(value));
        }

        [Test]
        public void CombinedId_PrivateConstructor_Works()
        {
            // Arrange & Act
            var tenantId = Guid.NewGuid();
            var userId = "user123";
            var result = TestCombinedIdPrivateConstructor.Create(tenantId, userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TenantId, Is.EqualTo(tenantId));
            Assert.That(result.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void GuidId_WithCheckValue_PrivateConstructor_Works()
        {
            // Arrange & Act
            var inputValue = Guid.NewGuid();
            var result = CheckValueGuidIdPrivateConstructor.Create(inputValue);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(CheckValueGuidIdPrivateConstructor.ValidValue));
        }

        [Test]
        public void CombinedId_WithCheckValue_PrivateConstructor_Works()
        {
            // Arrange & Act
            var testGuid = new TestGuidId(Guid.NewGuid());
            var stringId = "any-value";
            var result = CheckValueCombinedIdPrivateConstructor.Create(testGuid, stringId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TestGuid, Is.EqualTo(CheckValueCombinedIdPrivateConstructor.ValidValue.TestGuid));
            Assert.That(result.StringId, Is.EqualTo(CheckValueCombinedIdPrivateConstructor.ValidValue.StringId));
        }

        [Test]
        public void GuidId_WithPropertyName_PrivateConstructor_Works()
        {
            // Arrange & Act
            var guid = Guid.NewGuid();
            var result = TestGuidIdPrivateConstructorWithPropertyName.Create(guid);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Uuid, Is.EqualTo(guid), "Should use custom property name 'Uuid'");
        }
    }
}