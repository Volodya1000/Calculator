namespace Calculator.Core.ExpressionEvaluator.Tokinezation;

public record Token(TokenType Type, string Value, int Start, int End);
