namespace StrongTypeIdGenerator
{
    using System;

    /// <summary>
    /// Base class for all identifier attributes used by the StrongTypeIdGenerator library.
    /// </summary>
    /// <remarks>
    /// This abstract class serves as the foundation for all identifier attributes in the StrongTypeIdGenerator
    /// library, providing a common base type for attribute-driven source generation of strongly-typed identifiers.
    /// All concrete identifier attributes (such as <see cref="GuidIdAttribute"/>, <see cref="StringIdAttribute"/>,
    /// and <see cref="CombinedIdAttribute"/>) inherit from this base class.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BaseIdAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the generated constructor should be private.
        /// </summary>
        /// <value>
        /// <c>true</c> if the generated constructor should be private; otherwise, <c>false</c>.
        /// The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When set to <c>true</c>, the source generator will create a private constructor for the identifier class.
        /// This can be useful for scenarios where you want to restrict instantiation of the identifier to specific 
        /// methods or factories, forcing consumers to use factory methods or other controlled creation patterns.
        /// 
        /// This property is available on all identifier attribute types (<see cref="StringIdAttribute"/>, 
        /// <see cref="GuidIdAttribute"/>, and <see cref="CombinedIdAttribute"/>), providing consistent behavior
        /// across all strongly-typed identifier types.
        /// </remarks>
        /// <example>
        /// <para><strong>String ID with private constructor:</strong></para>
        /// <code>
        /// [StringId(GenerateConstructorPrivate = true)]
        /// public partial class SecureToken
        /// {
        ///     public static SecureToken Create(string value)
        ///     {
        ///         // Validation logic here
        ///         return new SecureToken(value);
        ///     }
        /// }
        /// </code>
        /// 
        /// <para><strong>GUID ID with private constructor:</strong></para>
        /// <code>
        /// [GuidId(GenerateConstructorPrivate = true)]
        /// public partial class UserId
        /// {
        ///     public static UserId CreateNew() => new UserId(Guid.NewGuid());
        ///     public static UserId FromString(string value) => new UserId(Guid.Parse(value));
        /// }
        /// </code>
        /// 
        /// <para><strong>Combined ID with private constructor:</strong></para>
        /// <code>
        /// [CombinedId(typeof(Guid), "TenantId", typeof(string), "UserId", GenerateConstructorPrivate = true)]
        /// public partial class CompositeKey
        /// {
        ///     public static CompositeKey CreateForTenant(Guid tenantId, string userId)
        ///     {
        ///         // Business logic validation
        ///         return new CompositeKey(tenantId, userId);
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool GenerateConstructorPrivate { get; set; }
    }
}