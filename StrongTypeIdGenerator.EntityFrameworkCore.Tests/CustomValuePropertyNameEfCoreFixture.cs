namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class CustomValuePropertyNameEfCoreFixture
    {
        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanRoundTripGuidIdWithCustomValuePropertyName(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var id = new TestGuidIdWithPropertyName(Guid.Parse("008AFC46-C30B-4904-B570-616A6C08EE49"));

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.GuidIdsWithPropertyName.Add(new GuidIdWithPropertyNameEntity
                {
                    Id = id,
                    Name = "guid-custom-property"
                });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var loaded = readContext.GuidIdsWithPropertyName.Single(x => x.Id == id);
                Assert.That(loaded.Id, Is.EqualTo(id));
                Assert.That(loaded.Name, Is.EqualTo("guid-custom-property"));
            }
        }

        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanRoundTripStringIdWithCustomValuePropertyName(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var id = new TestStringIdWithPropertyName("customer-xyz");

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.StringIdsWithPropertyName.Add(new StringIdWithPropertyNameEntity
                {
                    Id = id,
                    Name = "string-custom-property"
                });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var loaded = readContext.StringIdsWithPropertyName.Single(x => x.Id == id);
                Assert.That(loaded.Id, Is.EqualTo(id));
                Assert.That(loaded.Name, Is.EqualTo("string-custom-property"));
            }
        }
    }
}
