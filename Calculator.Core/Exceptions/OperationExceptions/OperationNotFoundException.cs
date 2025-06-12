namespace Calculator.Core.Exceptions.OperationExceptions;

public sealed class OperationNotFoundException : OperationException
{
    public OperationNotFoundException(string operation) : base(operation,$"Operation '{operation}' not found")
    {
        Operation= operation;
    }
}
