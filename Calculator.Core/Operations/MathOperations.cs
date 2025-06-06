using Calculator.Core.Error;

namespace Calculator.Core.Operations;

public static class MathOperations
{
    public static double Factorial(double n)
    {
        if (n < 0 || n % 1 != 0)
            throw  new InvalidCalculatorArgumentException("Requires non-negative integer", n,1);

        if (n > 170)
            new InvalidCalculatorArgumentException("Value too large for double factorial", n,1);

        double result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;

        return result;
    }

    public static double Logarithm(double number, double baseValue)
    {
        if (number <= 0)
            throw new InvalidCalculatorArgumentException("Argument must be positive", number, 1);

        if (baseValue <= 0 || baseValue == 1)
            throw new InvalidCalculatorArgumentException("Base must be positive and !=1", baseValue, 2);

        return Math.Log(number, baseValue);
    }

    public static double Root(double number, double degree)
    {
        if (degree == 0)
            throw new InvalidCalculatorArgumentException("Degree cannot be zero",  degree,1);

        if (number < 0 && degree % 2 == 0)
            throw new InvalidCalculatorArgumentException("Even root of negative is undefined", number,2);

        return number < 0
            ? -Math.Pow(-number, 1 / degree)
            : Math.Pow(number, 1 / degree);
    }

    public static double Tan(double angle)
    {
        double cos = Math.Cos(angle);

        if (Math.Abs(cos) < double.Epsilon)
            throw new InvalidCalculatorArgumentException("Undefined for this angle", angle,1);

        return Math.Tan(angle);
    }
}