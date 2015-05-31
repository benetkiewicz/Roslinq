namespace Roslinq.Core
{
    using System.Collections.Generic;

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
            if (this.methods == null)
            {
                this.methods = new List<MethodQueryData>();
                foreach (var classQueryData in parentClasses.Execute())
                {
                    foreach (var methodSymbol in classQueryData.Methods)
                    {
                        var methodQueryData = new MethodQueryData(methodSymbol);
                        this.methods.Add(methodQueryData);
                    }
                    
                }
            }

            return this.methods;
        }
    }
}