namespace StrongTypeIdGenerator.Analyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;
    using StrongTypeIdGenerator.SourceGenerator;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    [Generator]
    public sealed class CombinedIdGenerator : BaseIdGenerator
    {
        protected override string MarkerAttributeFullName => "StrongTypeIdGenerator.CombinedIdAttribute";

        protected override INamedTypeSymbol GetIdTypeSymbol(Compilation compilation, AttributeData attributeData)
        {
            var components = GetComponents(attributeData);

            var tupleType = compilation.CreateTupleTypeSymbol(
                components.Types.Cast<ITypeSymbol>().ToImmutableArray(),
                components.Names.Cast<string?>().ToImmutableArray());
            return tupleType;
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
                var generateConstructorPrivate = GetAttributeArgumentValue(attributeData, "GenerateConstructorPrivate", fallbackValue: false);
                var hasCheckValueMethod = HasCheckValueMethod(compilation, classDeclaration, attributeData, ensureIdParameter: false);
                var components = GetComponents(attributeData);
                var source = GenerateCombinedIdClass(compilation, namespaceName, className, components.Types, components.Names, generateConstructorPrivate, hasCheckValueMethod);

                context.AddSource($"{className}_CombinedId.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private static (INamedTypeSymbol[] Types, string[] Names) GetComponents(AttributeData attributeData)
        {
            var componentDescriptors = attributeData.ConstructorArguments;
            var types = componentDescriptors.Select(cd => cd.Value as INamedTypeSymbol).Where(symbol => symbol != null).Cast<INamedTypeSymbol>().ToArray();
            var names = componentDescriptors.Select(cd => cd.Value as string).Where(s => s != null).Cast<string>().ToArray();
            return (types, names);
        }

        private static string GenerateCombinedIdClass(Compilation compilation, string? namespaceName, string className, INamedTypeSymbol[] types, string[] names, bool generateConstructorPrivate, bool hasCheckValueMethod)
        {
            // Always implement Value explicitly
            var tupleDefinition = string.Join(", ", types.Select((t, i) => $"{t.ToDisplayString()} {names[i]}"));
            var constructorParameters = string.Join(", ", types.Select((t, i) => $"{t.ToDisplayString()} {names[i].Decapitalize(CultureInfo.InvariantCulture)}"));

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
            sourceBuilder.AppendLine($"    partial class {className} : ITypedIdentifierNoCast<{className}, ({tupleDefinition})>");
            sourceBuilder.AppendLine("    {");

            // Constructor with individual parameters
            sourceBuilder.AppendLine($"        {(generateConstructorPrivate ? "private" : "public")} {className}({constructorParameters})");
            sourceBuilder.AppendLine("        {");

            if (hasCheckValueMethod)
            {
                // Call CheckValue with individual parameters
                sourceBuilder.AppendLine($"            var checkResult = CheckValue({string.Join(", ", names.Select(n => n.Decapitalize(CultureInfo.InvariantCulture)))});");

                // Set individual properties from checkResult
                for (var i = 0; i < types.Length; i++)
                {
                    sourceBuilder.AppendLine($"            {names[i]} = checkResult.{names[i]};");
                }
            }
            else
            {
                // Add comment about CheckValue method signature if not defined
                sourceBuilder.AppendLine($"            // You can add validation by defining: private static ({tupleDefinition}) CheckValue({constructorParameters});");
                
                // Set individual properties directly
                for (var i = 0; i < types.Length; i++)
                {
                    sourceBuilder.AppendLine($"            {names[i]} = {names[i].Decapitalize(CultureInfo.InvariantCulture)};");
                }
            }

            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"            ValuePrivate = ({string.Join(", ", names)});");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();

            sourceBuilder.AppendLine($"        public static {className} Unspecified {{ get; }} = new {className}({string.Join(", ", GetDefaultValueDefinitions())});");

            // Individual component properties
            for (var i = 0; i < types.Length; i++)
            {
                sourceBuilder.AppendLine();
                sourceBuilder.AppendLine($"        public {types[i].ToDisplayString()} {names[i]} {{ get; }}");
            }
            sourceBuilder.AppendLine();

            // Explicitly implement Value property
            sourceBuilder.AppendLine($"        ({tupleDefinition}) ITypedIdentifier<({tupleDefinition})>.Value => ValuePrivate;");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        private ({tupleDefinition}) ValuePrivate {{ get; }}");
            sourceBuilder.AppendLine();

            sourceBuilder.AppendLine($"        public void Deconstruct({string.Join(", ", types.Select((t, i) => $"out {t.ToDisplayString()} {names[i]}"))})");
            sourceBuilder.AppendLine("        {");
            for (var i = 0; i < types.Length; i++)
            {
                sourceBuilder.AppendLine($"            {names[i]} = this.{names[i]};");
            }
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
            sourceBuilder.AppendLine("            return ValuePrivate.Equals(other.ValuePrivate);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public int CompareTo({className}? other)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return other is null ? 1 : ValuePrivate.CompareTo(other.ValuePrivate);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public override bool Equals(object? obj)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine($"            return obj is {className} other && Equals(other);");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("        public override int GetHashCode()");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return ValuePrivate.GetHashCode();");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();

            sourceBuilder.AppendLine("        public override string ToString()");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return ValuePrivate.ToString();");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();

            sourceBuilder.AppendLine("        public string ToString(string? format, IFormatProvider? formatProvider)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return ToString();");
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
            sourceBuilder.AppendLine("            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator <=({className}? left, {className}? right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator >({className}? left, {className}? right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine($"        public static bool operator >=({className}? left, {className}? right)");
            sourceBuilder.AppendLine("        {");
            sourceBuilder.AppendLine("            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine("    }");

            if (namespaceName is not null)
            {
                sourceBuilder.AppendLine("}");
            }

            return sourceBuilder.ToString();

            IEnumerable<string> GetDefaultValueDefinitions()
            {
                foreach (var typeSymbol in types)
                {
                    if (typeSymbol == null)
                    {
                        throw new InvalidOperationException("Type symbol is null");
                    }

                    var baseIdAttribute = compilation.GetTypeByMetadataName("StrongTypeIdGenerator.BaseIdAttribute");
                    var isAnotherStrongIdType = baseIdAttribute != null && typeSymbol.GetAttributes().Any(attr => InheritsFrom(attr.AttributeClass, baseIdAttribute));
                    if (isAnotherStrongIdType)
                    {
                        yield return $"{typeSymbol.ToDisplayString()}.Unspecified";
                        continue;
                    }

                    var equatableInterface = compilation.GetTypeByMetadataName("System.IEquatable`1");
                    if (equatableInterface == null || !typeSymbol.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, equatableInterface) && SymbolEqualityComparer.Default.Equals(i.TypeArguments[0], typeSymbol)))
                    {
                        throw new InvalidOperationException($"{typeSymbol.ToDisplayString()} does not implement IEquatable<{typeSymbol.ToDisplayString()}>");
                    }

                    if (typeSymbol.NullableAnnotation == NullableAnnotation.Annotated)
                    {
                        yield return "null";
                        continue;
                    }

                    var emptyMember = typeSymbol.GetMembers("Empty").FirstOrDefault();
                    if (emptyMember != null && emptyMember.IsStatic)
                    {
                        yield return $"{typeSymbol.ToDisplayString()}.Empty";
                        continue;
                    }

                    yield return "default";
                }

                bool InheritsFrom(INamedTypeSymbol? derivedType, INamedTypeSymbol baseType)
                {
                    while (derivedType != null)
                    {
                        if (SymbolEqualityComparer.Default.Equals(derivedType, baseType))
                        {
                            return true;
                        }

                        derivedType = derivedType.BaseType;
                    }

                    return false;
                }
            }
        }
    }
}

