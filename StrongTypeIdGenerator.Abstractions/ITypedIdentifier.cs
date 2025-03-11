namespace StrongTypeIdGenerator
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    [TypeConverter(typeof(TypedIdentifierConverter))]
    public interface ITypedIdentifier<out TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        TIdentifier Value { get; }
    }
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Desired syntax")]
        static abstract TSelf Unspecified { get; }
#endif
    }

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