using Calculator.Core.ResultPattern;

namespace Calculator.Core.Interfaces;

public interface IExpressionCalculator
{
    Result<double> EvaluateExpression(string expression);
}
