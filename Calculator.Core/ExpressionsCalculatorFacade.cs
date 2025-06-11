using Calculator.Core.ExpressionEvaluator;
using Calculator.Core.ExpressionEvaluator.Tokinezation;
using Calculator.Core.Interfaces;
using Calculator.Core.ResultPattern;

namespace Calculator.Core;

public class ExpressionsCalculatorFacade
{
    private readonly Dictionary<string, IOperation> _operations;
    private readonly ExpressionTokenizer _tokenizer;
    private readonly PrattParser _parser;
    private readonly ExpressionEvaluator.ExpressionEvaluator _evaluator;

    //public ExpressionsCalculatorFacade(Dictionary<string, IOperation> operations, 
    //                  ExpressionTokenizer tokenizer,
    //                  PrattParser parser,
    //                  ExpressionEvaluator.ExpressionEvaluator evaluator)
    //{
    //    _operations = new Dictionary<string, IOperation>(
    //        operations,
    //        StringComparer.OrdinalIgnoreCase);
    //    _tokenizer = tokenizer;
    //    _parser = parser;
    //    _evaluator= evaluator;
    //}

    public ExpressionsCalculatorFacade(Dictionary<string, IOperation> operations, 
                                       Dictionary<string, double> constants)
    {
        _operations = new Dictionary<string, IOperation>(
            operations,
            StringComparer.OrdinalIgnoreCase);
        _tokenizer = new ExpressionTokenizer(operations.Keys.ToList(), constants.Keys.ToList()); 
        _parser = new PrattParser(operations);
        _evaluator = new ExpressionEvaluator.ExpressionEvaluator(operations, constants);
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
}
