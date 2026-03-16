namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    using StrongTypeIdGenerator.EntityFrameworkCore;

    internal enum RegistrationMode
    {
        OptionsBuilder,
        OnModelCreating,
    }

    internal static class DbTestHelper
    {
        public static SqliteConnection CreateOpenConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            return connection;
        }

        public static DbContextOptions CreateOptions(SqliteConnection connection, RegistrationMode mode)
        {
            switch (mode)
            {
                case RegistrationMode.OptionsBuilder:
                    var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
                    optionsBuilder.UseSqlite(connection);
                    optionsBuilder.UseStrongTypeIds();
                    return optionsBuilder.Options;

                case RegistrationMode.OnModelCreating:
                    var modelBuilderOptions = new DbContextOptionsBuilder<ModelBuilderConfiguredDbContext>();
                    modelBuilderOptions.UseSqlite(connection);
                    return modelBuilderOptions.Options;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unsupported registration mode.");
            }
        }

        public static TestDbContextBase CreateContext(DbContextOptions options, RegistrationMode mode)
        {
            return mode switch
            {
                RegistrationMode.OptionsBuilder => new TestDbContext((DbContextOptions<TestDbContext>)options),
                RegistrationMode.OnModelCreating => new ModelBuilderConfiguredDbContext((DbContextOptions<ModelBuilderConfiguredDbContext>)options),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unsupported registration mode."),
            };
        }

        public static void EnsureCreated(DbContextOptions options, RegistrationMode mode)
        {
            using var context = CreateContext(options, mode);
            context.Database.EnsureCreated();
        }
    }
}
