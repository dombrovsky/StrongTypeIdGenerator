namespace StrongTypeIdGenerator.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System.Collections.Generic;
    using System;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for registering StrongTypeIdGenerator EF Core services on <see cref="DbContextOptionsBuilder"/>.
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Registers StrongTypeIdGenerator EF Core services in the current <see cref="DbContextOptionsBuilder"/>.
        /// </summary>
        /// <param name="optionsBuilder">The EF Core options builder to configure.</param>
        /// <returns>The same <paramref name="optionsBuilder"/> instance for chaining.</returns>
        /// <remarks>
        /// This replaces EF Core's default <see cref="IValueConverterSelector"/> so scalar strong IDs
        /// (for example, GuidId and StringId generated types) are mapped automatically.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="optionsBuilder"/> is <see langword="null"/>.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when <see cref="IValueConverterSelector"/> has already been replaced with a different implementation.
        /// In this case, use <see cref="ModelConfigurationBuilderExtensions.UseStrongTypeIds(ModelConfigurationBuilder)"/>
        /// for CombinedId mapping and configure scalar ID conversions via
        /// <see cref="StrongTypeIdPropertyBuilderExtensions.HasStrongTypeIdConversion{TStrongId, TIdentifier}(Microsoft.EntityFrameworkCore.Metadata.Builders.PropertyBuilder{TStrongId})"/>.
        /// </exception>
        public static DbContextOptionsBuilder UseStrongTypeIds(this DbContextOptionsBuilder optionsBuilder)
        {
            Argument.NotNull(optionsBuilder);

            EnsureNoConflictingValueConverterSelector(optionsBuilder);
            
            return optionsBuilder.ReplaceService<IValueConverterSelector, StrongTypeIdValueConverterSelector>();
        }

        private static void EnsureNoConflictingValueConverterSelector(DbContextOptionsBuilder optionsBuilder)
        {
            var coreOptions = optionsBuilder.Options.FindExtension<CoreOptionsExtension>();
            if (coreOptions is null)
            {
                return;
            }

            var replacedServices = coreOptions.ReplacedServices ??
                new Dictionary<(Type, Type?), Type>();

            var conflictingReplacement = replacedServices
                .Where(kvp => kvp.Key.Item1 == typeof(IValueConverterSelector))
                .Select(kvp => kvp.Value)
                .FirstOrDefault(implementationType => implementationType != typeof(StrongTypeIdValueConverterSelector));

            if (conflictingReplacement is not null)
            {
                throw new InvalidOperationException(
                    $"IValueConverterSelector is already replaced by '{conflictingReplacement.FullName}'. " +
                    $"StrongTypeIdGenerator requires '{typeof(StrongTypeIdValueConverterSelector).FullName}' or no replacement. " +
                    "Workaround: use configurationBuilder.UseStrongTypeIds() for CombinedId mapping and configure scalar ID conversions with HasStrongTypeIdConversion(...)." );
            }
        }
    }
}
