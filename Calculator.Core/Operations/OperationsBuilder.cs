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

    public OperationsBuilder AddArithmeticOperations()
    {
        return this
            .AddOperation("+", args => args.Sum() , 2,true)
            .AddOperation("-", args => args[0] + args.Skip(1).Sum(arg => arg * (-1)) , 2,true)
            .AddOperation("*", args => args.Aggregate(1.0,(acc,arg)=>acc*arg), 2,true)
            .AddOperation("/", args =>
            {
                if (args[1] == 0)
                    throw new InvalidCalculatorArgumentException("Division by zero",0,1);
                return args[0] / args[1];
            }, 2);
    }

    public OperationsBuilder AddMathFunctions()
    {
        return this
            .AddOperation("fact", MathOperations.Factorial, 1)
            .AddOperation("log", MathOperations.Logarithm, 2)
            .AddOperation("root", MathOperations.Root, 2)
            .AddOperation("sqrt", args =>
            {
                if (args[0] < 0)
                    throw new InvalidCalculatorArgumentException("", "Must be non-negative", args[0], 1);
                return Math.Sqrt(args[0]);
            }, 1);
    }

    public OperationsBuilder AddTrigonometricFunctions()
    {
        return this
            .AddOperation("sin", args => Math.Sin(args[0]), 1)
            .AddOperation("cos", args => Math.Cos(args[0]), 1)
            .AddOperation("tan", MathOperations.Tan, 1)
            .AddOperation("atan2", args => Math.Atan2(args[0], args[1]), 2);
    }

    public OperationsBuilder AddSpecialFunctions()
    {
        return this
           .AddOperation("pow", args => Math.Pow(args[0], args[1]), 2)
           .AddOperation("abs", args => Math.Abs(args[0]), 1);
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

