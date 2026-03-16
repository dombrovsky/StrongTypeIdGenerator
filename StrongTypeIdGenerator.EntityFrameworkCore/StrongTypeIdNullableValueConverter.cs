namespace StrongTypeIdGenerator.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System;
    using System.Linq.Expressions;

    internal sealed class StrongTypeIdNullableValueConverter<TStrongId, TIdentifier> : ValueConverter<TStrongId?, TIdentifier?>
        where TStrongId : class, ITypedIdentifier<TIdentifier>
        where TIdentifier : struct, IEquatable<TIdentifier>
    {
        public StrongTypeIdNullableValueConverter()
            : base(
                value => value == null ? (TIdentifier?)null : value.Value,
                CreateFromProvider())
        {
        }

        private static Expression<Func<TIdentifier?, TStrongId?>> CreateFromProvider()
        {
            var valueParameter = Expression.Parameter(typeof(TIdentifier?), "value");
            var hasValueExpression = Expression.Property(valueParameter, nameof(Nullable<TIdentifier>.HasValue));
            var valueExpression = Expression.Property(valueParameter, nameof(Nullable<TIdentifier>.Value));
            var convertedValue = Expression.Convert(valueExpression, typeof(TStrongId));
            var nullValue = Expression.Constant(null, typeof(TStrongId));
            var body = Expression.Condition(hasValueExpression, convertedValue, nullValue);

            return Expression.Lambda<Func<TIdentifier?, TStrongId?>>(body, valueParameter);
        }
    }
}