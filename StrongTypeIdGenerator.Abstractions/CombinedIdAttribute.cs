namespace StrongTypeIdGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// An attribute used to mark a class as a strong-typed identifier that combines multiple components.
    /// </summary>
    /// <remarks>
    /// This attribute is part of the StrongTypeIdGenerator library and is used in conjunction with source generators
    /// to create immutable, type-safe identifiers that combine multiple components (e.g., <see cref="Guid"/>, <see cref="string"/>, <see cref="int"/>).
    /// Each component is defined by its type and a corresponding name.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// using StrongTypeIdGenerator;
    /// 
    /// [CombinedId(typeof(Guid), "Id1", typeof(string), "Id2")]
    /// public partial class MyCombinedId
    /// {
    ///     // This class will be treated as a strong-typed identifier combining Guid and string values.
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CombinedIdAttribute : BaseIdAttribute
    {
        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3, Type type4, string name4)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3),
                new ComponentDescriptor(type4, name4)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3, Type type4, string name4, Type type5, string name5)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3),
                new ComponentDescriptor(type4, name4),
                new ComponentDescriptor(type5, name5)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3, Type type4, string name4, Type type5, string name5, Type type6, string name6)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3),
                new ComponentDescriptor(type4, name4),
                new ComponentDescriptor(type5, name5),
                new ComponentDescriptor(type6, name6)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3, Type type4, string name4, Type type5, string name5, Type type6, string name6, Type type7, string name7)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3),
                new ComponentDescriptor(type4, name4),
                new ComponentDescriptor(type5, name5),
                new ComponentDescriptor(type6, name6),
                new ComponentDescriptor(type7, name7)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3, Type type4, string name4, Type type5, string name5, Type type6, string name6, Type type7, string name7, Type type8, string name8)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3),
                new ComponentDescriptor(type4, name4),
                new ComponentDescriptor(type5, name5),
                new ComponentDescriptor(type6, name6),
                new ComponentDescriptor(type7, name7),
                new ComponentDescriptor(type8, name8)
            };
        }

        /// <summary>
        /// Gets the list of components that make up the combined identifier.
        /// </summary>
        /// <remarks>
        /// Each component is represented by a <see cref="ComponentDescriptor"/>, which includes the type and name of the component.
        /// </remarks>
        public IReadOnlyList<ComponentDescriptor> Components { get; }
    }
}