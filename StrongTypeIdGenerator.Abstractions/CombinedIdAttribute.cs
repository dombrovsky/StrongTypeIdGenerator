namespace StrongTypeIdGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CombinedIdAttribute : BaseIdAttribute
    {
        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3, Type type4, string name4)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3),
                new ComponentDescriptor(type4, name4)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3, Type type4, string name4, Type type5, string name5)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3),
                new ComponentDescriptor(type4, name4),
                new ComponentDescriptor(type5, name5)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3, Type type4, string name4, Type type5, string name5, Type type6, string name6)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3),
                new ComponentDescriptor(type4, name4),
                new ComponentDescriptor(type5, name5),
                new ComponentDescriptor(type6, name6)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3, Type type4, string name4, Type type5, string name5, Type type6, string name6, Type type7, string name7)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3),
                new ComponentDescriptor(type4, name4),
                new ComponentDescriptor(type5, name5),
                new ComponentDescriptor(type6, name6),
                new ComponentDescriptor(type7, name7)
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Types are initialized through the constructor.")]
        public CombinedIdAttribute(Type type1, string name1, Type type2, string name2, Type type3, string name3, Type type4, string name4, Type type5, string name5, Type type6, string name6, Type type7, string name7, Type type8, string name8)
        {
            Components = new[]
            {
                new ComponentDescriptor(type1, name1),
                new ComponentDescriptor(type2, name2),
                new ComponentDescriptor(type3, name3),
                new ComponentDescriptor(type4, name4),
                new ComponentDescriptor(type5, name5),
                new ComponentDescriptor(type6, name6),
                new ComponentDescriptor(type7, name7),
                new ComponentDescriptor(type8, name8)
            };
        }

        public IReadOnlyList<ComponentDescriptor> Components { get; }
    }
}