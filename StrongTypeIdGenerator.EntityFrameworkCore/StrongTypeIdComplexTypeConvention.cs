namespace StrongTypeIdGenerator.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore.Metadata.Conventions;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System;
    using System.Linq;
    using System.Reflection;

    internal sealed class StrongTypeIdComplexTypeConvention : IEntityTypeAddedConvention
    {
        public void ProcessEntityTypeAdded(
            IConventionEntityTypeBuilder entityTypeBuilder,
            IConventionContext<IConventionEntityTypeBuilder> context)
        {
            var clrType = entityTypeBuilder.Metadata.ClrType;
            if (clrType is null)
            {
                return;
            }

            var properties = clrType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(static property => IsCombinedStrongTypeId(property.PropertyType));

            foreach (var property in properties)
            {
                var complexPropertyBuilder = entityTypeBuilder.ComplexProperty(property.PropertyType, property.Name);
                if (complexPropertyBuilder is null)
                {
                    continue;
                }

                RegisterComponentProperties(complexPropertyBuilder, property.PropertyType);
            }
        }

        private static void RegisterComponentProperties(IConventionComplexPropertyBuilder complexPropertyBuilder, Type complexClrType)
        {
            var complexTypeBuilder = complexPropertyBuilder.Metadata.ComplexType.Builder;

            foreach (var componentProperty in complexClrType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var componentType = componentProperty.PropertyType;
                var propBuilder = complexTypeBuilder.Property(componentType, componentProperty.Name);
                if (propBuilder is null)
                {
                    continue;
                }

                if (IsScalarStrongTypeId(componentType))
                {
                    var identifierType = GetScalarIdentifierType(componentType);
                    if (identifierType is not null)
                    {
                        var converter = (ValueConverter)Activator.CreateInstance(
                            typeof(StrongTypeIdValueConverter<,>).MakeGenericType(componentType, identifierType))!;
                        propBuilder.HasConversion(converter);
                    }
                }
            }
        }

        private static bool IsCombinedStrongTypeId(Type type)
        {
            var interfaces = type.GetInterfaces();
            var hasNoCast = interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITypedIdentifierNoCast<,>));
            var hasScalarStrongTypeId = interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITypedIdentifier<,>));

            return hasNoCast && !hasScalarStrongTypeId;
        }

        private static bool IsScalarStrongTypeId(Type type)
        {
            return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITypedIdentifier<,>));
        }

        private static Type? GetScalarIdentifierType(Type type)
        {
            return type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITypedIdentifier<,>))
                ?.GetGenericArguments()[1];
        }
    }
}
