namespace Roslinq.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
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
            if (classBaseType.Name == type.Name)
            {
                return true;
            }

            if (classBaseType.Name == typeof(object).Name)
            {
                return false;
            }

            return InheritsFromInternal(classBaseType, type);
        }

        internal bool ImplementsInterface(string interfaceName)
        {
            if (this.classSymbol.Interfaces != null)
            {
                if (this.classSymbol.Interfaces.Any())
                {
                    if (this.classSymbol.Interfaces.FirstOrDefault(i => i.Name == interfaceName) != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}