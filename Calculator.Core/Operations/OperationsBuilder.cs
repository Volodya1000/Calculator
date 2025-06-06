using Calculator.Core.Error;
using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

//пататерн строитель позволяет гибко добавлять только нужные операции. Например инжнерный либо упрощённый калькулятор
public class OperationsBuilder
{
    private readonly Dictionary<string, IOperation> _operations = new (StringComparer.OrdinalIgnoreCase);

    public OperationsBuilder AddOperation(string key,Func<double[],double> func, int argCount,bool canHaveMoreArgsThanRequired=false)
    {
        _operations.TryAdd(key, new Operation(key, func, argCount, canHaveMoreArgsThanRequired));
        return this;
    }

    public OperationsBuilder AddOperation(string key, Func<double, double, double> func, bool canHaveMoreArgsThanRequired = false)
        => AddOperation(key, args => func(args[0], args[1]), 2, canHaveMoreArgsThanRequired);


    public OperationsBuilder AddOperation(string key, Func<double, double> func)
         => AddOperation(key, args => func(args[0]), 1);

    public OperationsBuilder AddOperation(string key, Func<double, double, double> func)
        =>  AddOperation(key, args => {return func(args[0], args[1]);},2);

    public OperationsBuilder AddArithmeticOperations()
    {
        return this
            .AddOperation("+", args => args.Sum(),2,true)
            .AddOperation("-", args => args[0] + args.Skip(1).Sum(arg => arg * (-1)) , 2,true)
            .AddOperation("*", args => args.Aggregate(1.0,(acc,arg)=>acc*arg), 2,true)
            .AddOperation("/", (a,b) =>
            {
                if (b == 0)
                    throw new InvalidCalculatorArgumentException("Division by zero",0,1);
                return a / b;
            });
    }

    public OperationsBuilder AddMathFunctions()
    {
        return this
            .AddOperation("fact", MathOperations.Factorial)
            .AddOperation("log", MathOperations.Logarithm)
            .AddOperation("root", MathOperations.Root)
            .AddOperation("sqrt", arg =>
            {
                if (arg < 0)
                    throw new InvalidCalculatorArgumentException("Must be non-negative", arg, 1);
                return Math.Sqrt(arg);
            });
    }

    public OperationsBuilder AddTrigonometricFunctions()
    {
        return this
            .AddOperation("sin", Math.Sin)
            .AddOperation("cos", Math.Cos)
            .AddOperation("tan", MathOperations.Tan)
            .AddOperation("atan2", Math.Atan2);
    }

    public OperationsBuilder AddSpecialFunctions()
    {
        return this
           .AddOperation("pow", Math.Pow)
           .AddOperation("abs", Math.Abs);
    }

    public OperationsBuilder AddAll()
    {
        return AddArithmeticOperations()
            .AddMathFunctions()
            .AddTrigonometricFunctions()
            .AddSpecialFunctions();
    }

    public Dictionary<string, IOperation> Build()
    {
        return new Dictionary<string, IOperation>(_operations, StringComparer.OrdinalIgnoreCase);
    }
}

