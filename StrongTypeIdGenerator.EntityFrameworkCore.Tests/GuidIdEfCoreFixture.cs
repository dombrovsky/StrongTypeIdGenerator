namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class GuidIdEfCoreFixture
    {
        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanRoundTripGuidStrongTypeIdAsPrimaryKey(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var id = new TestGuidId(Guid.Parse("D5A5C4D3-4B7E-4B6A-90A5-5A5A08E43A11"));

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.GuidIds.Add(new GuidIdEntity
                {
                    Id = id,
                    Name = "guid-row"
                });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var loaded = readContext.GuidIds.Single(x => x.Id == id);
                Assert.That(loaded.Id, Is.EqualTo(id));
                Assert.That(loaded.Name, Is.EqualTo("guid-row"));
            }
        }
    }
}
