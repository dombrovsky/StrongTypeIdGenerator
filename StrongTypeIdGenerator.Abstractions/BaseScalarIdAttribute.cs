namespace StrongTypeIdGenerator
{
    using System;

    /// <summary>
    /// Abstract base class for scalar identifier attributes that support a single underlying value type.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="BaseIdAttribute"/> and provides common functionality for identifier
    /// attributes that wrap a single scalar value, such as <see cref="GuidIdAttribute"/> and
    /// <see cref="StringIdAttribute"/>. It includes support for customizing the property name that
    /// stores the underlying value.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BaseScalarIdAttribute : BaseIdAttribute
    {
        /// <summary>
        /// Gets or sets the name of the property that stores the underlying value.
        /// </summary>
        /// <value>
        /// The name of the property that will hold the identifier's value in the generated class.
        /// The default value is "Value".
        /// </value>
        /// <remarks>
        /// When specified, the source generator will use this name for the property that holds the identifier's value.
        /// If not specified, the default name "Value" will be used. This allows for more descriptive property names
        /// when appropriate, such as "Id", "Code", "Token", etc.
        /// </remarks>
        /// <example>
        /// <code>
        /// [StringId(ValuePropertyName = "Code")]
        /// public partial class ProductCode
        /// {
        ///     // Generated class will have a 'Code' property instead of 'Value'
        /// }
        /// </code>
        /// </example>
        public string ValuePropertyName { get; set; } = "Value";
    }
}