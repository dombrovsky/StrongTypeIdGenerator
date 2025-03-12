namespace StrongTypeIdGenerator.Analyzer
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Immutable;
    using System.Linq;

    public abstract class BaseIdGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null)!;

            var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndClasses,
                (spc, source) => Execute(source.Item1, source.Item2, spc));
        }

        protected static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
            node is ClassDeclarationSyntax classDeclaration &&
            classDeclaration.AttributeLists.Count > 0;

        protected static T GetAttributeArgumentValue<T>(AttributeData attributeData, string memberName, T fallbackValue)
        {
            if (attributeData == null)
            {
                throw new ArgumentNullException(nameof(attributeData));
            }

            var value = attributeData.NamedArguments.FirstOrDefault(pair => pair.Key == memberName).Value.Value;
            return value is T tValue ? tValue : fallbackValue;
        }

        private (ClassDeclarationSyntax ClassSyntax, AttributeData Attribute)? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

            foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
            {
                foreach (var attributeSyntax in attributeListSyntax.Attributes)
                {
                    if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is IMethodSymbol attributeSymbol)
                    {
                        if (attributeSymbol.ContainingType.ToDisplayString() == MarkerAttributeFullName)
                        {
                            // Retrieve the full attribute data
                            var attributeData = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax)?
                                .GetAttributes()
                                .FirstOrDefault(attr => attr.AttributeClass?.ToDisplayString() == MarkerAttributeFullName);

                            if (attributeData is not null)
                            {
                                return (classDeclarationSyntax, attributeData);
                            }
                        }
                    }
                }
            }

            return null;
        }

        protected abstract string MarkerAttributeFullName { get; }

        protected abstract INamedTypeSymbol GetIdTypeSymbol(Compilation compilation, AttributeData attributeData);

        protected abstract void Execute(Compilation compilation, ImmutableArray<(ClassDeclarationSyntax ClassSyntax, AttributeData Attribute)?> classes, SourceProductionContext context);

        protected static string? GetNamespace(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration is null)
            {
                throw new ArgumentNullException(nameof(classDeclaration));
            }

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

        protected bool HasCheckValueMethod(Compilation compilation, ClassDeclarationSyntax classDeclaration, AttributeData attributeData)
        {
            if (compilation is null)
            {
                throw new ArgumentNullException(nameof(compilation));
            }

            if (classDeclaration is null)
            {
                throw new ArgumentNullException(nameof(classDeclaration));
            }

            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;
            if (classSymbol is null)
            {
                return false;
            }

            var idTypeSymbol = GetIdTypeSymbol(compilation, attributeData);

            foreach (var member in classSymbol.GetMembers("CheckValue"))
            {
                if (member is IMethodSymbol methodSymbol &&
                    methodSymbol.IsStatic &&
                    methodSymbol.DeclaredAccessibility == Accessibility.Private &&
                    methodSymbol.Parameters.Length == 1 &&
                    SymbolEqualityComparer.Default.Equals(methodSymbol.Parameters[0].Type, idTypeSymbol))
                {
                    return true;
                }
            }

            return false;
        }
    }
}