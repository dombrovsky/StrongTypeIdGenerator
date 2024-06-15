namespace StrongTypeIdGenerator
{
    using System;
    using System.ComponentModel;

    [TypeConverter(typeof(TypedIdentifierConverter))]
    public interface ITypedIdentifier<out TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        TIdentifier Value { get; }
    }

    public interface ITypedIdentifier<TSelf, out TIdentifier> :
        ITypedIdentifier<TIdentifier>,
        IEquatable<TSelf>,
        IComparable<TSelf>,
        IFormattable
            where TIdentifier : IEquatable<TIdentifier>
            where TSelf : ITypedIdentifier<TSelf, TIdentifier>
    {
#if NET7_0_OR_GREATER
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Desired syntax")]
        static abstract TSelf Unspecified { get; }
#endif
    }
}