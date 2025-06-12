using Calculator.Core.Attributes;
using Calculator.Core.Exceptions.OperationExceptions;
using Calculator.Core.Interfaces;
using Calculator.Core.Operations;
using Calculator.Core.ResultPattern;
using System.Reflection;

namespace Calculator.Core;

public class Calculator
{
    public Calculator()
    {
        this["+"] = (double a, double b) => a+b;
        this["-"] = (double a, double b) => a-b;
        this["*"] = (double a, double b) => a*b;
        this["/"] = (double a, double b) =>
        {
            if (b == 0)
                throw new InvalidCalculatorArgumentException("Division by zero", 0, 1);
            return a / b;
        };
        this["!"] = MathOperations.Factorial;
        this["%"] = (double arg) => arg / 100;
    }

    public Dictionary<string, IOperation> Operations { get; private set; }
        = new(StringComparer.OrdinalIgnoreCase); 

    public Result<double> Call(string operationName, params double[] args)
    {
        if (!Operations.TryGetValue(operationName, out var op))
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

    public bool OperationExists(string name) => Operations.ContainsKey(name);

    public IEnumerable<string> GetAvailableOperationsNames() => Operations.Keys.OrderBy(k => k);

    public Delegate this[string name]
    {
        set => Operations[name] = CreateOperationFromDelegate(name, value);
    }

    public Calculator AddAssembly(string assemblyPath)
    {
        var asm = Assembly.LoadFrom(assemblyPath);
        foreach (var type in asm.GetExportedTypes())
            foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = m.GetCustomAttribute<OperationAttribute>();
                if (attr == null) continue;

                if (m.ReturnType != typeof(double)) continue;

                string opName = attr.Name ?? m.Name;
                var prms = m.GetParameters();

                // Обработка операций с переменным числом аргументов
                if (prms.Length == 1 && prms[0].ParameterType == typeof(double[]))
                {
                    var func = (Func<double[], double>)(arr =>
                        (double)m.Invoke(null, new object[] { arr })!);
                    Operations[opName] = new Operation(opName, func, -1);
                }
                // Обработка операций с фиксированными аргументами
                else if (prms.All(p => p.ParameterType == typeof(double)))
                {
                    int arity = prms.Length;
                    Operations[opName] = new Operation(
                        opName,
                        args =>
                        {
                            if (args.Length != arity)
                                throw new InsufficientArgumentsException(opName, arity, args.Length);
                            return (double)m.Invoke(null, args.Cast<object>().ToArray())!;
                        },
                        arity); 
                }
            }
        return this;
    }

    private IOperation CreateOperationFromDelegate(string name, Delegate del)
    {
        var m = del.Method;
        var prms = m.GetParameters();

        if (m.ReturnType != typeof(double))
            throw new ArgumentException($"Operation '{name}' must return double");

        // Фиксированное число аргументов
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

        if (prms.Length == 1 && prms[0].ParameterType == typeof(double[]))
        {
            var func = (Func<double[], double>)del;
            return new Operation(name, func, -1);
        }

        throw new ArgumentException(
            $"Unsupported delegate type for '{name}'. Must be Func<double[],double> or Func<double,…,double>.");
    }
}