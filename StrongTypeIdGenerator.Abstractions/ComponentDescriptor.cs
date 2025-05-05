namespace StrongTypeIdGenerator
{
    using System;

    public sealed record ComponentDescriptor
    {
        public ComponentDescriptor(Type type, string name)
        {
            Argument.NotNull(type);
            Argument.NotEmpty(name);

            Type = type;
            Name = name;
        }

        public Type Type { get; }

        public string Name { get; }
    }
}