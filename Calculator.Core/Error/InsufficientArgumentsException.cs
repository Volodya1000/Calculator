namespace Calculator.Core.Error;

public class InsufficientArgumentsException: CalculatorException 
{
    public int Expected {  get; }

    public int Actual { get; }

    public InsufficientArgumentsException(string operation, int expected, int actual) : base(operation,$"Operation '{operation}' requires {expected} arguments, provided {actual}")
    {
        Operation = operation;
        Expected = expected;
        Actual = actual;
    }
}
