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
    /// 
    /// You can customize validation by defining a method with signature:
    /// <code>
    /// private static string CheckValue(string value)
    /// {
    ///     // Validate and optionally transform the value
    ///     return value;
    /// }
    /// </code>
    /// 
    /// You can also customize the property name using <see cref="StringIdAttribute.ValuePropertyName"/> property.
    /// You can also generate a private constructor using <see cref="BaseIdAttribute.GenerateConstructorPrivate"/> property.
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
    ///     
    ///     private static string CheckValue(string value)
    ///     {
    ///         if (string.IsNullOrEmpty(value))
    ///         {
    ///             throw new ArgumentException("Value cannot be null or empty", nameof(value));
    ///         }
    ///         
    ///         // You can also transform the value if needed
    ///         return value.ToUpperInvariant();
    ///     }
    /// }
    /// </code>
    /// 
    /// Using custom property name:
    /// <code>
    /// [StringId(ValuePropertyName = "Text")]
    /// public partial class MyStringId
    /// {
    ///     // The generated class will have a property named 'Text' instead of 'Value'
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StringIdAttribute : BaseScalarIdAttribute
    {
    }
}
