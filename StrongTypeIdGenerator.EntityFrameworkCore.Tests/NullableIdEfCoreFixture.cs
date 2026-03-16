namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class NullableIdEfCoreFixture
    {
        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanRoundTripNullableGuidStrongTypeId(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var nonNullId = new TestGuidId(Guid.Parse("A26974E3-69A2-473A-B9D4-7C1D5E70F435"));

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.NullableGuidIds.Add(new NullableGuidIdEntity { Id = 1, OptionalId = null });
                writeContext.NullableGuidIds.Add(new NullableGuidIdEntity { Id = 2, OptionalId = nonNullId });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var nullEntity = readContext.NullableGuidIds.Single(x => x.Id == 1);
                var nonNullEntity = readContext.NullableGuidIds.Single(x => x.Id == 2);

                Assert.That(nullEntity.OptionalId, Is.Null);
                Assert.That(nonNullEntity.OptionalId, Is.EqualTo(nonNullId));
            }
        }
    }
}
