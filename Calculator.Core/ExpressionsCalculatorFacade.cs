using Calculator.Core.ExpressionEvaluator;
using Calculator.Core.ExpressionEvaluator.Tokinezation;
using Calculator.Core.Interfaces;

namespace Calculator.Core;

public class ExpressionsCalculatorFacade
{
    private readonly Dictionary<string, IOperation> _operations;
    private readonly ExpressionTokenizer _tokenizer
    private readonly PrattParser _parser;
    private readonly ExpressionEvaluator evaluator;

    public Calculator(Dictionary<string, IOperation> operations, 
                      ExpressionTokenizer tokenizer,
                      PrattParser parser,
                      ExpressionEvaluator evaluator)
    {
        _operations = new Dictionary<string, IOperation>(
            operations,
            StringComparer.OrdinalIgnoreCase);

    }
}
