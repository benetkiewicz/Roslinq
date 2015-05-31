namespace Roslinq.Core
{
    using System;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    public class MethodQueryData
    {
        private IMethodSymbol methodSymbol;

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
            return this.methodSymbol.ReturnType.Name == type.Name;
        }

        public bool HasAttributeApplied(Type type)
        {
            var methodAppliedAttributes = this.methodSymbol.GetAttributes();
            foreach (var methodAppliedAttribute in methodAppliedAttributes)
            {
                if (methodAppliedAttribute.AttributeClass.Name == type.Name)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasParameterType(Type parameterType)
        {
            if (!this.methodSymbol.Parameters.Any())
            {
                return false;
            }

            foreach (var parameter in this.methodSymbol.Parameters)
            {
                if (parameter.Type.Name == parameterType.Name)
                {
                    return true;
                }

                if (parameter.IsParams)
                {
                    var arrayType = (IArrayTypeSymbol)parameter.Type;
                    if (arrayType.ElementType.Name == parameterType.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}