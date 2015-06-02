namespace Roslinq.Core
{
    using System;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    public class MethodQueryData
    {
        private readonly IMethodSymbol methodSymbol;

        public MethodQueryData(IMethodSymbol methodSymbol)
        {
            this.methodSymbol = methodSymbol;
        }

        public string MethodName
        {
            get { return this.methodSymbol.Name; }
        }

        internal bool ReturnsType(Type type)
        {
            return TypeComparer.TypesMatch(this.methodSymbol.ReturnType, type);
        }

        internal bool HasAttributeApplied(Type type)
        {
            var methodAppliedAttributes = this.methodSymbol.GetAttributes();
            foreach (var methodAppliedAttribute in methodAppliedAttributes)
            {
                if (TypeComparer.TypesMatch(methodAppliedAttribute.AttributeClass, type))
                {
                    return true;
                }
            }

            return false;
        }

        internal bool HasParameterType(Type parameterType)
        {
            if (!this.methodSymbol.Parameters.Any())
            {
                return false;
            }

            foreach (var parameter in this.methodSymbol.Parameters)
            {
                if (TypeComparer.TypesMatch(parameter.Type, parameterType))
                {
                    return true;
                }

                if (parameter.IsParams)
                {
                    var arrayType = (IArrayTypeSymbol)parameter.Type;
                    if (TypeComparer.TypesMatch(arrayType.ElementType, parameterType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal bool HasModifier(int modifier)
        {
            switch (modifier)
            {
                case Modifiers.Method.Private:
                    return this.methodSymbol.DeclaredAccessibility == Accessibility.Private;
                case Modifiers.Method.Protected:
                    return this.methodSymbol.DeclaredAccessibility == Accessibility.Protected;
                case Modifiers.Method.Public:
                    return this.methodSymbol.DeclaredAccessibility == Accessibility.Public;
                case Modifiers.Method.Internal:
                    return this.methodSymbol.DeclaredAccessibility == Accessibility.Internal;
                case Modifiers.Method.Static:
                    return this.methodSymbol.IsStatic;
                case Modifiers.Method.Virtual:
                    return this.methodSymbol.IsVirtual;
                case Modifiers.Method.Abstract:
                    return this.methodSymbol.IsAbstract;
                default: throw new NotImplementedException(string.Format("Modifier {0} is unknown", modifier));
            }
        }
    }
}