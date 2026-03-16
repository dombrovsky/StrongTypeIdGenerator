namespace StrongTypeIdGenerator.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class StrongTypeIdValueConverterSelector : ValueConverterSelector
    {
        private static readonly ConcurrentDictionary<(Type ModelType, Type ProviderType), ValueConverterInfo> ConverterInfoCache = new();

        public StrongTypeIdValueConverterSelector(ValueConverterSelectorDependencies dependencies)
            : base(dependencies)
        {
        }

        public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
        {
            var scalarStrongTypeIdInterface = modelClrType
                .GetInterfaces()
                .FirstOrDefault(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ITypedIdentifier<,>));

            if (scalarStrongTypeIdInterface is not null)
            {
                var identifierType = scalarStrongTypeIdInterface.GetGenericArguments()[1];

                if (providerClrType is null || providerClrType == identifierType)
                {
                    yield return ConverterInfoCache.GetOrAdd(
                        (modelClrType, identifierType),
                        static key => new ValueConverterInfo(
                            key.ModelType,
                            key.ProviderType,
                            _ => (ValueConverter)Activator.CreateInstance(typeof(StrongTypeIdValueConverter<,>).MakeGenericType(key.ModelType, key.ProviderType))!));
                }
            }

            foreach (var valueConverterInfo in base.Select(modelClrType, providerClrType))
            {
                yield return valueConverterInfo;
            }
        }
    }
}
