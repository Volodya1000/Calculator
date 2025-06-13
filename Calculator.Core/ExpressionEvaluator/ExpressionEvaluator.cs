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
    }

    public double Evaluate(IEnumerable<Token> rpn)
    {
        Stack<double> operandsStack = new Stack<double>();

        foreach (Token token in rpn)
        {
            switch (token.Type)
            {
                case TokenType.Number:
                    operandsStack.Push(double.Parse(token.Value, CultureInfo.InvariantCulture));
                    break;
                case TokenType.Constant:
                    EvaluateConstant(token,operandsStack);
                    break;
                case TokenType.Operator:
                case TokenType.Function:
                    EvaluateOperation(token, operandsStack);
                    break;
                case TokenType.Delimiter:
                    operandsStack.Push(double.NaN); //NaN как маркер в стеке для функций с неограниченным количеством аргументов
                    break;
                default:
                    throw new UnexpectedTokenException(token.Value,"",token.Start,token.End);
            }
        }

        //В стеке должен остаться только один ответ
        if (operandsStack.Count != 1)
            throw new InvalidExpressionException();

        return operandsStack.Pop();
    }

    private void EvaluateConstant(Token token, Stack<double> operandsStack)
    {
        if (!_constants.TryGetValue(token.Value, out double constant))
            throw new UnknownTokenException(token.Value, token.Start, token.End);

        operandsStack.Push(constant);
    }

    private void EvaluateOperation(Token token, Stack<double> operandsStack)
    {
        if (!_calculator.OperationExists(token.Value))
            throw new UnknownTokenException(token.Value, token.Start, token.End);

        int argsCount = _calculator.Operations[token.Value].ArgsCount;
        List<double> args = new List<double>();

        if (argsCount == -1)
        {
            // собираем аргументы пока не будет найден маркер
            while (operandsStack.Count > 0 && !double.IsNaN(operandsStack.Peek()))
            {
                args.Insert(0, operandsStack.Pop());
            }
            if (operandsStack.Count == 0)
                throw new InvalidExpressionException();
            operandsStack.Pop(); // удаление маркера
        }
        else
        {
            if (operandsStack.Count < argsCount)
                throw new InsufficientArgumentsException(token.Value, argsCount, operandsStack.Count);
            for (int i = 0; i < argsCount; i++)
            {
                args.Insert(0, operandsStack.Pop());
            }
        }

        var result = _calculator.Call(token.Value, args.ToArray());

        if (result.IsFailure)
            throw result.Error;
        else
            operandsStack.Push(result.Value);
    }

}
