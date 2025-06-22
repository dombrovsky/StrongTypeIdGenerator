namespace StrongTypeIdGenerator.Analyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;
    using StrongTypeIdGenerator.SourceGenerator;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    [Generator]
    public sealed class GuidIdGenerator : BaseIdGenerator
    {
        protected override string MarkerAttributeFullName => "StrongTypeIdGenerator.GuidIdAttribute";

        protected override INamedTypeSymbol GetIdTypeSymbol(Compilation compilation, AttributeData attributeData)
        {
            return compilation.GetTypeByMetadataName("System.Guid")!;
        }

        protected override void Execute(Compilation compilation, ImmutableArray<(ClassDeclarationSyntax ClassSyntax, AttributeData Attribute)?> classes, SourceProductionContext context)
        {
            if (classes.IsDefaultOrEmpty)
            {
                return;
            }

            foreach (var (classDeclaration, attributeData) in classes.Where(x => x.HasValue).Select(tuple => tuple!.Value))
            {
                var className = classDeclaration.Identifier.Text;
                var namespaceName = GetNamespace(classDeclaration);
                var valuePropertyName = GetAttributeArgumentValue(attributeData, "ValuePropertyName", fallbackValue: "Value");
                var hasCheckValueMethod = HasCheckValueMethod(compilation, classDeclaration, attributeData, ensureIdParameter: true);

                var source = GenerateStrongTypeIdClass(namespaceName, className, hasCheckValueMethod, valuePropertyName);

                context.AddSource($"{className}_StrongTypeId.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        static string GenerateStrongTypeIdClass(string? namespaceName, string className, bool hasCheckValueMethod, string valuePropertyName)
        {
            const string TIdentifier = "Guid";
            var needsExplicitInterfaceImplementation = valuePropertyName != "Value";
            var valueParameterName = valuePropertyName.Decapitalize(CultureInfo.InvariantCulture);

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
            sourceBuilder.AppendLine($"        public {className}({TIdentifier} {valueParameterName})");
            sourceBuilder.AppendLine("        {");

            if (hasCheckValueMethod)
            {
                sourceBuilder.AppendLine($"            {valuePropertyName} = CheckValue({valueParameterName});");
            }
            else
            {
                sourceBuilder.AppendLine($"            // You can add validation by defining: private static {TIdentifier} CheckValue({TIdentifier} {valueParameterName});");
                sourceBuilder.AppendLine($"            {valuePropertyName} = {valueParameterName};");
            }

            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static {className} Unspecified {{ get; }} = new {className}({TIdentifier}.Empty);");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public {TIdentifier} {valuePropertyName} {{ get; }}");
            sourceBuilder.AppendLine();

            if (needsExplicitInterfaceImplementation)
            {
                sourceBuilder.AppendLine($"        {TIdentifier} ITypedIdentifier<{TIdentifier}>.Value => {valuePropertyName};");
                sourceBuilder.AppendLine();
            }

            sourceBuilder.AppendLine($"        public static implicit operator {className}({TIdentifier} value)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return value == Unspecified.{valuePropertyName} ? Unspecified : new {className}(value);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static implicit operator {TIdentifier}({className} value)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return value?.{valuePropertyName} ?? {TIdentifier}.Empty;");
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
            sourceBuilder.AppendLine($"            return {valuePropertyName}.Equals(other.{valuePropertyName});");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public int CompareTo({className}? other)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return other is null ? 1 : {valuePropertyName}.CompareTo(other.{valuePropertyName});");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public override bool Equals(object? obj)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return obj is {className} other && Equals(other);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public override int GetHashCode()");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return {valuePropertyName}.GetHashCode();");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public override string ToString()");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return {valuePropertyName}.ToString();");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public string ToString(string? format, IFormatProvider? formatProvider)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return {valuePropertyName}.ToString(format, formatProvider);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator ==({className}? left, {className}? right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            if (ReferenceEquals(left, null))");
            sourceBuilder.AppendLine("            {");
            sourceBuilder.AppendLine("                return ReferenceEquals(right, null);");
            sourceBuilder.AppendLine("            }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("            return left.Equals(right);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator !=({className}? left, {className}? right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return !(left == right);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator <({className}? left, {className}? right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator <=({className}? left, {className}? right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator >({className}? left, {className}? right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator >=({className}? left, {className}? right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        private sealed partial class {className}Converter : TypeToStringConverter<{className}>");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            protected override string? InternalConvertToString({className} value)");
            sourceBuilder.AppendLine("            {");
            sourceBuilder.AppendLine($"                return value.{valuePropertyName}.ToString();");
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
