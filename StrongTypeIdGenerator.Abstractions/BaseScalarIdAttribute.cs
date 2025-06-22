namespace StrongTypeIdGenerator
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BaseScalarIdAttribute : BaseIdAttribute
    {
        /// <summary>
        /// Gets or sets the name of the property that stores the underlying value.
        /// </summary>
        /// <remarks>
        /// When specified, the source generator will use this name for the property that holds the identifier's value.
        /// If not specified, the default name "Value" will be used.
        /// </remarks>
        public string ValuePropertyName { get; set; } = "Value";
    }
}