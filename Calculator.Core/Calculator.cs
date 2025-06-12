using Calculator.Core.Exceptions.OperationExceptions;
using Calculator.Core.Interfaces;
using Calculator.Core.Operations;
using Calculator.Core.ResultPattern;
using System.Reflection;

namespace Calculator.Core;

public class Calculator
{
    private readonly Dictionary<string, IOperation> _operations
        = new(StringComparer.OrdinalIgnoreCase);

    public Result<double> Call(string operationName, params double[] args)
    {
        if (!_operations.TryGetValue(operationName, out var op))
            return Result<double>.Failure(new OperationNotFoundException(operationName));

        try 
        { 
            return Result<double>.Success(op.Call(args)); 
        }
        catch (Exception ex) 
        {
            return Result<double>.Failure(ex); 
        }
    }

    public bool OperationExists(string name) => _operations.ContainsKey(name);

    public IEnumerable<string> GetAvailableOperationsNames() => _operations.Keys.OrderBy(k => k);

    public Delegate this[string name]
    {
        set => _operations[name] = CreateOperationFromDelegate(name, value);
    }

    public Calculator AddAssembly(string assemblyPath)
    {
        var asm = Assembly.LoadFrom(assemblyPath);
        foreach (var type in asm.GetExportedTypes())
            foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (m.ReturnType != typeof(double))
                    continue;

                Delegate del;
                var prms = m.GetParameters();

                // один параметр double[]
                if (prms.Length == 1 && prms[0].ParameterType == typeof(double[]))
                {
                    del = (Func<double[], double>)(arr =>
                        (double)m.Invoke(null, new object[] { arr })!);
                }
                // несколько отдельных double
                else if (prms.All(p => p.ParameterType == typeof(double)))
                {
                    del = CreateFixedArgDelegate(m);
                }
                else
                    continue;

                _operations[m.Name] = CreateOperationFromDelegate(m.Name, del);
            }

        return this;
    }

    private Func<double[], double> CreateFixedArgDelegate(MethodInfo m)
    {
        var count = m.GetParameters().Length;
        return args =>
        {
            if (args.Length != count)
                throw new InsufficientArgumentsException(m.Name, count, args.Length);
            return (double)m.Invoke(null, args.Cast<object>().ToArray())!;
        };
    }

    private IOperation CreateOperationFromDelegate(string name, Delegate del)
    {
        var m = del.Method;
        var prms = m.GetParameters();

        if (m.ReturnType != typeof(double))
            throw new ArgumentException($"Operation '{name}' must return double");

        // varargs: Func<double[], double>
        if (prms.Length == 1 && prms[0].ParameterType == typeof(double[]))
        {
            var func = (Func<double[], double>)del;
            return new Operation(name, func, -1);
        }

        // фиксированное число аргументов
        if (prms.All(p => p.ParameterType == typeof(double)))
        {
            int expected = prms.Length;
            return new Operation(
                name,
                args =>
                {
                    if (args.Length != expected)
                        throw new InsufficientArgumentsException(name, expected, args.Length);
                    return (double)del.DynamicInvoke(args.Cast<object>().ToArray())!;
                },
                expected);
        }

        throw new ArgumentException(
            $"Unsupported delegate type for '{name}'. Must be Func<double[],double> or Func<double,…,double>.");
    }
}