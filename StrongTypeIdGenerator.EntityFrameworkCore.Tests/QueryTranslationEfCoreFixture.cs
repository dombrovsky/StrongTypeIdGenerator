namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class QueryTranslationEfCoreFixture
    {
        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanTranslateWhereForNonKeyStrongIdProperty(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var matchingExternalId = new TestGuidId(Guid.Parse("90889BC7-AD77-45B8-BF30-81FC0A9DAA51"));
            var otherExternalId = new TestGuidId(Guid.Parse("A36A674B-B9D8-4104-A8C8-5CD7C4E7BFAF"));

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.NonKeyStrongIds.Add(new NonKeyStrongIdEntity
                {
                    Id = 1,
                    ExternalId = otherExternalId,
                    Name = "other"
                });
                writeContext.NonKeyStrongIds.Add(new NonKeyStrongIdEntity
                {
                    Id = 2,
                    ExternalId = matchingExternalId,
                    Name = "match"
                });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var match = readContext.NonKeyStrongIds
                    .Single(x => x.ExternalId == matchingExternalId);

                Assert.That(match.Id, Is.EqualTo(2));
                Assert.That(match.Name, Is.EqualTo("match"));
                Assert.That(match.ExternalId, Is.EqualTo(matchingExternalId));
            }
        }

        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanTranslateNullableStrongIdNullAndNonNullPredicates(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var targetId = new TestGuidId(Guid.Parse("D199C0CD-66AB-4CC9-93CF-9F2CBE4B50B3"));

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.NullableGuidIds.Add(new NullableGuidIdEntity { Id = 1, OptionalId = null });
                writeContext.NullableGuidIds.Add(new NullableGuidIdEntity { Id = 2, OptionalId = targetId });
                writeContext.NullableGuidIds.Add(new NullableGuidIdEntity { Id = 3, OptionalId = new TestGuidId(Guid.Parse("37E2F609-9010-4A98-B01A-98BB45D8B671")) });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var nullIds = readContext.NullableGuidIds
                    .Where(x => x.OptionalId == null)
                    .Select(x => x.Id)
                    .ToList();

                var matchingIds = readContext.NullableGuidIds
                    .Where(x => x.OptionalId == targetId)
                    .Select(x => x.Id)
                    .ToList();

                Assert.That(nullIds, Has.Count.EqualTo(1));
                Assert.That(nullIds[0], Is.EqualTo(1));

                Assert.That(matchingIds, Has.Count.EqualTo(1));
                Assert.That(matchingIds[0], Is.EqualTo(2));
            }
        }

        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanTranslateContainsOverStrongIdList(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var id1 = new TestGuidId(Guid.Parse("72D0D2A1-6D13-41C9-B7FC-F8A60C5C0B5B"));
            var id2 = new TestGuidId(Guid.Parse("E8A22A83-2780-42DE-A43C-40A74D74E012"));
            var id3 = new TestGuidId(Guid.Parse("1D8A4F84-FCEE-4EA7-8CAD-1FC5004815EE"));

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.NonKeyStrongIds.Add(new NonKeyStrongIdEntity { Id = 1, ExternalId = id1, Name = "one" });
                writeContext.NonKeyStrongIds.Add(new NonKeyStrongIdEntity { Id = 2, ExternalId = id2, Name = "two" });
                writeContext.NonKeyStrongIds.Add(new NonKeyStrongIdEntity { Id = 3, ExternalId = id3, Name = "three" });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var lookupIds = new[] { id1, id3 };

                var matches = readContext.NonKeyStrongIds
                    .Where(x => lookupIds.Contains(x.ExternalId))
                    .OrderBy(x => x.Id)
                    .Select(x => x.Name)
                    .ToList();

                Assert.That(matches, Has.Count.EqualTo(2));
                Assert.That(matches[0], Is.EqualTo("one"));
                Assert.That(matches[1], Is.EqualTo("three"));
            }
        }

        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanTranslateOrderByOnStrongIdProperty(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var lower = new TestGuidId(Guid.Parse("00000000-0000-0000-0000-000000000001"));
            var middle = new TestGuidId(Guid.Parse("00000000-0000-0000-0000-000000000010"));
            var upper = new TestGuidId(Guid.Parse("00000000-0000-0000-0000-000000000100"));

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.NonKeyStrongIds.Add(new NonKeyStrongIdEntity { Id = 1, ExternalId = middle, Name = "middle" });
                writeContext.NonKeyStrongIds.Add(new NonKeyStrongIdEntity { Id = 2, ExternalId = upper, Name = "upper" });
                writeContext.NonKeyStrongIds.Add(new NonKeyStrongIdEntity { Id = 3, ExternalId = lower, Name = "lower" });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var orderedNames = readContext.NonKeyStrongIds
                    .OrderBy(x => x.ExternalId)
                    .Select(x => x.Name)
                    .ToList();

                Assert.That(orderedNames, Has.Count.EqualTo(3));
                Assert.That(orderedNames[0], Is.EqualTo("lower"));
                Assert.That(orderedNames[1], Is.EqualTo("middle"));
                Assert.That(orderedNames[2], Is.EqualTo("upper"));
            }
        }

        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanProjectStrongIdsAndRoundTripMaterializedResults(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var id1 = new TestGuidId(Guid.Parse("59A9B909-107A-4A64-BBE9-9A5D260A7118"));
            var id2 = new TestGuidId(Guid.Parse("509734E7-5925-43C5-89A1-8D67A95C8A79"));

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.NonKeyStrongIds.Add(new NonKeyStrongIdEntity { Id = 1, ExternalId = id1, Name = "A" });
                writeContext.NonKeyStrongIds.Add(new NonKeyStrongIdEntity { Id = 2, ExternalId = id2, Name = "B" });
                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var projectedIds = readContext.NonKeyStrongIds
                    .OrderBy(x => x.Id)
                    .Select(x => x.ExternalId)
                    .ToList();

                Assert.That(projectedIds, Has.Count.EqualTo(2));
                Assert.That(projectedIds[0], Is.EqualTo(id1));
                Assert.That(projectedIds[1], Is.EqualTo(id2));
            }
        }

        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void CanTranslateCombinedIdComponentPredicates(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            var targetTestGuid = new TestGuidId(Guid.Parse("A98E3D1D-8A6A-4A5B-8838-7D9237B45212"));
            var targetString = "X2";

            using (var writeContext = DbTestHelper.CreateContext(options, mode))
            {
                writeContext.CombinedIds.Add(new CombinedIdEntity
                {
                    Id = 1,
                    CompositeId = new TestCombinedId(
                        new TestGuidId(Guid.Parse("A241354C-B2C0-40D9-A76C-F72CD4D09F29")),
                        "X1",
                        Guid.Parse("08B9D335-E652-4B2F-9089-7A8F5DF96FE9"),
                        10),
                });

                writeContext.CombinedIds.Add(new CombinedIdEntity
                {
                    Id = 2,
                    CompositeId = new TestCombinedId(
                        targetTestGuid,
                        targetString,
                        Guid.Parse("E049D899-16C7-4CBA-8F8F-31A5A7B2A526"),
                        20),
                });

                writeContext.CombinedIds.Add(new CombinedIdEntity
                {
                    Id = 3,
                    CompositeId = new TestCombinedId(
                        targetTestGuid,
                        "X3",
                        Guid.Parse("A5616C0E-4601-40E0-93D8-5A7158D9978A"),
                        30),
                });

                writeContext.SaveChanges();
            }

            using (var readContext = DbTestHelper.CreateContext(options, mode))
            {
                var match = readContext.CombinedIds
                    .Single(x =>
                        x.CompositeId.TestGuid == targetTestGuid &&
                        x.CompositeId.StringId == targetString);

                Assert.That(match.Id, Is.EqualTo(2));
                Assert.That(match.CompositeId.TestGuid, Is.EqualTo(targetTestGuid));
                Assert.That(match.CompositeId.StringId, Is.EqualTo(targetString));
            }
        }
    }
}
