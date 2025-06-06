using Calculator.Core.Error;

namespace Calculator.Core.Operations;
public static class MathOperations
{
    public static double Factorial(double[] args)
    {
        double n = args[0];

        if (n < 0 || n % 1 != 0)
            throw  CalculatorException.InvalidArgument("fact", "Requires non-negative integer");

        if (n > 170)
            throw CalculatorException.CalculationError("fact", "Value too large for double factorial");

        double result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;

        return result;
    }

    public static double Logarithm(double[] args)
    {
        double number = args[0];
        double baseValue = args[1];

        if (number <= 0)
            throw CalculatorException.InvalidArgument("log", "Argument must be positive", 1);

        if (baseValue <= 0 || baseValue == 1)
            throw CalculatorException.InvalidArgument("log", "Base must be positive and !=1", 2);

        return Math.Log(number, baseValue);
    }

    public static double Root(double[] args)
    {
        double number = args[0];
        double degree = args[1];

        if (degree == 0)
            throw CalculatorException.InvalidArgument("root", "Degree cannot be zero", 2);

        if (number < 0 && degree % 2 == 0)
            throw CalculatorException.InvalidArgument("root", "Even root of negative is undefined");

        return number < 0
            ? -Math.Pow(-number, 1 / degree)
            : Math.Pow(number, 1 / degree);
    }

    public static double Tan(double[] args)
    {
        double angle = args[0];
        double cos = Math.Cos(angle);

        if (Math.Abs(cos) < double.Epsilon)
            throw CalculatorException.CalculationError("tan", "Undefined for this angle");

        return Math.Tan(angle);
    }
}