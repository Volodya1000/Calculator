using Calculator.Core.ExpressionEvaluator;
using Calculator.Core.ExpressionEvaluator.Tokinezation;
using Calculator.Core.Interfaces;
using Calculator.Core.ResultPattern;

namespace Calculator.Core;

public class ExpressionsEvaluatorFacade : IExpressionCalculator
{
    private readonly Dictionary<string, IOperation> _operations;
    private readonly ExpressionTokenizer _tokenizer;
    private readonly PrattParser _parser;
    private readonly ExpressionEvaluator.ExpressionEvaluator _evaluator;

    public ExpressionsEvaluatorFacade(Calculator calculator,
                                       Dictionary<string, double> constants)
    {
        _operations = new Dictionary<string, IOperation>(
            calculator.Operations,
            StringComparer.OrdinalIgnoreCase);
        _tokenizer = new ExpressionTokenizer(calculator.Operations.Keys.ToList(), constants.Keys.ToList());
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

        if (tokens.Count >= 1)
        {
            var lastTokenType = tokens[tokens.Count - 1].Type;
            if (lastTokenType == TokenType.Constant)
            {
                tokens.RemoveAt(tokens.Count - 1);
                var tokensNames = tokens.Select(t => t.Value);
                return String.Join("", tokensNames);
            }
            if (tokens.Count > 1)
            {
                var prevLastTokenType = tokens[tokens.Count - 2].Type;
                if (lastTokenType == TokenType.LeftParenthesis && prevLastTokenType == TokenType.Function)
                {
                    tokens.RemoveAt(tokens.Count - 1);
                    tokens.RemoveAt(tokens.Count - 1);
                    var tokensNames = tokens.Select(t => t.Value);
                    return String.Join("", tokensNames);
                }
            }
        }
        return expression[..^1];
    }
}
