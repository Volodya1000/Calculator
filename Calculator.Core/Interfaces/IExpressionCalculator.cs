using Calculator.Core.ResultPattern;

namespace Calculator.Core.Interfaces;

public interface IExpressionCalculator
{
    Result<double> EvaluateExpression(string expression);
    public string GetStringAfterErasingLastToken(string expression);
    public string TryAddOperator(string expression, string operatorName);
}
