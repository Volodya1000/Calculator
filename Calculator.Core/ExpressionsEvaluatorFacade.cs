using Calculator.Core.ExpressionEvaluator;
using Calculator.Core.ExpressionEvaluator.Tokinezation;
using Calculator.Core.Interfaces;
using Calculator.Core.Operations;
using Calculator.Core.ResultPattern;

namespace Calculator.Core;

public class ExpressionsEvaluatorFacade : IExpressionCalculator
{
    private readonly ExpressionTokenizer _tokenizer;
    private readonly PrattParser _parser;
    private readonly ExpressionEvaluator.ExpressionEvaluator _evaluator;
    private readonly Calculator _calculator;

    public ExpressionsEvaluatorFacade(Calculator calculator,
                                       Dictionary<string, double> constants)
    {
        _calculator = calculator;
        _tokenizer = new ExpressionTokenizer(calculator.GetAllOperationsNames().ToList(), calculator.GetOperatorNames().ToList() ,constants.Keys.ToList());
        _parser = new PrattParser(calculator.Operations);
        _evaluator = new ExpressionEvaluator.ExpressionEvaluator(calculator, constants);
    }

    public Result<double> EvaluateExpression(string expression)
    {
        try
        {
            var tokens = _tokenizer.Tokenize(expression);
            var rpn = _parser.Parse(tokens.ToList());
            var result = _evaluator.Evaluate(rpn);
            return Result<double>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<double>.Failure(ex);
        }
    }

    /// <summary>
    /// Удаляет последний токен из математического выражения.
    /// Специальные случаи:
    /// 1. Если последний токен — константа, удаляет её полное название.
    /// 2. Если последний токен — '(', а предыдущий — функция (например, "sin("), удаляет оба.
    /// В остальных случаях удаляет последний символ строки.
    public string GetStringAfterErasingLastToken(string expression)
    {
        var tokens = new List<Token>();
        try
        {
            tokens = _tokenizer.Tokenize(expression);
        }
        catch
        {
            return expression[..^1];
        }

        if (tokens.Count == 0)
            return expression[..^1];

        var lastTokenType = tokens[^1].Type;
        if (lastTokenType == TokenType.Constant)
        {
            tokens.RemoveAt(tokens.Count - 1);
            return string.Join("", tokens.Select(t => t.Value));
        }

        if (tokens.Count <= 1)
            return expression[..^1];

        var prevLastTokenType = tokens[^2].Type;
        if (lastTokenType == TokenType.LeftParenthesis && prevLastTokenType == TokenType.Function)
        {
            tokens.RemoveAt(tokens.Count - 1);
            tokens.RemoveAt(tokens.Count - 1);
            return string.Join("", tokens.Select(t => t.Value));
        }

        return expression[..^1];
    }


    /// <summary>
    /// Не позволяет ввести после бинарного оператора новый оператор
    /// Заменяет бинарный оператор на новый оператор
    /// Пример:
    /// 2+ Нажат минус Будет 2-
    /// При этом если 2! нажат минус Будет 2!-
    /// </summary>
    public string TryAddOperator(string expression, string operatorName)
    {
        var defaultExpression = expression + operatorName;

        var tokens = new List<Token>();
        try
        {
            tokens = _tokenizer.Tokenize(expression);
        }
        catch
        {
            return defaultExpression;
        }

        if (tokens.Count < 1)
            return defaultExpression;

        var lastToken = tokens[^1];
        if (lastToken.Type != TokenType.Operator)
            return defaultExpression;

        var @operator = _calculator.Operations
            .Where(pair => pair.Key == lastToken.Value && pair.Value is Operator)
            .Select(pair => pair.Value)
            .FirstOrDefault();

        if (@operator == null || @operator.ArgsCount != 2)
            return defaultExpression;

        tokens.RemoveAt(tokens.Count - 1);
        return string.Join("", tokens.Select(t => t.Value)) + operatorName;
    }
}
