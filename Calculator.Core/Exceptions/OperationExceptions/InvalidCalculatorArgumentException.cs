namespace Calculator.Core.Exceptions.OperationExceptions;

public sealed class InvalidCalculatorArgumentException:OperationException
{
    public double Arg { get; }

    public int ArgIndex { get; }

    public InvalidCalculatorArgumentException(string operation, string problem, double arg, int argIndex) : base(operation,problem)
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
