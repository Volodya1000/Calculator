using Calculator.Core.ExpressionEvaluator.Tokinezation;

namespace Calculator.Core.Exceptions.ExpressionExceptions;

public sealed class UnexpectedEndException : ExpressionException
{
    public TokenType ExpectedType { get;}

    public UnexpectedEndException(TokenType expected)
        : base($"Unexpected end of input, expected {expected}") 
    {
        ExpectedType = expected;
    }
}
