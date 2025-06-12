using Calculator.Core.Exceptions.ExpressionExceptions;
using Calculator.Core.Exceptions.OperationExceptions;
using Calculator.Core.ExpressionEvaluator.Tokinezation;
using Calculator.Core.Interfaces;
using Calculator.Core.Operations;
using System.Globalization;

namespace Calculator.Core.ExpressionEvaluator;

public class ExpressionEvaluator

{
    private readonly Dictionary<string, IOperation> _operations;
    private readonly Dictionary<string, double> _constants;
    public ExpressionEvaluator(Dictionary<string, IOperation> operations,Dictionary<string,double> constants)
    {
        _operations= operations;
        _constants= constants;
        _operations.Add("~", new Operation("~", args =>(-1)*args[0],1));
    }

    public double Evaluate(IEnumerable<Token> rpn)
    {
        Stack<double> rpnStack = new Stack<double>();

        foreach (Token token in rpn)
        {
            switch (token.Type)
            {
                case TokenType.Number:
                    rpnStack.Push(double.Parse(token.Value, CultureInfo.InvariantCulture));
                    break;
                case TokenType.Constant:
                    EvaluateConstant(token,rpnStack);
                    break;
                case TokenType.Operator:
                case TokenType.Function:
                    EvaluateOperation(token, rpnStack);
                    break;
                default:
                    throw new UnexpectedTokenException(token.Value,"",token.Start,token.End);
            }
        }

        //В стеке должен остаться только один ответ
        if (rpnStack.Count != 1)
            throw new InvalidExpressionException();

        return rpnStack.Pop();
    }

    private void EvaluateConstant(Token token, Stack<double> rpnStack)
    {
        if (!_constants.TryGetValue(token.Value, out double constant))
            throw new UnknownTokenException(token.Value, token.Start, token.End);

        rpnStack.Push(constant);
    }

    private void EvaluateOperation(Token token, Stack<double> rpnStack)
    {
        if (!_operations.TryGetValue(token.Value, out var operation))
            throw new UnknownTokenException(token.Value,token.Start,token.End);

        if (rpnStack.Count < operation.ArgsCount)
            throw new InsufficientArgumentsException(token.Value, operation.ArgsCount, rpnStack.Count);

        var args = new double[operation.ArgsCount];
        for (int i = operation.ArgsCount - 1; i >= 0; i--)
        {
            args[i] = rpnStack.Pop();
        }

        rpnStack.Push(operation.Call(args));
    }
}
