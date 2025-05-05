namespace StrongTypeIdGenerator
{
    using System;

    /// <summary>
    /// An attribute used to mark a class as a strong-typed identifier based on a <see cref="Guid"/>.
    /// </summary>
    /// <remarks>
    /// This attribute is part of the StrongTypeIdGenerator library and is used in conjunction with source generators
    /// to create immutable, type-safe identifiers. Classes marked with this attribute will be treated as identifiers
    /// that use a <see cref="Guid"/> as their underlying value.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// using StrongTypeIdGenerator;
    /// 
    /// [GuidId]
    /// public partial class MyGuidId
    /// {
    ///     // This class will be treated as a strong-typed identifier for Guid values.
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class GuidIdAttribute : BaseIdAttribute
    {
    }
}