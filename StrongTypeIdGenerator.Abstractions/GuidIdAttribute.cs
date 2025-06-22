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
    /// 
    /// You can customize validation by defining a method with signature:
    /// <code>
    /// private static Guid CheckValue(Guid value)
    /// {
    ///     // Validate and optionally transform the value
    ///     return value;
    /// }
    /// </code>
    /// 
    /// You can also customize the property name using <see cref="GuidIdAttribute.ValuePropertyName"/> property.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// using StrongTypeIdGenerator;
    /// 
    /// [GuidId]
    /// public partial class MyGuidId
    /// {
    ///     private static Guid CheckValue(Guid value)
    ///     {
    ///         if (value == Guid.Empty)
    ///         {
    ///             throw new ArgumentException("Empty GUID is not allowed", nameof(value));
    ///         }
    ///         return value;
    ///     }
    /// }
    /// </code>
    /// 
    /// Using custom property name:
    /// <code>
    /// [GuidId(ValuePropertyName = "Uuid")]
    /// public partial class MyGuidId
    /// {
    ///     // The generated class will have a property named 'Uuid' instead of 'Value'
    /// }
    /// </code>
    /// </example>
    public sealed class GuidIdAttribute : BaseScalarIdAttribute
    {
    }
}