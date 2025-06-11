namespace Calculator.Core.Exceptions.OperationExceptions;

public abstract class OperationException : Exception
{
    public string Operation { get; protected set; }

    public OperationException(string operation, string message) : base(message)
    {
        Operation = operation;
    }


    public OperationException(string operation, string message, Exception inner)
        : base(message, inner)
    {
        Operation = operation;
    }
}

