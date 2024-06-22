namespace StrongTypeIdGenerator.Analyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;
    using System.Collections.Immutable;
    using System.Text;

    [Generator]
    public sealed class GuidIdGenerator : BaseIdGenerator
    {
        protected override string MarkerAttributeFullName => "StrongTypeIdGenerator.GuidIdAttribute";

        protected override INamedTypeSymbol GetIdTypeSymbol(Compilation compilation)
        {
            return compilation.GetTypeByMetadataName("System.Guid")!;
        }

        protected override void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
        {
            if (classes.IsDefaultOrEmpty)
            {
                return;
            }

            foreach (var classDeclaration in classes)
            {
                var className = classDeclaration.Identifier.Text;
                var namespaceName = GetNamespace(classDeclaration);
                var hasCheckValueMethod = HasCheckValueMethod(compilation, classDeclaration);

                var source = GenerateStrongTypeIdClass(namespaceName, className, hasCheckValueMethod);

                context.AddSource($"{className}_StrongTypeId.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        static string GenerateStrongTypeIdClass(string? namespaceName, string className, bool hasCheckValueMethod)
        {
            const string TIdentifier = "Guid";

            var sourceBuilder = new StringBuilder();
            sourceBuilder.AppendLine("#nullable enable");

            if (namespaceName is not null)
            {
                sourceBuilder.AppendLine($"namespace {namespaceName}");
                sourceBuilder.AppendLine("{");
            }

            sourceBuilder.AppendLine("    using System;");
            sourceBuilder.AppendLine("    using StrongTypeIdGenerator;");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"    [System.ComponentModel.TypeConverter(typeof({className}Converter))]");
            sourceBuilder.AppendLine($"    partial class {className} : ITypedIdentifier<{className}, {TIdentifier}>");
            sourceBuilder.AppendLine("    {");
            sourceBuilder.AppendLine($"        public {className}({TIdentifier} value)");
            sourceBuilder.AppendLine("        {");

            if (hasCheckValueMethod)
            {
                sourceBuilder.AppendLine("            CheckValue(value);");
                sourceBuilder.AppendLine();
            }

            sourceBuilder.AppendLine("            Value = value;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static {className} Unspecified {{ get; }} = new {className}({TIdentifier}.Empty);");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public {TIdentifier} Value {{ get; }}");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static implicit operator {className}({TIdentifier} value)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return value == Unspecified.Value ? Unspecified : new {className}(value);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static implicit operator {TIdentifier}({className} value)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return value?.Value ?? {TIdentifier}.Empty;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public bool Equals({className}? other)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            if (ReferenceEquals(null, other))");
            sourceBuilder.AppendLine("            {");
            sourceBuilder.AppendLine("                return false;");
            sourceBuilder.AppendLine("            }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("            if (ReferenceEquals(this, other))");
            sourceBuilder.AppendLine("            {");
            sourceBuilder.AppendLine("                return true;");
            sourceBuilder.AppendLine("            }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"            if (other.GetType() != this.GetType())");
            sourceBuilder.AppendLine("            {");
            sourceBuilder.AppendLine("                return false;");
            sourceBuilder.AppendLine("            }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("            return Value.Equals(other.Value);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public int CompareTo({className}? other)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return other is null ? 1 : Value.CompareTo(other.Value);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public override bool Equals(object? obj)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return obj is {className} other && Equals(other);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public override int GetHashCode()");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return Value.GetHashCode();");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public override string ToString()");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return Value.ToString();");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public string ToString(string? format, IFormatProvider? formatProvider)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return Value.ToString(format, formatProvider);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator ==({className} left, {className} right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            if (ReferenceEquals(left, null))");
            sourceBuilder.AppendLine("            {");
            sourceBuilder.AppendLine("                return ReferenceEquals(right, null);");
            sourceBuilder.AppendLine("            }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("            return left.Equals(right);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator !=({className} left, {className} right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return !(left == right);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator <({className} left, {className} right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator <=({className} left, {className} right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator >({className} left, {className} right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator >=({className} left, {className} right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        private sealed partial class {className}Converter : TypeToStringConverter<{className}>");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            protected override string? InternalConvertToString({className} value)");
            sourceBuilder.AppendLine("            {");
            sourceBuilder.AppendLine("                return value.Value.ToString();");
            sourceBuilder.AppendLine("            }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"            protected override {className}? InternalConvertFromString(string value)");
            sourceBuilder.AppendLine("            {");
            sourceBuilder.AppendLine($"                return new {className}(Guid.Parse(value));");
            sourceBuilder.AppendLine("            }");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine("    }");

            if (namespaceName is not null)
            {
                sourceBuilder.AppendLine("}");
            }

            return sourceBuilder.ToString();
        }
    }
}
