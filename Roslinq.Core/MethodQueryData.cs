namespace Roslinq.Core
{
    using System;
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
    }
}