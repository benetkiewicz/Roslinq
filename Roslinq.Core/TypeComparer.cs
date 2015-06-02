namespace Roslinq.Core
{
    using System;
    using Microsoft.CodeAnalysis;

    public class TypeComparer
    {
        public static bool TypesMatch(ITypeSymbol typeSymbol, Type type)
        {
            return typeSymbol.Name == type.Name;
        } 
    }
}