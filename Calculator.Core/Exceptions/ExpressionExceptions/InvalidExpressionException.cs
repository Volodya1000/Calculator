namespace Calculator.Core.Exceptions.ExpressionExceptions;

public sealed class InvalidExpressionException : ExpressionException
{
    public InvalidExpressionException()
        : base("Expression is invalid") { }
}
