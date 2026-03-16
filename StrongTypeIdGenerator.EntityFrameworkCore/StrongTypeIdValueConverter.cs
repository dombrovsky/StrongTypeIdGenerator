namespace StrongTypeIdGenerator.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System;
    using System.Linq.Expressions;

    internal sealed class StrongTypeIdValueConverter<TId, TIdentifier> : ValueConverter<TId, TIdentifier>
        where TId : ITypedIdentifier<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        public StrongTypeIdValueConverter()
            : base(
                id => id.Value,
                CreateFromProvider())
        {
        }

        private static Expression<Func<TIdentifier, TId>> CreateFromProvider()
        {
            var valueParameter = Expression.Parameter(typeof(TIdentifier), "value");
            var convertedValue = Expression.Convert(valueParameter, typeof(TId));
            return Expression.Lambda<Func<TIdentifier, TId>>(convertedValue, valueParameter);
        }
    }
}
