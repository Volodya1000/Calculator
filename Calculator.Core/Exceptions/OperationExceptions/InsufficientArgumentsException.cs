namespace Calculator.Core.Exceptions.OperationExceptions;

public sealed class InsufficientArgumentsException: OperationException 
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
