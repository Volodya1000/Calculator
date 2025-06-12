using Calculator.Core.Builders;
using Calculator.Core.Exceptions.OperationExceptions;
using Calculator.Core.Interfaces;
using Calculator.Core.Operations;
using Calculator.Core.ResultPattern;
using System.Reflection;
using System.Xml.Linq;

namespace Calculator.Core;

public class Calculator
{
    private readonly Dictionary<string, IOperation> _operations;

    public Calculator()
    {
        _operations = new Dictionary<string, IOperation>(StringComparer.OrdinalIgnoreCase);
    }



    public Result<double> Call(string operationName, params double[] args)
    {
        if (!_operations.TryGetValue(operationName, out var operation))
            return Result<double>.Failure(new OperationNotFoundException(operationName));

        try
        {
            return Result<double>.Success(operation.Call(args));
        }
        catch (Exception ex)
        {
            return Result<double>.Failure(ex);
        }
    }

    public bool OperationExists(string operation) => _operations.ContainsKey(operation);

    public IEnumerable<string> GetAvailableOperationsNames()
    {
        return _operations.Keys.OrderBy(k => k);
    }


    public Delegate this[string name]
    {
        set => _operations[name] = CreateOperation(name, value);
    }

    public Calculator AddAssembly(string assemblyPath)
    {
        var assembly = Assembly.LoadFrom(assemblyPath);
        foreach (var type in assembly.GetExportedTypes())
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (method.ReturnType != typeof(double)) continue;

                var parameters = method.GetParameters();

                // Поддержка метода с одним параметром double[]
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(double[]))
                {
                    var del = new Func<double[], double>((args) =>
                    {
                        return (double)method.Invoke(null, new object[] { args })!;
                    });

                    _operations[method.Name] = new Operation(method.Name, del, -1);
                    continue;
                }

                // Поддержка метода с несколькими параметрами double
                if (parameters.All(p => p.ParameterType == typeof(double)))
                {
                    var argCount = parameters.Length;
                    var del = CreateDelegate(method, argCount);
                    _operations[method.Name] = new Operation(method.Name, del, argCount);
                    continue;
                }
            }
        }
        return this;
    }

    private Func<double[], double> CreateDelegate(MethodInfo method, int argCount)
    {
        return args =>
        {
            if (args.Length != argCount)
                throw new InsufficientArgumentsException(method.Name, argCount, args.Length); ;

            return (double)method.Invoke(null, args.Select(a => (object)a).ToArray())!;
        };
    }
    private IOperation CreateOperation(string name, Delegate del)
    {
        var method = del.Method;
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(double))
            throw new ArgumentException($"Operation '{name}' must return double");

        // Поддержка операций с переменным количеством аргументов
        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(double[]))
        {
            var func = (Func<double[], double>)del;
            return new Operation(name, func, -1); 
        }

        // Поддержка операций с фиксированными аргументами
        if (parameters.All(p => p.ParameterType == typeof(double)))
        {
            int argCount = parameters.Length;
            return new Operation(name, args =>
            {
                if (args.Length != argCount)
                    throw new InsufficientArgumentsException(name, argCount, args.Length);

                return (double)del.DynamicInvoke(args.Cast<object>().ToArray())!;
            }, argCount);
        }

        throw new ArgumentException(
            $"Unsupported delegate type for operation '{name}'. " +
            "Parameters must be either all double or a single double[].");
    }
}