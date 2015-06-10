namespace Roslinq.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    public class ClassQueryData
    {
        private readonly INamedTypeSymbol classSymbol;

        internal ClassQueryData(INamedTypeSymbol clasSymbol)
        {
            this.classSymbol = clasSymbol;
        }

        public string ClassName
        {
            get { return this.classSymbol.ToString(); }
        }

        internal bool InheritsFrom(Type type)
        {
            return InheritanceHelper.InheritsFrom(this.classSymbol, type);
        }

        internal IEnumerable<IMethodSymbol> Methods
        {
            get
            {
                return classSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Method).Cast<IMethodSymbol>();
            }
        }

        internal bool ImplementsInterface(Type interfaceType)
        {
            if (this.classSymbol.Interfaces == null)
            {
                return false;
            }

            if (!this.classSymbol.Interfaces.Any())
            {
                return false;
            }

            if (this.classSymbol.Interfaces.FirstOrDefault(i => TypeComparer.TypesMatch(i, interfaceType)) != null)
            {
                return true;
            }

            return false;
        }

        internal bool HasAttributeApplied(Type type)
        {
            var classAppliedAttributes = this.classSymbol.GetAttributes();
            foreach (var methodAppliedAttribute in classAppliedAttributes)
            {
                if (TypeComparer.TypesMatch(methodAppliedAttribute.AttributeClass, type))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool HasModifier(ClassModifier modifier)
        {
            switch (modifier)
            {
                case ClassModifier.Private:
                    return this.classSymbol.DeclaredAccessibility == Accessibility.Private;
                case ClassModifier.Protected:
                    return this.classSymbol.DeclaredAccessibility == Accessibility.Protected;
                case ClassModifier.Public:
                    return this.classSymbol.DeclaredAccessibility == Accessibility.Public;
                case ClassModifier.Internal:
                    return this.classSymbol.DeclaredAccessibility == Accessibility.Internal;
                case ClassModifier.Static:
                    return this.classSymbol.IsStatic;
                case ClassModifier.Sealed:
                    return this.classSymbol.IsSealed;
                case ClassModifier.Abstract:
                    return this.classSymbol.IsAbstract;
                default: throw new NotImplementedException(string.Format("Modifier {0} is unknown", modifier));
            }
        }
    }
}