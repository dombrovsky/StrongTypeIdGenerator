namespace StrongTypeIdGenerator
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a strongly-typed identifier with a value of type <typeparamref name="TIdentifier"/>.
    /// </summary>
    /// <typeparam name="TIdentifier">The type of the underlying value.</typeparam>
    /// <remarks>
    /// This interface is used to define a strongly-typed identifier that wraps a value of type <typeparamref name="TIdentifier"/>.
    /// It ensures type safety and provides a consistent way to access the underlying value.
    /// </remarks>
    [TypeConverter(typeof(TypedIdentifierConverter))]
    public interface ITypedIdentifier<out TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        /// <summary>
        /// Gets the underlying value of the identifier.
        /// </summary>
        TIdentifier Value { get; }
    }

    /// <summary>
    /// Represents a strongly-typed identifier with additional functionality for equality, comparison, and formatting.
    /// </summary>
    /// <typeparam name="TSelf">The type of the implementing class.</typeparam>
    /// <typeparam name="TIdentifier">The type of the underlying value.</typeparam>
    /// <remarks>
    /// This interface extends <see cref="ITypedIdentifier{TIdentifier}"/> and adds support for equality, comparison, and formatting.
    /// </remarks>
#if NET7_0_OR_GREATER
    public interface ITypedIdentifierNoCast<TSelf, TIdentifier> :
#else
    public interface ITypedIdentifierNoCast<TSelf, out TIdentifier> :
#endif
    ITypedIdentifier<TIdentifier>,
        IEquatable<TSelf>,
        IComparable<TSelf>,
        IFormattable
            where TIdentifier : IEquatable<TIdentifier>
            where TSelf : ITypedIdentifierNoCast<TSelf, TIdentifier>
    {
#if NET7_0_OR_GREATER
        /// <summary>
        /// Gets the unspecified value of the identifier.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Desired syntax")]
        static abstract TSelf Unspecified { get; }
#endif
    }

    /// <summary>
    /// Represents a strongly-typed identifier with implicit conversion operators.
    /// </summary>
    /// <typeparam name="TSelf">The type of the implementing class.</typeparam>
    /// <typeparam name="TIdentifier">The type of the underlying value.</typeparam>
    /// <remarks>
    /// This interface extends <see cref="ITypedIdentifierNoCast{TSelf, TIdentifier}"/> and adds support for implicit conversions
    /// between the identifier and its underlying value.
    /// </remarks>
#if NET7_0_OR_GREATER
    public interface ITypedIdentifier<TSelf, TIdentifier> :
#else
    public interface ITypedIdentifier<TSelf, out TIdentifier> :
#endif
    ITypedIdentifierNoCast<TSelf, TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TSelf : ITypedIdentifier<TSelf, TIdentifier>
    {
#if NET7_0_OR_GREATER
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Other languages can still use ctor")]
        static abstract implicit operator TSelf(TIdentifier value);

        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Other languages can still use ctor")]
        static abstract implicit operator TIdentifier(TSelf value);
#endif
    }
}