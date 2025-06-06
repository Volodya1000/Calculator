using Calculator.Core.Error;
using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class OperationsBuilder
{
    private readonly Dictionary<string, IOperation> _operations = new (StringComparer.OrdinalIgnoreCase);

    public OperationsBuilder AddOperation(string key, string name,Func<double[],double> func, int argCount)
    {
        _operations.Add(key, new Operation(name,func, argCount));
        return this;
    }

    public OperationsBuilder AddArithmeticOperations()
    {
        return this
            .AddOperation("+", "add", args => args[0] + args[1], 2)
            .AddOperation("-", "subtract", args => args[0] - args[1], 2)
            .AddOperation("*", "multiply", args => args[0] * args[1], 2)
            .AddOperation("/", "divide", args =>
            {
                if (args[1] == 0)
                    throw CalculatorException.CalculationError("divide", "Division by zero");
                return args[0] / args[1];
            }, 2);
    }

    public OperationsBuilder AddMathFunctions()
    {
        return this
            .AddOperation("fact","fact", MathOperations.Factorial, 1)
            .AddOperation("log", "log", MathOperations.Logarithm, 2)
            .AddOperation("root", "root", MathOperations.Root, 2)
            .AddOperation("sqrt", "sqrt", args =>
            {
                if (args[0] < 0)
                    throw CalculatorException.InvalidArgument("sqrt", "Must be non-negative", 1);
                return Math.Sqrt(args[0]);
            }, 1);
    }

    public OperationsBuilder AddTrigonometricFunctions()
    {
        return this
            .AddOperation("sin", "sin", args => Math.Sin(args[0]), 1)
            .AddOperation("cos", "cos", args => Math.Cos(args[0]), 1)
            .AddOperation("tan", "tan", MathOperations.Tan, 1)
            .AddOperation("atan2", "atan2", args => Math.Atan2(args[0], args[1]), 2);
    }

    public OperationsBuilder AddSpecialFunctions()
    {
        return this
           .AddOperation("pow","pow", args => Math.Pow(args[0], args[1]), 2)
           .AddOperation("abs","abs", args => Math.Abs(args[0]), 1);
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

