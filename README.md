# Roslinq (LINQ to Roslyn)
Roslinq is a Roslyn based component that enables queries over C# source code.

## Features
Roslinq currently supports:

* MSBuild based workspaces (_.sln_, _.csproj_)
* class queries
* method queries

## Examples

Private static methods taking `int` parameter input:

``` csharp
var codeQuery = new ProjectQuery(@"path\to\RoslinqTestTarget.csproj");
var result = codeQuery
    .Classes
    .Methods
    .WithModifier(MethodModifier.Static)
    .WithModifier(MethodModifier.Private)
    .WithParameterType<int>()
    .Execute();
```

HTTP POST handlers in MVC Controllers:

``` csharp
var codeQuery = new ProjectQuery(@"path\to\RoslinqTestTarget.csproj");
var postControllerActions = codeQuery
    .Classes.InheritingFrom<Controller>()
    .Methods.WithAttribute<HttpPostAttribute>()
    .Execute();
```
