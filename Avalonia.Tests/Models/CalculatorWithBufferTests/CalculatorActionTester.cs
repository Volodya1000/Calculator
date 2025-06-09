using Calculator.Avalonia.Models;
using Calculator.Core.Operations;
using FluentAssertions;

namespace Avalonia.Tests.Models.CalculatorWithBufferTests;

public class CalculatorActionTester
{
    private readonly CalculatorWithBuffer _calc;
    public CalculatorActionTester()
    {
        var operations = new OperationsBuilder().AddAll().Build();
        _calc = new CalculatorWithBuffer(new Calculator.Core.Calculator(operations));
    }

    // Вспомогательный метод для последовательных действий
    public void ExecuteSequence(string action, string expectedHistory, string expectedDisplay)
    {
        if (int.TryParse(action, out int number))
            _calc.EnterNumber(number);
        else
        {
            switch (action)
            {
                case ".":
                    _calc.EnterDot();
                    break;
                case "+":
                    _calc.EnterOperation(OperationType.Addition);
                    break;
                case "-":
                    _calc.EnterOperation(OperationType.Subtraction);
                    break;
                case "*":
                    _calc.EnterOperation(OperationType.Multiplication);
                    break;
                case "/":
                    _calc.EnterOperation(OperationType.Dividing);
                    break;
                case "=":
                    _calc.Execute();
                    break;
                case "CE":
                    _calc.ClearMainBuffer();
                    break;
                case "AC":
                    _calc.ClearMainAndHistoryBufers();
                    break;
                case "←":
                    _calc.EraseLast();
                    break;
                default:
                    throw new ArgumentException($"Unknown action: {action}");
            }

            _calc.HistoryBuffer.Replace(" ", "").Should().Be(expectedHistory);
            _calc.MainBuffer.Replace(" ", "").Should().Be(expectedDisplay);
        }
    }
}
