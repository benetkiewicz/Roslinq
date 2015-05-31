namespace Roslinq.Core
{
    using System;
    using Microsoft.CodeAnalysis;

    public class InheritanceHelper
    {
        /// <summary>
        /// Check if a given type directly or indirectly iherits from other type.
        /// </summary>
        /// <param name="queriedType">Type which base type is being checked</param>
        /// <param name="type">Type which is potentially a base type</param>
        /// <returns>True if queried type inherits from type, false otherwise.</returns>
        public static bool InheritsFrom(Type queriedType, Type type)
        {
            if (queriedType.BaseType == null)
            {
                return false;
            }

            if (queriedType.BaseType.Name == type.Name)
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
        public static bool InheritsFrom(INamedTypeSymbol queriedType, Type type)
        {
            var classBaseType = queriedType.BaseType;
            if (classBaseType == null)
            {
                return false;
            }

            if (classBaseType.Name == type.Name)
            {
                return true;
            }

            if (classBaseType.Name == typeof(object).Name)
            {
                return false;
            }

            return InheritsFrom(classBaseType, type);
        }
    }
}