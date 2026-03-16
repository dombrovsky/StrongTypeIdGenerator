namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class NullableStringIdEfCoreFixture
    {
        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanRoundTripNullableStringStrongTypeId(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var nonNullId = new TestStringId("customer-xyz");

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.NullableStringIds.Add(new NullableStringIdEntity { Id = 1, OptionalId = null });
                writeContext.NullableStringIds.Add(new NullableStringIdEntity { Id = 2, OptionalId = nonNullId });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var nullEntity = readContext.NullableStringIds.Single(x => x.Id == 1);
                var nonNullEntity = readContext.NullableStringIds.Single(x => x.Id == 2);

                Assert.That(nullEntity.OptionalId, Is.Null);
                Assert.That(nonNullEntity.OptionalId, Is.EqualTo(nonNullId));
            }
        }

        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanTranslateNullableStringStrongIdNullAndNonNullPredicates(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var targetId = new TestStringId("target-id");

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.NullableStringIds.Add(new NullableStringIdEntity { Id = 1, OptionalId = null });
                writeContext.NullableStringIds.Add(new NullableStringIdEntity { Id = 2, OptionalId = targetId });
                writeContext.NullableStringIds.Add(new NullableStringIdEntity { Id = 3, OptionalId = new TestStringId("other-id") });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var nullIds = readContext.NullableStringIds
                    .Where(x => x.OptionalId == null)
                    .Select(x => x.Id)
                    .ToList();

                var matchingIds = readContext.NullableStringIds
                    .Where(x => x.OptionalId == targetId)
                    .Select(x => x.Id)
                    .ToList();

                Assert.That(nullIds, Has.Count.EqualTo(1));
                Assert.That(nullIds[0], Is.EqualTo(1));

                Assert.That(matchingIds, Has.Count.EqualTo(1));
                Assert.That(matchingIds[0], Is.EqualTo(2));
            }
        }
    }
}