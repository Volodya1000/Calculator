namespace Calculator.Core.Exceptions.ExpressionExceptions;

public sealed class ExtraTokensException : ExpressionException
{
    public ExtraTokensException(int position)
        : base($"Extra tokens after end of expression", position, position) { }
}