namespace Roslinq.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MethodQuery
    {
        private readonly ClassQuery parentClasses;
        private IList<MethodQueryData> methods;

        internal MethodQuery(ClassQuery parentClasses)
        {
            this.parentClasses = parentClasses;
        }

        /// <summary>
        /// Executes the query with all previously applied filters.
        /// </summary>
        /// <returns>The list of <see cref="MethodQueryData"/></returns>
        public IList<MethodQueryData> Execute()
        {
            EnsureMethodsExist();
            return this.methods;
        }

        /// <summary>
        /// Filter methods by type they return.
        /// </summary>
        /// <param name="type">The type of value that method returns.</param>
        public MethodQuery ReturningType(Type type)
        {
            EnsureMethodsExist();
            Filter(x => x.ReturnsType(type));
            return this;
        }

        /// <summary>
        /// Filter methods by parameter type they take.
        /// </summary>
        /// <param name="parameterType">The type of paramter that method takes.</param>
        public MethodQuery WithParameterType(Type parameterType)
        {
            EnsureMethodsExist();
            this.Filter(x => x.HasParameterType(parameterType));
            return this;
        }
        
        /// <summary>
        /// Filter methods by modifier they have applied.
        /// </summary>
        /// <param name="modifier"><see cref="Modifiers.Method"/> access modifier.</param>
        /// <returns></returns>
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