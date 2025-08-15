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
    }
}