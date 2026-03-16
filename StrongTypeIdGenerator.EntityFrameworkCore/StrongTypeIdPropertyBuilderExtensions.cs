namespace StrongTypeIdGenerator.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    /// <summary>
    /// Provides helper methods for configuring explicit EF Core conversions for scalar strongly-typed IDs.
    /// </summary>
    /// <remarks>
    /// These methods are intended for scalar strong IDs such as GuidId and StringId generated types.
    /// CombinedId generated types are mapped as EF Core complex types and should not use these methods directly
    /// on the CombinedId property itself.
    /// </remarks>
    public static class StrongTypeIdPropertyBuilderExtensions
    {
        /// <summary>
        /// Configures a value converter for a non-nullable strongly-typed ID property.
        /// </summary>
        /// <typeparam name="TStrongId">The strongly-typed ID CLR type.</typeparam>
        /// <typeparam name="TIdentifier">The underlying scalar identifier type.</typeparam>
        /// <param name="propertyBuilder">The property builder to configure.</param>
        /// <returns>The same <paramref name="propertyBuilder"/> instance for chaining.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="propertyBuilder"/> is <see langword="null"/>.</exception>
        public static PropertyBuilder<TStrongId> HasStrongTypeIdConversion<TStrongId, TIdentifier>(
            this PropertyBuilder<TStrongId> propertyBuilder)
            where TStrongId : class, ITypedIdentifier<TIdentifier>
            where TIdentifier : IEquatable<TIdentifier>
        {
            Argument.NotNull(propertyBuilder);

            return propertyBuilder.HasConversion(new StrongTypeIdValueConverter<TStrongId, TIdentifier>());
        }

        /// <summary>
        /// Configures a value converter for a nullable strongly-typed ID property.
        /// </summary>
        /// <typeparam name="TStrongId">The strongly-typed ID CLR type.</typeparam>
        /// <typeparam name="TIdentifier">The underlying scalar identifier type.</typeparam>
        /// <param name="propertyBuilder">The nullable property builder to configure.</param>
        /// <returns>The same <paramref name="propertyBuilder"/> instance for chaining.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="propertyBuilder"/> is <see langword="null"/>.</exception>
        public static PropertyBuilder<TStrongId?> HasNullableStrongTypeIdConversion<TStrongId, TIdentifier>(
            this PropertyBuilder<TStrongId?> propertyBuilder)
            where TStrongId : class, ITypedIdentifier<TIdentifier>
            where TIdentifier : struct, IEquatable<TIdentifier>
        {
            Argument.NotNull(propertyBuilder);

            return propertyBuilder.HasConversion(new StrongTypeIdNullableValueConverter<TStrongId, TIdentifier>());
        }

        /// <summary>
        /// Configures a value converter for a non-nullable strongly-typed ID complex-type property.
        /// </summary>
        /// <typeparam name="TStrongId">The strongly-typed ID CLR type.</typeparam>
        /// <typeparam name="TIdentifier">The underlying scalar identifier type.</typeparam>
        /// <param name="propertyBuilder">The complex-type property builder to configure.</param>
        /// <returns>The same <paramref name="propertyBuilder"/> instance for chaining.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="propertyBuilder"/> is <see langword="null"/>.</exception>
        public static ComplexTypePropertyBuilder<TStrongId> HasStrongTypeIdConversion<TStrongId, TIdentifier>(
            this ComplexTypePropertyBuilder<TStrongId> propertyBuilder)
            where TStrongId : class, ITypedIdentifier<TIdentifier>
            where TIdentifier : IEquatable<TIdentifier>
        {
            Argument.NotNull(propertyBuilder);

            return propertyBuilder.HasConversion(new StrongTypeIdValueConverter<TStrongId, TIdentifier>());
        }

        /// <summary>
        /// Configures a value converter for a nullable strongly-typed ID complex-type property.
        /// </summary>
        /// <typeparam name="TStrongId">The strongly-typed ID CLR type.</typeparam>
        /// <typeparam name="TIdentifier">The underlying scalar identifier type.</typeparam>
        /// <param name="propertyBuilder">The nullable complex-type property builder to configure.</param>
        /// <returns>The same <paramref name="propertyBuilder"/> instance for chaining.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="propertyBuilder"/> is <see langword="null"/>.</exception>
        public static ComplexTypePropertyBuilder<TStrongId?> HasNullableStrongTypeIdConversion<TStrongId, TIdentifier>(
            this ComplexTypePropertyBuilder<TStrongId?> propertyBuilder)
            where TStrongId : class, ITypedIdentifier<TIdentifier>
            where TIdentifier : struct, IEquatable<TIdentifier>
        {
            Argument.NotNull(propertyBuilder);

            return propertyBuilder.HasConversion(new StrongTypeIdNullableValueConverter<TStrongId, TIdentifier>());
        }
    }
}
