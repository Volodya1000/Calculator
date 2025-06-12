
namespace Calculator.Core.Exceptions.ExpressionExceptions;

public sealed class UnexpectedTokenException : ExpressionException
{

    public UnexpectedTokenException(string token,string message, int start, int end)
        : base($"Unexpected token: '{token}' {message}", start, end) {}
}
