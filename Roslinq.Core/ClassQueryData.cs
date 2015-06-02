namespace Roslinq.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    public class ClassQueryData
    {
        private readonly INamedTypeSymbol classSymbol;

        public ClassQueryData(INamedTypeSymbol clasSymbol)
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

        internal bool HasModifier(int modifier)
        {
            switch (modifier)
            {
                case Modifiers.Class.Private:
                    return this.classSymbol.DeclaredAccessibility == Accessibility.Private;
                case Modifiers.Class.Protected:
                    return this.classSymbol.DeclaredAccessibility == Accessibility.Protected;
                case Modifiers.Class.Public:
                    return this.classSymbol.DeclaredAccessibility == Accessibility.Public;
                case Modifiers.Class.Internal:
                    return this.classSymbol.DeclaredAccessibility == Accessibility.Internal;
                case Modifiers.Class.Static:
                    return this.classSymbol.IsStatic;
                case Modifiers.Class.Sealed:
                    return this.classSymbol.IsSealed;
                case Modifiers.Class.Abstract:
                    return this.classSymbol.IsAbstract;
                default: throw new NotImplementedException(string.Format("Modifier {0} is unknown", modifier));
            }
        }
    }
}