namespace Roslinq
{
    using System;
    using Microsoft.CodeAnalysis;

    internal class InheritanceHelper
    {
        /// <summary>
        /// Check if a given type directly or indirectly iherits from other type.
        /// </summary>
        /// <param name="queriedType">Type which base type is being checked</param>
        /// <param name="type">Type which is potentially a base type</param>
        /// <returns>True if queried type inherits from type, false otherwise.</returns>
        internal static bool InheritsFrom(Type queriedType, Type type)
        {
            if (queriedType.BaseType == null)
            {
                return false;
            }

            if (queriedType.BaseType == type)
            {
                return true;
            }

            if (queriedType.BaseType.Name == typeof(object).Name)
            {
                return false;
            }

            return InheritsFrom(queriedType.BaseType, type);
        }

        /// <summary>
        /// Check if a given type directly or indirectly iherits from other type.
        /// </summary>
        /// <param name="queriedType">Type which base type is being checked</param>
        /// <param name="type">Type which is potentially a base type</param>
        /// <returns>True if queried type inherits from type, false otherwise.</returns>
        internal static bool InheritsFrom(INamedTypeSymbol queriedType, Type type)
        {
            var classBaseType = queriedType.BaseType;
            if (classBaseType == null)
            {
                return false;
            }

            if (TypeComparer.TypesMatch(classBaseType, type))
            {
                return true;
            }

            if (TypeComparer.TypesMatch(classBaseType, typeof(object)))
            {
                return false;
            }

            return InheritsFrom(classBaseType, type);
        }
    }
}