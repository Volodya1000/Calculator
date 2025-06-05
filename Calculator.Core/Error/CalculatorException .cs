namespace Calculator.Core.Error;

public class CalculatorException : Exception
{
    public CustomError Error { get; }

    public CalculatorException(CustomError error) : base(error.Message)
    {
        Error = error;
    }

    public CalculatorException(CustomError error, Exception inner)
        : base(error.Message, inner)
    {
        Error = error;
    }
}

