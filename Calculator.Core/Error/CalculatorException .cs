namespace Calculator.Core.Error;

public abstract class CalculatorException : Exception
{
    public string Operation { get; protected set; }

    public CalculatorException(string operation, string message) : base(message)
    {
        Operation = operation;
    }


    public CalculatorException(string operation, string message, Exception inner)
        : base(message, inner)
    {
        Operation = operation;
    }
}

