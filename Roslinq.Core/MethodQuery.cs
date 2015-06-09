namespace Roslinq.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MethodQuery
    {
        private IList<MethodQueryData> methods;

        internal MethodQuery(IList<MethodQueryData> methods)
        {
            this.methods = methods;
        }

        /// <summary>
        /// Executes the query with all previously applied filters.
        /// </summary>
        /// <returns>The list of <see cref="MethodQueryData"/></returns>
        public IList<MethodQueryData> Execute()
        {
            return this.methods;
        }

        /// <summary>
        /// Filter methods by type they return.
        /// </summary>
        /// <param name="type">The type of value that method returns.</param>
        public MethodQuery ReturningType(Type type)
        {
            Filter(x => x.ReturnsType(type));
            return this;
        }

        /// <summary>
        /// Filter methods by type they return.
        /// </summary>
        /// <typeparam name="T">The type of value that method returns.</typeparam>
        public MethodQuery ReturningType<T>()
        {
            return this.ReturningType(typeof(T));
        }

        /// <summary>
        /// Filter methods by parameter type they take.
        /// </summary>
        /// <param name="parameterType">The type of paramter that method takes.</param>
        public MethodQuery WithParameterType(Type parameterType)
        {
            this.Filter(x => x.HasParameterType(parameterType));
            return this;
        }

        /// <summary>
        /// Filter methods by parameter type they take.
        /// </summary>
        /// <typeparam name="T">The type of paramter that method takes.</typeparam>
        public MethodQuery WithParameterType<T>()
        {
            return this.WithParameterType(typeof(T));
        }

        /// <summary>
        /// Filter methods by modifier they have applied.
        /// </summary>
        /// <param name="modifier"><see cref="Modifiers.Method"/> access modifier.</param>
        /// <returns></returns>
        public MethodQuery WithModifier(int modifier)
        {
            this.Filter(x => x.HasModifier(modifier));
            return this;
        }

        private void Filter(Func<MethodQueryData, bool> predicate)
        {
            var result = this.methods.Where(predicate).ToList();
            this.methods = result;
        }
    }
}