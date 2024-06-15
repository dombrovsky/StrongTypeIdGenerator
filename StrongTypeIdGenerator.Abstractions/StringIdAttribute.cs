namespace StrongTypeIdGenerator
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StringIdAttribute : Attribute
    {
        public bool GenerateConstructorPrivate { get; set; }
    }
}
