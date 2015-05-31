﻿namespace Roslinq.Core
{
    using System;
    using System.Collections;
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
            return this.InheritsFromInternal(this.classSymbol, type);
        }

        internal IEnumerable<IMethodSymbol> Methods
        {
            get
            {
                var x = classSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Method);
                foreach (var source in x.ToList())
                {
                    yield return (IMethodSymbol)source;
                }
            }
        }

        private bool InheritsFromInternal(INamedTypeSymbol queriedType, Type type)
        {
            var classBaseType = queriedType.BaseType;
            if (classBaseType.Name == type.Name)
            {
                return true;
            }

            if (classBaseType.Name == typeof(object).Name)
            {
                return false;
            }

            return InheritsFromInternal(classBaseType, type);
        }

        internal bool ImplementsInterface(string interfaceName)
        {
            if (this.classSymbol.Interfaces != null)
            {
                if (this.classSymbol.Interfaces.Any())
                {
                    if (this.classSymbol.Interfaces.FirstOrDefault(i => i.Name == interfaceName) != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal bool HasAttributeApplied(Type type)
        {
            var classAppliedAttributes = this.classSymbol.GetAttributes();
            foreach (var methodAppliedAttribute in classAppliedAttributes)
            {
                if (methodAppliedAttribute.AttributeClass.Name == type.Name)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasModifier(int modifier)
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