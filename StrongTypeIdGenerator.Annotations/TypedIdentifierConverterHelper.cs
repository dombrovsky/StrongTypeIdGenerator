namespace StrongTypeIdGenerator
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    internal static class TypedIdentifierConverterHelper
    {
        private static readonly ConcurrentDictionary<Type, Delegate> TypedIdentifierFactories = new();

        public static Func<TIdentifier, object> GetFactory<TIdentifier>(Type typedIdentifierType)
            where TIdentifier : IEquatable<TIdentifier>
        {
            return (Func<TIdentifier, object>)TypedIdentifierFactories.GetOrAdd(
                typedIdentifierType,
                CreateFactory<TIdentifier>);
        }

        public static bool IsTypedIdentifier(Type type)
        {
            return IsTypedIdentifier(type, out _);
        }

        public static bool IsTypedIdentifier(Type type, [NotNullWhen(true)] out Type[]? argumentTypes)
        {
            Argument.NotNull(type);

            var typeIdentifierInterfaceType = type.GetInterfaces().SingleOrDefault(interfaceType =>
                interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ITypedIdentifier<>));

            if (typeIdentifierInterfaceType != null)
            {
                argumentTypes = typeIdentifierInterfaceType.GetGenericArguments();
                return true;
            }

            argumentTypes = null;
            return false;
        }

        private static Func<TIdentifier, object> CreateFactory<TIdentifier>(Type typedIdentifierType)
            where TIdentifier : IEquatable<TIdentifier>
        {
            Argument.Assert(typedIdentifierType, IsTypedIdentifier, $"Type '{typedIdentifierType}' is not a strongly-typed id type");
            var constructor = typedIdentifierType.GetConstructor(new[] { typeof(TIdentifier) });

            if (constructor is null)
            {
                throw new ArgumentException($"Type '{typedIdentifierType}' doesn't have a constructor with one parameter of type '{typeof(TIdentifier)}'", nameof(typedIdentifierType));
            }

            var param = Expression.Parameter(typeof(TIdentifier), constructor.GetParameters()[0].Name);
            var body = Expression.New(constructor, param);
            var lambda = Expression.Lambda<Func<TIdentifier, object>>(body, param);
            return lambda.Compile();
        }
    }
}