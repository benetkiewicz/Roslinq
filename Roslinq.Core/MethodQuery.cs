namespace Roslinq.Core
{
    using System;
    using System.Collections.Generic;

    public class MethodQuery
    {
        private readonly ClassQuery parentClasses;
        private IList<MethodQueryData> methods;

        public MethodQuery(ClassQuery parentClasses)
        {
            this.parentClasses = parentClasses;
        }

        public MethodQuery ReturningType(Type type)
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

            var result  = new List<MethodQueryData>();
            foreach (var methodQueryData in this.methods)
            {
                if (methodQueryData.ReturnsType(type))
                {
                    result.Add(methodQueryData);
                }
            }

            this.methods = result;
            return this;
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

        public MethodQuery WithParameterType(Type parameterType)
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

            var result = new List<MethodQueryData>();
            foreach (var methodQueryData in this.methods)
            {
                if (methodQueryData.HasParameterType(parameterType))
                {
                    result.Add(methodQueryData);
                }
            }

            this.methods = result;
            return this;
        }
    }
}