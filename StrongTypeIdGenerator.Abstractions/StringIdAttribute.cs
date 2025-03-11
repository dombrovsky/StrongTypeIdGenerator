namespace StrongTypeIdGenerator
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StringIdAttribute : BaseIdAttribute
    {
        public bool GenerateConstructorPrivate { get; set; }
    }
}
