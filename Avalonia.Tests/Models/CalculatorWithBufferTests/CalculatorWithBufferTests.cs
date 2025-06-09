using Calculator.Avalonia.Models;
using FluentAssertions;

namespace Avalonia.Tests.Models.CalculatorWithBufferTests;

public class CalculatorWithBufferTests
{
    [Fact(DisplayName = "EnterNumber: корректное добавление числа и отбражение в обоих буферах")]
    public void NumberInput_UpdatesDisplay_HistoryRemainsEmpty()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();
        int number=10;

        //Act
        calc.EnterNumber(number);

        //Assert
        calc.HistoryBuffer.Should().BeEmpty();
        calc.MainBuffer.Should().Be(number.ToString());
    }

    [Fact(DisplayName = "EnterNumber: после вызова несколько раз подряд на MainBuffer отображается конкатанация чисел ")]
    public void After_Calling_Several_Times_MainBuffer_Displays_Concatenation_Of_Numbers()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();

        int number1 = 10;
        int number2 = 15;
        int number3 = 27;

        string concatenationOfNumbers = $"{number1}{number2}{number3}";

        //Act
        calc.EnterNumber(number1);
        calc.EnterNumber(number2);
        calc.EnterNumber(number3);

        //Assert
        calc.HistoryBuffer.Should().BeEmpty();
        calc.MainBuffer.Should().Be(concatenationOfNumbers);
    }

    [Fact(DisplayName ="Корректное отображение точки после нуля")]
    public void Dot_AfterZero_AddsDecimal()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();

        //Act
        calc.EnterDot();

        //Assert
        calc.HistoryBuffer.Should().BeEmpty();
        calc.MainBuffer.Should().Be("0.");
    }

    [Fact(DisplayName= "EnterOperation: обрезка незначащих нулей для дробного числа")]
    public void Trimming_Trailing_Zeros()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();
        int number = 5;
        int zeroNumber = 0;

        //Act
        calc.EnterDot();

        calc.EnterNumber(number);

        calc.EnterNumber(zeroNumber);
        calc.EnterNumber(zeroNumber);
        calc.EnterNumber(zeroNumber);

        calc.EnterOperation(OperationType.Addition);

        //Assert
        calc.HistoryBuffer.Should().Be($"0.{number}+"); //нужно улучшить добавив какое то соответствие между OperationType.Substraction и +
        calc.MainBuffer.Should().Be($"0.{number}");
    }

    [Fact(DisplayName = "При вводе операции для числа обрезаются нули на конце Для нуля")]
    public void Trimming_Trailing_Zeros_For_Zero()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();
        int zeroNumber = 0;

        //Act
        calc.EnterDot();

        calc.EnterNumber(zeroNumber);
        calc.EnterNumber(zeroNumber);
        calc.EnterNumber(zeroNumber);

        calc.EnterOperation(OperationType.Addition);

        //Assert
        calc.HistoryBuffer.Should().Be($"0+"); //нужно улучшить добавив какое то соответствие между OperationType.Substraction и +
        calc.MainBuffer.Should().Be($"0");
    }

    [Fact(DisplayName = "К числу с точкой нельзя добавить вторую точку")]
    public void EnterDotMultipleTimesAddsOnlyOneDot()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();

        //Act
        calc.EnterDot();
        calc.EnterDot();
        calc.EnterDot();

        //Assert
        calc.HistoryBuffer.Should().BeEmpty();
        calc.MainBuffer.Should().Be("0.");
    }


    [Fact(DisplayName = "Корректное нажатие операции после ввода числа")]
    public void Correct_Operation_Entering_After_Enterin_Number()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();
        int number = 10;

        //Act
        calc.EnterNumber(number);

        //Assert
        calc.HistoryBuffer.Should().BeEmpty($"{number}+"); //нужно улучшить добавив какое то соответствие между OperationType.Substraction и +
        calc.MainBuffer.Should().Be($"{number}");
    }

    [Fact(DisplayName = "EnterNumber: замена начального нуля при вводе числа")]
    public void EnterNumber_Replaces_Initial_Zero()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();
        int number = 5;

        //Act
        calc.EnterNumber(0);
        calc.EnterNumber(number);

        //Assert
        calc.MainBuffer.Should().Be(number.ToString());
    }

    [Fact(DisplayName = "EnterNumber: множественные нули остаются одним нулём")]
    public void Multiple_Zeros_Kept_As_Single_Zero()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();

        //Act
        calc.EnterNumber(0);
        calc.EnterNumber(0);
        calc.EnterNumber(0);

        //Assert
        calc.MainBuffer.Should().Be("0");
    }

    [Fact(DisplayName = "EnterOperation: замена предыдущей операции")]
    public void Operation_Replaces_Previous_Operation()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();

        calc.EnterNumber(5);
        calc.EnterOperation(OperationType.Addition);
        calc.EnterOperation(OperationType.Subtraction);
        calc.HistoryBuffer.Should().Be("5-");
    }

    [Fact(DisplayName = "Execute: простое вычисление 2+2=4")]
    public void Execute_Performs_Calculation()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();

        calc.EnterNumber(2);
        calc.EnterOperation(OperationType.Addition);
        calc.EnterNumber(2);
        calc.Execute();
        calc.MainBuffer.Should().Be("4");
        calc.HistoryBuffer.Should().Be("2+2");
    }

    [Fact(DisplayName = "Execute: вычисление с последовательными операциями: после того как достаочно операндов для вычисления предыдущей операции и нажата следующая, предыдущая операция должна автоматически вычисляться")]
    public void Execute_With_Multiple_Operations_Correct_DisplayBuffer()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();

        int number1 = 10;
        int number2 = 5;

        int result1 = number1 + number2;

        //Act
        calc.EnterNumber(10);
        calc.EnterOperation(OperationType.Addition);
        calc.EnterNumber(5);
        calc.EnterOperation(OperationType.Multiplication);
        calc.Execute();
        calc.MainBuffer.Should().Be("15");
        calc.HistoryBuffer.Should().Be("10+5*2=");
    }

    [Fact(DisplayName = "Execute: вычисление с последовательными операциями")]
    public void Execute_With_Multiple_Operations()
    {
        //Arrange
        var calc = new CalculatorWithBuffer();

        int number1 = 10;
        int number2 = 5;

        int result1 = number1 + number2;

        //Act
        calc.EnterNumber(10);
        calc.EnterOperation(OperationType.Addition);
        calc.EnterNumber(5);
        calc.EnterOperation(OperationType.Multiplication);
        calc.EnterNumber(2);
        calc.Execute();
        calc.MainBuffer.Should().Be("30");
        calc.HistoryBuffer.Should().Be("10+5*2=");
    }


    // Вспомогательный метод для последовательных действий
    private void ExecuteSequence(params (string action, string expectedHistory, string expectedDisplay)[] steps)
    {
        var calc = new CalculatorWithBuffer();
        foreach (var step in steps)
        {
            switch (step.action)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    calc.EnterNumber(int.Parse(step.action));
                    break;
                case ".":
                    calc.EnterDot();
                    break;
                case "+":
                    calc.EnterOperation(OperationType.Addition);
                    break;
                case "-":
                    calc.EnterOperation(OperationType.Subtraction);
                    break;
                case "*":
                    calc.EnterOperation(OperationType.Multiplication);
                    break;
                case "/":
                    calc.EnterOperation(OperationType.Dividing);
                    break;
                case "=":
                    calc.Execute();
                    break; 
                case "C":
                    calc.ClearMainBuffer();
                    break;
                case "AC":
                    calc.ClearMainAndHistoryBufers();
                    break;
            }

            calc.HistoryBuffer.Should().Be(step.expectedHistory);
            calc.MainBuffer.Should().Be(step.expectedDisplay);
        }
    }
}
