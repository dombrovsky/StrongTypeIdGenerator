namespace StrongTypeIdGenerator.EntityFrameworkCore.Tests
{
    internal sealed class MaterializationValidationEfCoreFixture
    {
        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void ThrowsWhenGuidCheckValueRejectsStoredValue(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            InsertRawCheckValueGuidRow(connection, 1, CheckValueGuidId.InvalidValue);

            using var readContext = DbTestHelper.CreateContext(options, mode);

            Assert.Throws<ArgumentException>(() => readContext.CheckValueGuidIds.Single(x => x.Id == 1));
        }

        [TestCase(RegistrationMode.OptionsBuilder)]
        [TestCase(RegistrationMode.OnModelCreating)]
        public void ThrowsWhenStringCheckValueRejectsStoredValue(RegistrationMode mode)
        {
            using var connection = DbTestHelper.CreateOpenConnection();
            var options = DbTestHelper.CreateOptions(connection, mode);
            DbTestHelper.EnsureCreated(options, mode);

            InsertRawCheckValueStringRow(connection, 1, "string-value-that-is-too-long");

            using var readContext = DbTestHelper.CreateContext(options, mode);

            Assert.Throws<ArgumentException>(() => readContext.CheckValueStringIds.Single(x => x.Id == 1));
        }

        private static void InsertRawCheckValueGuidRow(SqliteConnection connection, int id, Guid value)
        {
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO CheckValueGuidIds (Id, StrongId) VALUES ($id, $strongId);";
            command.Parameters.AddWithValue("$id", id);
            command.Parameters.AddWithValue("$strongId", value);
            command.ExecuteNonQuery();
        }

        private static void InsertRawCheckValueStringRow(SqliteConnection connection, int id, string value)
        {
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO CheckValueStringIds (Id, StrongId) VALUES ($id, $strongId);";
            command.Parameters.AddWithValue("$id", id);
            command.Parameters.AddWithValue("$strongId", value);
            command.ExecuteNonQuery();
        }
    }
}