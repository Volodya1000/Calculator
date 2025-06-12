namespace Calculator.Core.Exceptions.ExpressionExceptions;

public sealed class UnknownTokenException : ExpressionException
{
    public UnknownTokenException(string token, int start, int end)
        : base($"Invalid token: {token}", start, end) { }
}
