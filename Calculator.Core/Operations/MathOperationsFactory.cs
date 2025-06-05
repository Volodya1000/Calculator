using Calculator.Core.Error;
using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public static class MathOperationsFactory
{
    public static Dictionary<string, IOperation> CreateOperations()
    {
        return new Dictionary<string, IOperation>(StringComparer.OrdinalIgnoreCase)
        {
            // Базовые арифметические операции
            ["+"] = new Operation("add", args => args[0] + args[1], 2),
            ["-"] = new Operation("subtract", args => args[0] - args[1], 2),
            ["*"] = new Operation("multiply", args => args[0] * args[1], 2),
            ["/"] = new Operation("divide", args =>
            {
                if (args[1] == 0)
                    throw new CalculatorException(
                        CustomError.CalculationError("divide", "Division by zero"));
                return args[0] / args[1];
            }, 2),

            // Математические функции
            ["fact"] = new Operation("fact", MathOperations.Factorial, 1),
            ["log"] = new Operation("log", MathOperations.Logarithm, 2),
            ["root"] = new Operation("root", MathOperations.Root, 2),
            ["sqrt"] = new Operation("sqrt", args =>
            {
                if (args[0] < 0)
                    throw new CalculatorException(
                        CustomError.InvalidArgument("sqrt", "Must be non-negative", 1));
                return Math.Sqrt(args[0]);
            }, 1),

            // Тригонометрические функции
            ["sin"] = new Operation("sin", args=> Math.Sin(args[0]), 1),
            ["cos"] = new Operation("cos", args => Math.Cos(args[0]), 1),
            ["tan"] = new Operation("tan", MathOperations.Tan, 1),
            ["atan2"] = new Operation("atan2", args => Math.Atan2(args[0], args[1]), 2),

            // Специальные операции
            ["pow"] = new Operation("pow", args => Math.Pow(args[0], args[1]), 2),
            ["abs"] = new Operation("abs", args => Math.Abs(args[0]), 1)
        };
    }
}

