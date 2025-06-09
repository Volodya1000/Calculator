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
        calc.MainBuffer.Should().Equals(number);
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
        calc.MainBuffer.Should().Equals(concatenationOfNumbers);
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
        calc.MainBuffer.Should().Equals("0.");
    }

    [Fact(DisplayName= "При вводе операции для числа обрезаются нули на конце")]
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

        calc.EnterOperatin(OperationType.Addition);

        //Assert
        calc.HistoryBuffer.Should().BeEmpty($"0.{number}+"); //нужно улучшить добавив какое то соответствие между OperationType.Substraction и +
        calc.MainBuffer.Should().Equals($"0.{number}");
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

        calc.EnterOperatin(OperationType.Addition);

        //Assert
        calc.HistoryBuffer.Should().BeEmpty($"0+"); //нужно улучшить добавив какое то соответствие между OperationType.Substraction и +
        calc.MainBuffer.Should().Equals($"0");
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
        calc.MainBuffer.Should().Equals("0.");
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
        calc.MainBuffer.Should().Equals($"{number}");
    }
}
