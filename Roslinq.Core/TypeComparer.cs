namespace Roslinq.Core
{
    using System;
    using Microsoft.CodeAnalysis;

    internal class TypeComparer
    {
        internal static bool TypesMatch(ITypeSymbol typeSymbol, Type type)
        {
            if (typeSymbol.ContainingNamespace != null)
            {
                return typeSymbol.ContainingNamespace.ToString() == type.Namespace && typeSymbol.Name == type.Name;
            }

            return typeSymbol.Name == type.Name;
        } 
    }
}