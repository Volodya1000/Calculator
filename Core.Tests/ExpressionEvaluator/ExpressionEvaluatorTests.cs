using Calculator.Core;
using Calculator.Core.ExpressionEvaluator;
using Calculator.Core.ExpressionEvaluator.Tokinezation;
using Calculator.Core.Operations;

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
    public void ValidExpressions_EvaluateCorrectly(string expression, double expected)
    {
        var operationsDict = new OperationsBuilder().AddAll().Build();
        var facade= new ExpressionsCalculatorFacade(operationsDict, new Dictionary<string, double>());
        var result = facade.EvaluateExpression(expression);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value, Eps);
    }

    //[Fact]
    //public void ExpressionWithVariables_EvaluatesCorrectly()
    //{
    //    // Arrange
    //    string expression = "sin(x) * (pi/-x - 5)^2";
    //    var parser = new ExpressionParser(expression);
    //    parser.SetVariable("x", -Math.PI / 2);
    //    parser.SetVariable("pi", Math.PI);

    //    // Act
    //    double result = parser.Evaluate();

    //    // Assert
    //    Assert.Equal(-9, result, Eps);
    //}

    //[Theory]
    //[InlineData("invalid", null)]
    //[InlineData("1 + +", null)]
    //[InlineData("max(1,2", null)]
    //public void InvalidExpressions_ThrowException(string expression, object _)
    //{
    //    var parser = new ExpressionParser(expression);
    //    Assert.Throws<Exception>(() => parser.Evaluate());
    //}
}
