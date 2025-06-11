using Calculator.Core.ExpressionEvaluator.Tokinezation;
using Calculator.Core.Interfaces;

namespace Calculator.Core.ExpressionEvaluator;

public class ExpressionEvaluator
{
    private readonly Dictionary<string, IOperation> _operations;
    private readonly Dictionary<string, double> _constants;
    public ExpressionEvaluator(Dictionary<string, IOperation> operations,Dictionary<string,double> constants)
    {
        _operations= operations;
        _constants= constants;
    }

    public double Evaluate(IEnumerable<Token> rpn)
    {
        Stack<double> rpnStack = new Stack<double>();

        foreach (Token token in rpn)
        {
            switch (token.Type)
            {
                case TokenType.Number:
                    rpnStack.Push(double.Parse(token.Value));
                    break;
                case TokenType.Constant:
                    EvaluateConstant(token,rpnStack);
                    break;
                case TokenType.Operator:
                case TokenType.Function:
                    EvaluateOperation(token, rpnStack);
                    break;
                default:
                    throw new Exception($"got invalid token \"{token.Value}\" ({token.Start}:{token.End})");
            }
        }
        return 0;
    }

    private void EvaluateConstant(Token token, Stack<double> rpnStack)
    {
        if (_constants.TryGetValue(token.Value, out double constant))
            rpnStack.Push(constant);
    }

    private void EvaluateOperation(Token token, Stack<double> rpnStack)
    {
        if (!_operations.TryGetValue(token.Value, out var operation))
            throw new Exception($"got unknown function \"{token.Value}\" ({token.Start}:{token.End})");

        var args = new double[operation.ArgsCount];
        for (int i = operation.ArgsCount - 1; i >= 0; i--)
        {
            args[i] = rpnStack.Pop();
        }

        rpnStack.Push(operation.Call(args));
    }
}
