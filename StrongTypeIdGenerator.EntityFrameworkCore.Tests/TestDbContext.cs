namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    using Microsoft.EntityFrameworkCore;

    internal abstract class TestDbContextBase(DbContextOptions options) : DbContext(options)
    {
        public DbSet<GuidIdEntity> GuidIds => Set<GuidIdEntity>();

        public DbSet<StringIdEntity> StringIds => Set<StringIdEntity>();

        public DbSet<CombinedIdEntity> CombinedIds => Set<CombinedIdEntity>();

        public DbSet<NullableGuidIdEntity> NullableGuidIds => Set<NullableGuidIdEntity>();

        public DbSet<NullableStringIdEntity> NullableStringIds => Set<NullableStringIdEntity>();

        public DbSet<CheckValueGuidIdEntity> CheckValueGuidIds => Set<CheckValueGuidIdEntity>();

        public DbSet<CheckValueStringIdEntity> CheckValueStringIds => Set<CheckValueStringIdEntity>();

        public DbSet<PrivateGuidIdEntity> PrivateGuidIds => Set<PrivateGuidIdEntity>();

        public DbSet<GuidIdWithPropertyNameEntity> GuidIdsWithPropertyName => Set<GuidIdWithPropertyNameEntity>();

        public DbSet<StringIdWithPropertyNameEntity> StringIdsWithPropertyName => Set<StringIdWithPropertyNameEntity>();

        public DbSet<NonKeyStrongIdEntity> NonKeyStrongIds => Set<NonKeyStrongIdEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuidIdEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<StringIdEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<CombinedIdEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<NullableGuidIdEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<NullableStringIdEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<CheckValueGuidIdEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<CheckValueStringIdEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<PrivateGuidIdEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<GuidIdWithPropertyNameEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<StringIdWithPropertyNameEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<NonKeyStrongIdEntity>().HasKey(x => x.Id);

            ConfigureStrongIdMappings(modelBuilder);
        }

        protected virtual void ConfigureStrongIdMappings(ModelBuilder modelBuilder)
        {
        }
    }

    internal sealed class TestDbContext(DbContextOptions<TestDbContext> options) : TestDbContextBase(options)
    {
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.UseStrongTypeIds();
        }
    }

    internal sealed class ModelBuilderConfiguredDbContext(DbContextOptions<ModelBuilderConfiguredDbContext> options) : TestDbContextBase(options)
    {
        protected override void ConfigureStrongIdMappings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuidIdEntity>()
                .Property(x => x.Id)
                .HasStrongTypeIdConversion<TestGuidId, Guid>();

            modelBuilder.Entity<StringIdEntity>()
                .Property(x => x.Id)
                .HasStrongTypeIdConversion<TestStringId, string>();

            var combinedIdBuilder = modelBuilder.Entity<CombinedIdEntity>()
                .ComplexProperty(x => x.CompositeId);

            combinedIdBuilder
                .Property(x => x.TestGuid)
                .HasStrongTypeIdConversion<TestGuidId, Guid>();
            combinedIdBuilder.Property(x => x.StringId);
            combinedIdBuilder.Property(x => x.GuidId);
            combinedIdBuilder.Property(x => x.IntId);

            modelBuilder.Entity<NullableGuidIdEntity>()
                .Property(x => x.OptionalId)
                .HasNullableStrongTypeIdConversion<TestGuidId, Guid>();

            modelBuilder.Entity<NullableStringIdEntity>()
                .Property(x => x.OptionalId)
                .HasConversion(
                    value => value == null ? null : value.Value,
                    value => value == null ? null : new TestStringId(value));

            modelBuilder.Entity<CheckValueGuidIdEntity>()
                .Property(x => x.StrongId)
                .HasStrongTypeIdConversion<CheckValueGuidId, Guid>();

            modelBuilder.Entity<CheckValueStringIdEntity>()
                .Property(x => x.StrongId)
                .HasStrongTypeIdConversion<CheckValueStringId, string>();

            modelBuilder.Entity<PrivateGuidIdEntity>()
                .Property(x => x.StrongId)
                .HasStrongTypeIdConversion<TestGuidIdPrivateConstructor, Guid>();

            modelBuilder.Entity<GuidIdWithPropertyNameEntity>()
                .Property(x => x.Id)
                .HasStrongTypeIdConversion<TestGuidIdWithPropertyName, Guid>();

            modelBuilder.Entity<StringIdWithPropertyNameEntity>()
                .Property(x => x.Id)
                .HasStrongTypeIdConversion<TestStringIdWithPropertyName, string>();

            modelBuilder.Entity<NonKeyStrongIdEntity>()
                .Property(x => x.ExternalId)
                .HasStrongTypeIdConversion<TestGuidId, Guid>();
        }
    }
}
