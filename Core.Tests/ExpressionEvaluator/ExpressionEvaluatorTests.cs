using Calculator.Core;
using Calculator.Core.Builders;

namespace Core.Tests.ExpressionEvaluator;

public class ExpressionEvaluatorTests
{
    private const double Eps = 1e-15;

    [Theory]
    [InlineData("1", 1)]
    [InlineData("1 + 2 * 3", 7)]
    [InlineData("-(1 + 2) * 3 - 4", -13)]
    [InlineData("-2^2", -4)]
    [InlineData("(-2)^2", 4)]
    [InlineData("-2^-2", -0.25)]
    [InlineData("(-2)^-2", 0.25)]
    [InlineData("cos(7 - 5)^2 + sin(4-2)^2", 1)]
    [InlineData("500%!", 120)]
    [InlineData("max(min(1,2),min(3,4),0)", 3)]
    public void ValidExpressions_EvaluateCorrectly(string expression, double expected)
    {
        var constantsDictionary = new ConstantsBuilder().AddMathConstants().Build();

        var calculator = new Calculator.Core.Calculator().AddAssembly("Calculator.Core.dll");

        var facade= new ExpressionsEvaluatorFacade(calculator, constantsDictionary);
        var result = facade.EvaluateExpression(expression);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value, Eps);
    }
}
