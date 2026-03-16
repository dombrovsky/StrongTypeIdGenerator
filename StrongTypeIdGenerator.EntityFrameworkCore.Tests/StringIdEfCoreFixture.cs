namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class StringIdEfCoreFixture
    {
        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanRoundTripStringStrongTypeIdAsPrimaryKey(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var id = new TestStringId("order-123");

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.StringIds.Add(new StringIdEntity
                {
                    Id = id,
                    Value = "value-row"
                });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var loaded = readContext.StringIds.Single(x => x.Id == id);
                Assert.That(loaded.Id, Is.EqualTo(id));
                Assert.That(loaded.Value, Is.EqualTo("value-row"));
            }
        }
    }
}
