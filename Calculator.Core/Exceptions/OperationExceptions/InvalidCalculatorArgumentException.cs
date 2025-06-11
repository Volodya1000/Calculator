namespace Calculator.Core.Exceptions.OperationExceptions;

public class InvalidCalculatorArgumentException:OperationException
{
    public double Arg { get; }

    public int ArgIndex { get; }

    public InvalidCalculatorArgumentException(string operation, string problem, double arg, int argIndex) : base(operation, $"{operation}: {problem} (argument {arg} on position: {argIndex})")
    {
        Operation = operation;
        Arg = arg;
        ArgIndex = argIndex;
    }
    public InvalidCalculatorArgumentException(string problem, double arg, int argIndex) : base("", problem)
    {
        Operation = "";
        Arg = arg;
        ArgIndex = argIndex;
    }
}
