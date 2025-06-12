namespace Calculator.Core.Exceptions.ExpressionExceptions;

public  abstract class ExpressionException : Exception
{
    public int Start { get; }
    public int End { get; }

    protected ExpressionException(string message, int start, int end)
        : base($"{message}")
    {
        Start = start;
        End = end;
    }

    protected ExpressionException(string message)
        : base(message) { }
}
