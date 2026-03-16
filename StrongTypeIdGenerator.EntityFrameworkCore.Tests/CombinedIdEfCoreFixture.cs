namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class CombinedIdEfCoreFixture
    {
        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanRoundTripCombinedIdAsComplexType(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var compositeId = new TestCombinedId(new TestGuidId(Guid.Parse("0D3D8446-9E0D-4E0B-B8E4-4AE07E2A9D4A")), "A1", Guid.Parse("2263F5F1-7D90-48A0-977E-536A96AB4067"), 42);

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.CombinedIds.Add(new CombinedIdEntity
                {
                    Id = 1,
                    CompositeId = compositeId,
                });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var loaded = readContext.CombinedIds.Single(x => x.Id == 1);
                Assert.That(loaded.CompositeId, Is.EqualTo(compositeId));
            }
        }
    }
}
