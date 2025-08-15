namespace StrongTypeIdGenerator
{
    using System;

    /// <summary>
    /// Represents a descriptor for a component in a combined identifier, containing type and name information.
    /// </summary>
    /// <remarks>
    /// This record is used to define the components that make up a combined identifier when using the
    /// <see cref="CombinedIdAttribute"/>. Each component consists of a <see cref="Type"/> and a corresponding
    /// name that will be used as the property name in the generated identifier class.
    /// </remarks>
    public sealed record ComponentDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentDescriptor"/> record.
        /// </summary>
        /// <param name="type">The type of the component.</param>
        /// <param name="name">The name of the component, which will be used as the property name.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="type"/> is <c>null</c> or <paramref name="name"/> is <c>null</c> or empty.
        /// </exception>
        public ComponentDescriptor(Type type, string name)
        {
            Argument.NotNull(type);
            Argument.NotEmpty(name);

            Type = type;
            Name = name;
        }

        /// <summary>
        /// Gets the type of the component.
        /// </summary>
        /// <value>
        /// The <see cref="System.Type"/> that represents the type of this component in the combined identifier.
        /// </value>
        public Type Type { get; }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        /// <value>
        /// The name that will be used as the property name for this component in the generated identifier class.
        /// </value>
        public string Name { get; }
    }
}