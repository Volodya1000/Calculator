using Calculator.Core.Exceptions.ExpressionExceptions;
using Calculator.Core.Exceptions.OperationExceptions;
using Calculator.Core.ExpressionEvaluator.Tokinezation;
using System.Globalization;

namespace Calculator.Core.ExpressionEvaluator;

public class ExpressionEvaluator
{
    private readonly Calculator _calculator;
    private readonly Dictionary<string, double> _constants;

    public ExpressionEvaluator(Calculator calculator,Dictionary<string,double> constants)
    {
        _constants= constants;
        _calculator=calculator;
        _calculator["~"]= (double arg) =>(-1)*arg;
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
        if (!_calculator.OperationExists(token.Value))
            throw new UnknownTokenException(token.Value,token.Start,token.End);

        int argsCount = _calculator.Operations[token.Value].ArgsCount;

        if (rpnStack.Count < argsCount)
            throw new InsufficientArgumentsException(token.Value, argsCount, rpnStack.Count);
        double[] args;
        if (argsCount == -1)
        {
            args = rpnStack.ToArray();
        }
        else
        {
            // Выделение массива под нужное число аргументов
            args = new double[argsCount];

            // Забираем нужное количество элементов из стека
            for (int i = argsCount - 1; i >= 0; i--)
            {
                args[i] = rpnStack.Pop();
            }
        }

        var result = _calculator.Call(token.Value, args);

        if (result.IsFailure)
            throw result.Error;
        else
            rpnStack.Push(result.Value);
    }
}
