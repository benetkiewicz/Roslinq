namespace Roslinq.Core
{
    using System;
    using Microsoft.CodeAnalysis;

    public class TypeComparer
    {
        public static bool TypesMatch(ITypeSymbol typeSymbol, Type type)
        {
            if (typeSymbol.ContainingNamespace != null)
            {
                return typeSymbol.ContainingNamespace.ToString() == type.Namespace && typeSymbol.Name == type.Name;
            }

            return typeSymbol.Name == type.Name;
        } 
    }
}