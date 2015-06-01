namespace Roslinq.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MethodQuery
    {
        private readonly ClassQuery parentClasses;
        private IList<MethodQueryData> methods;

        public MethodQuery(ClassQuery parentClasses)
        {
            this.parentClasses = parentClasses;
        }

        public IList<MethodQueryData> Execute()
        {
            EnsureMethodsExist();
            return this.methods;
        }

        public MethodQuery ReturningType(Type type)
        {
            EnsureMethodsExist();
            Filter(x => x.ReturnsType(type));
            return this;
        }

        public MethodQuery WithParameterType(Type parameterType)
        {
            EnsureMethodsExist();
            this.Filter(x => x.HasParameterType(parameterType));
            return this;
        }

        public MethodQuery WithModifier(int modifier)
        {
            EnsureMethodsExist();
            this.Filter(x => x.HasModifier(modifier));
            return this;
        }

        private void Filter(Func<MethodQueryData, bool> predicate)
        {
            var result = this.methods.Where(predicate).ToList();
            this.methods = result;
        }

        private void EnsureMethodsExist()
        {
            if (this.methods == null)
            {
                this.methods = GetMethods().ToList();
            }
        }

        private IEnumerable<MethodQueryData> GetMethods()
        {
            foreach (var classQueryData in parentClasses.Execute())
            {
                foreach (var methodSymbol in classQueryData.Methods)
                {
                    yield return new MethodQueryData(methodSymbol);
                }
            }
        }
    }
}