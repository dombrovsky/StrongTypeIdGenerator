namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class CombinedIdModelMetadataFixture
    {
        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CombinedIdPropertyIsConfiguredAsComplexTypeWithConverterOnStrongIdComponent(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            using var context = DbTestHelper.CreateContext(options, mode);

            var entityType = context.Model.FindEntityType(typeof(CombinedIdEntity));
            Assert.That(entityType, Is.Not.Null);

            var complexProperty = entityType!.FindComplexProperty(nameof(CombinedIdEntity.CompositeId));
            Assert.That(complexProperty, Is.Not.Null);
            Assert.That(complexProperty!.ComplexType.ClrType, Is.EqualTo(typeof(TestCombinedId)));

            var strongIdComponent = complexProperty.ComplexType.FindProperty(nameof(TestCombinedId.TestGuid));
            Assert.That(strongIdComponent, Is.Not.Null);
            Assert.That(strongIdComponent!.ClrType, Is.EqualTo(typeof(TestGuidId)));

            var converter = strongIdComponent.GetValueConverter();
            Assert.That(converter, Is.Not.Null);
            Assert.That(converter!.ModelClrType, Is.EqualTo(typeof(TestGuidId)));
            Assert.That(converter.ProviderClrType, Is.EqualTo(typeof(Guid)));
        }
    }
}