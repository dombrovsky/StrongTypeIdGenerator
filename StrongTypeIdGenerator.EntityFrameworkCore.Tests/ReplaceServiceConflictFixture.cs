namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using StrongTypeIdGenerator.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class ReplaceServiceConflictFixture
    {
        [Test]
        public void UseStrongTypeIds_CanBeCalledMultipleTimes()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();

            Assert.DoesNotThrow(() => optionsBuilder.UseStrongTypeIds());
            Assert.DoesNotThrow(() => optionsBuilder.UseStrongTypeIds());
        }

        [Test]
        public void UseStrongTypeIds_DoesNotThrowWhenAlreadyConfiguredWithExpectedSelector()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseStrongTypeIds();

            Assert.DoesNotThrow(() => optionsBuilder.UseStrongTypeIds());
        }

        [Test]
        public void UseStrongTypeIds_ThrowsWhenSelectorAlreadyReplacedByAnotherLibrary()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.ReplaceService<IValueConverterSelector, AlternativeValueConverterSelector>();

            var exception = Assert.Throws<InvalidOperationException>(() => optionsBuilder.UseStrongTypeIds());

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception!.Message, Does.Contain(nameof(IValueConverterSelector)));
            Assert.That(exception.Message, Does.Contain("configurationBuilder.UseStrongTypeIds()"));
            Assert.That(exception.Message, Does.Contain("HasStrongTypeIdConversion"));
        }

        [Test]
        public void Workaround_CombinedConventionAndExplicitModelBuilderConversion_WorksWhenAnotherLibraryReplacesSelector()
        {
            using var connection = DbTestHelper.CreateOpenConnection();

            var optionsBuilder = new DbContextOptionsBuilder<WorkaroundDbContext>();
            optionsBuilder.UseSqlite(connection);
            optionsBuilder.ReplaceService<IValueConverterSelector, AlternativeValueConverterSelector>();
            var options = optionsBuilder.Options;

            using (var setupContext = new WorkaroundDbContext(options))
            {
                setupContext.Database.EnsureCreated();
            }

            var strongId = new TestGuidId(Guid.Parse("03F807D2-C0DF-414E-BDF4-BF4843CE49D8"));
            var combinedId = new TestCombinedId(
                new TestGuidId(Guid.Parse("C560341E-008D-47A9-8A45-7B6A8D5362E8")),
                "C1",
                Guid.Parse("6AA0A86B-23E0-4DE6-9C66-25031F53B920"),
                77);

            using (var writeContext = new WorkaroundDbContext(options))
            {
                writeContext.Entities.Add(new WorkaroundEntity
                {
                    Id = 1,
                    StrongId = strongId,
                    CombinedId = combinedId,
                });
                writeContext.SaveChanges();
            }

            using (var readContext = new WorkaroundDbContext(options))
            {
                var entity = readContext.Entities.Single(x => x.StrongId == strongId);
                Assert.That(entity.StrongId, Is.EqualTo(strongId));
                Assert.That(entity.CombinedId, Is.EqualTo(combinedId));
            }
        }

        private sealed class AlternativeValueConverterSelector : ValueConverterSelector
        {
            public AlternativeValueConverterSelector(ValueConverterSelectorDependencies dependencies)
                : base(dependencies)
            {
            }

            public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
            {
                foreach (var converter in base.Select(modelClrType, providerClrType))
                {
                    yield return converter;
                }
            }
        }

        private sealed class WorkaroundDbContext(DbContextOptions<WorkaroundDbContext> options) : DbContext(options)
        {
            public DbSet<WorkaroundEntity> Entities => Set<WorkaroundEntity>();

            protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
            {
                configurationBuilder.UseStrongTypeIds();
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<WorkaroundEntity>().HasKey(x => x.Id);

                // Workaround when another library owns IValueConverterSelector replacement.
                modelBuilder.Entity<WorkaroundEntity>()
                    .Property(x => x.StrongId)
                    .HasConversion(
                        value => value.Value,
                        value => new TestGuidId(value));
            }
        }

        private sealed class WorkaroundEntity
        {
            public int Id { get; set; }

            public required TestGuidId StrongId { get; set; }

            public required TestCombinedId CombinedId { get; set; }
        }
    }
}
