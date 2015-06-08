namespace Roslinq.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ClassQuery
    {
        private IList<ClassQueryData> classes;

        internal ClassQuery(IList<ClassQueryData> classes)
        {
            this.classes = classes;
        }

        /// <summary>
        /// Entry point for creating and executing method queries.
        /// </summary>
        public MethodQuery Methods
        {
            get
            {
                return new MethodQuery(this);
            }
        }

        /// <summary>
        /// Executes the query with all previously applied filters.
        /// </summary>
        /// <returns>The list of <see cref="ClassQueryData"/></returns>
        public IList<ClassQueryData> Execute()
        {
            return this.classes;
        }

        /// <summary>
        /// Filter classes by type they inherit from (directly or indirectly).
        /// </summary>
        /// <param name="type">The type that class inherits from.</param>
        public ClassQuery InheritingFrom(Type type)
        {
            this.Filter(x => x.InheritsFrom(type));
            return this;
        }

        /// <summary>
        /// Filter classes by type they inherit from (directly or indirectly).
        /// </summary>
        /// <typeparam name="T">The type that class inherits from.</typeparam>
        public ClassQuery InheritingFrom<T>()
        {
            return this.InheritingFrom(typeof(T));
        }

        /// <summary>
        /// Filter classes by interface they implement.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <exception cref="ArgumentException">If the type is not interface type.</exception>
        public ClassQuery ImplementingInterface(Type interfaceType)
        {
            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException("Only interface types allowed", "interfaceType");
            }

            this.Filter(x => x.ImplementsInterface(interfaceType));
            return this;
        }

        /// <summary>
        /// Filter classes by interface they implement.
        /// </summary>
        /// <typeparam name="T">Type of the interface.</typeparam>
        /// <exception cref="ArgumentException">If the type is not interface type.</exception>
        public ClassQuery ImplementingInterface<T>()
        {
            return this.ImplementingInterface(typeof(T));
        }

        /// <summary>
        /// Filter classes by attribute thay have applied.
        /// </summary>
        /// <param name="type">Attribute type.</param>
        /// <exception cref="ArgumentException">If the type is not attribute type.</exception>
        public ClassQuery WithAttribute(Type type)
        {
            if (!InheritanceHelper.InheritsFrom(type, typeof(Attribute)))
            {
                throw new ArgumentException("Only attribute types allowed", "type");
            }

            this.Filter(x => x.HasAttributeApplied(type));
            return this;
        }

        /// <summary>
        /// Filter classes by attribute thay have applied.
        /// </summary>
        /// <typeparam name="T">Attribute type.</typeparam>
        /// <exception cref="ArgumentException">If the type is not attribute type.</exception>
        public ClassQuery WithAttribute<T>()
        {
            return this.WithAttribute(typeof(T));
        }

        /// <summary>
        /// Filter classes by modifier they have applied.
        /// </summary>
        /// <param name="modifier"><see cref="Modifiers.Class"/> access modifier.</param>
        public ClassQuery WithModifier(int modifier)
        {
            this.Filter(x => x.HasModifier(modifier));
            return this;
        }

        private void Filter(Func<ClassQueryData, bool> predicate)
        {
            var result = this.classes.Where(predicate).ToList();
            this.classes = result;
        }
    }
}