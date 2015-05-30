namespace Roslinq.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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

        private bool InheritsFromInternal(INamedTypeSymbol queriedType, Type type)
        {
            var classBaseType = queriedType.BaseType;
            if (classBaseType.ToString() == type.ToString())
            {
                return true;
            }

            if (classBaseType.ToString() == "object")
            {
                return false;
            }

            return InheritsFromInternal(classBaseType, type);
        }
    }
}