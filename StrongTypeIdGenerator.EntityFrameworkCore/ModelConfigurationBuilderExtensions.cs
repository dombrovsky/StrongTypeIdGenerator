namespace StrongTypeIdGenerator.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Provides extension methods for registering StrongTypeIdGenerator EF Core conventions on <see cref="ModelConfigurationBuilder"/>.
    /// </summary>
    public static class ModelConfigurationBuilderExtensions
    {
        /// <summary>
        /// Registers StrongTypeIdGenerator conventions in the current <see cref="ModelConfigurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The model configuration builder to configure.</param>
        /// <returns>The same <paramref name="configurationBuilder"/> instance for chaining.</returns>
        /// <remarks>
        /// This adds convention-based mapping for CombinedId generated types as EF Core complex types.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="configurationBuilder"/> is <see langword="null"/>.</exception>
        public static ModelConfigurationBuilder UseStrongTypeIds(this ModelConfigurationBuilder configurationBuilder)
        {
            Argument.NotNull(configurationBuilder);

            configurationBuilder.Conventions.Add(_ => new StrongTypeIdComplexTypeConvention());
            
            return configurationBuilder;
        }
    }
}
