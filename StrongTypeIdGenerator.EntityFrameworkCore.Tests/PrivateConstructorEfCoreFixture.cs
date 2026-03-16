namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class PrivateConstructorEfCoreFixture
    {
        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanRoundTripPrivateConstructorGuidStrongTypeId(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var id = TestGuidIdPrivateConstructor.Create(Guid.Parse("A7AA3D7E-A028-4C18-91C6-EAF8F8867E95"));

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.PrivateGuidIds.Add(new PrivateGuidIdEntity
                {
                    Id = 1,
                    StrongId = id,
                });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var loaded = readContext.PrivateGuidIds.Single(x => x.Id == 1);
                Assert.That(loaded.StrongId, Is.EqualTo(id));
            }
        }
    }
}
