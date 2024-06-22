namespace StrongTypeIdGenerator.Analyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;
    using System;
    using System.Collections.Immutable;
    using System.Text;

    [Generator]
    public sealed class StringIdGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Find all classes with the StringIdentityAttribute
            IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null)!;

            // Combine the selected class declarations with the compilation
            IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses =
                context.CompilationProvider.Combine(classDeclarations.Collect());

            // Generate the source code
            context.RegisterSourceOutput(compilationAndClasses,
                static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }

        static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
            node is ClassDeclarationSyntax classDeclaration &&
            classDeclaration.AttributeLists.Count > 0;

        static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

            // Check if the class has the StrongTypeId attribute
            foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
            {
                foreach (var attributeSyntax in attributeListSyntax.Attributes)
                {
                    if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is IMethodSymbol attributeSymbol)
                    {
                        if (attributeSymbol.ContainingType.ToDisplayString() == "StrongTypeIdGenerator.StringIdAttribute")
                        {
                            return classDeclarationSyntax;
                        }
                    }
                }
            }

            return null;
        }

        static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
        {
            if (classes.IsDefaultOrEmpty)
            {
                return;
            }

            foreach (var classDeclaration in classes)
            {
                var className = classDeclaration.Identifier.Text;
                var namespaceName = GetNamespace(classDeclaration);
                var attributeSyntax = GetStringIdAttributeSyntax(classDeclaration);
                var generateConstructorPrivate = GetGenerateConstructorPrivate(compilation, attributeSyntax);
                var hasCheckValueMethod = HasCheckValueMethod(compilation, classDeclaration);

                var source = GenerateStrongTypeIdClass(namespaceName, className, generateConstructorPrivate, hasCheckValueMethod);

                context.AddSource($"{className}_StrongTypeId.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        static string? GetNamespace(ClassDeclarationSyntax classDeclaration)
        {
            // Walk upwards until we find the namespace declaration
            SyntaxNode? potentialNamespaceParent = classDeclaration.Parent;
            while (potentialNamespaceParent != null &&
                   potentialNamespaceParent is not NamespaceDeclarationSyntax &&
                   potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
            {
                potentialNamespaceParent = potentialNamespaceParent.Parent;
            }

            // Return the namespace name if it was found, otherwise null
            if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceDeclaration)
            {
                return namespaceDeclaration.Name.ToString();
            }

            return null;
        }
        static AttributeSyntax GetStringIdAttributeSyntax(ClassDeclarationSyntax classDeclaration)
        {
            foreach (var attributeListSyntax in classDeclaration.AttributeLists)
            {
                foreach (var attributeSyntax in attributeListSyntax.Attributes)
                {
                    if (attributeSyntax.Name.ToString() == "StringId")
                    {
                        return attributeSyntax;
                    }
                }
            }
            throw new InvalidOperationException("Expected class to have StringId attribute.");
        }

        static bool GetGenerateConstructorPrivate(Compilation compilation, AttributeSyntax attributeSyntax)
        {
            var model = compilation.GetSemanticModel(attributeSyntax.SyntaxTree);
            var attributeSymbol = model.GetSymbolInfo(attributeSyntax).Symbol as IMethodSymbol;
            if (attributeSymbol == null)
            {
                return false;
            }

            if (attributeSyntax.ArgumentList != null)
            {
                foreach (var argument in attributeSyntax.ArgumentList.Arguments)
                {
                    var argumentName = argument.NameEquals?.Name.Identifier.Text;
                    if (argumentName == "GenerateConstructorPrivate" && argument.Expression is LiteralExpressionSyntax literalExpression)
                    {
                        return (bool)model.GetConstantValue(literalExpression).Value!;
                    }
                }
            }

            return false;
        }

        static bool HasCheckValueMethod(Compilation compilation, ClassDeclarationSyntax classDeclaration)
        {
            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;
            if (classSymbol == null)
            {
                return false;
            }

            foreach (var member in classSymbol.GetMembers("CheckValue"))
            {
                if (member is IMethodSymbol methodSymbol &&
                    methodSymbol.IsStatic &&
                    methodSymbol.DeclaredAccessibility == Accessibility.Private &&
                    methodSymbol.Parameters.Length == 1 &&
                    methodSymbol.Parameters[0].Type.SpecialType == SpecialType.System_String)
                {
                    return true;
                }
            }

            return false;
        }

        static string GenerateStrongTypeIdClass(string? namespaceName, string className, bool generateConstructorPrivate, bool hasCheckValueMethod)
        {
            const string TIdentifier = "string";

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
            sourceBuilder.AppendLine($"        {(generateConstructorPrivate ? "private" : "public")} {className}({TIdentifier} value)");
            sourceBuilder.AppendLine("        {");

            if (!generateConstructorPrivate)
            {
                sourceBuilder.AppendLine("            if (value is null)");
                sourceBuilder.AppendLine("            {");
                sourceBuilder.AppendLine("                throw new ArgumentNullException(nameof(value));");
                sourceBuilder.AppendLine("            }");
                sourceBuilder.AppendLine();
            }

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
            sourceBuilder.AppendLine($"            return {TIdentifier}.IsNullOrEmpty(value) ? Unspecified : new {className}(value);");
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
            sourceBuilder.AppendLine("            return other is null ? 1 : string.Compare(Value, other.Value, StringComparison.Ordinal);");
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
            sourceBuilder.AppendLine("            return Value;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public string ToString(string? format, IFormatProvider? formatProvider)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return Value;");
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
            sourceBuilder.AppendLine("                return value.Value;");
            sourceBuilder.AppendLine("            }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"            protected override {className}? InternalConvertFromString(string value)");
            sourceBuilder.AppendLine("            {");
            sourceBuilder.AppendLine($"                return new {className}(value);");
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
