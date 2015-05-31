namespace Roslinq.Core
{
    using Microsoft.CodeAnalysis;

    public class MethodQueryData
    {
        private IMethodSymbol methodSymbol;

        public MethodQueryData(IMethodSymbol methodSymbol)
        {
            this.methodSymbol = methodSymbol;
        }

        public string Name
        {
            get { return this.methodSymbol.Name; }
        }
    }
}