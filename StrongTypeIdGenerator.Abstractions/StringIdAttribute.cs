namespace StrongTypeIdGenerator
{
    using System;

    /// <summary>
    /// An attribute used to mark a class as a strong-typed identifier based on a <see cref="string"/>.
    /// </summary>
    /// <remarks>
    /// This attribute is part of the StrongTypeIdGenerator library and is used in conjunction with source generators
    /// to create immutable, type-safe identifiers. Classes marked with this attribute will be treated as identifiers
    /// that use a <see cref="string"/> as their underlying value.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// using StrongTypeIdGenerator;
    /// 
    /// [StringId(GenerateConstructorPrivate = true)]
    /// public partial class MyStringId
    /// {
    ///     // This class will be treated as a strong-typed identifier for string values.
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StringIdAttribute : BaseIdAttribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the generated constructor should be private.
        /// </summary>
        /// <remarks>
        /// When set to <c>true</c>, the source generator will create a private constructor for the identifier class.
        /// This can be useful for scenarios where you want to restrict instantiation of the identifier to specific methods or factories.
        /// </remarks>
        public bool GenerateConstructorPrivate { get; set; }
    }
}
