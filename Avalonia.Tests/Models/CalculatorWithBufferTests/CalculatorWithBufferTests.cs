using Calculator.Avalonia.Models;
using Calculator.Core.Operations;
using FluentAssertions;

namespace Avalonia.Tests.Models.CalculatorWithBufferTests;

public class CalculatorWithBufferTests
{
    public CalculatorWithBuffer CreateCalculator()
    {
        var operations = new OperationsBuilder().AddAll().Build();
        var calculator= new Calculator.Core.Calculator(operations);
        return new CalculatorWithBuffer(calculator);
    }


    [Fact(DisplayName = "EnterNumber: корректное добавление числа и отбражение в обоих буферах")]
    public void NumberInput_UpdatesDisplay_HistoryRemainsEmpty()
    {
        ExecuteSequence(
            ("1", "", "1"),
            ("0", "", "10")
        );
    }

    [Fact(DisplayName = "EnterNumber: после вызова несколько раз подряд на MainBuffer отображается конкатанация чисел")]
    public void After_Calling_Several_Times_MainBuffer_Displays_Concatenation_Of_Numbers()
    {
        ExecuteSequence(
            ("1", "", "1"),
            ("0", "", "10"),
            ("1", "", "101"),
            ("5", "", "1015"),
            ("2", "", "10152"),
            ("7", "", "101527")
        );
    }

    [Fact(DisplayName = "Корректное отображение точки после нуля")]
    public void Dot_AfterZero_AddsDecimal()
    {
        ExecuteSequence(
            (".", "", "0.")
        );
    }

    [Fact(DisplayName = "EnterOperation: обрезка незначащих нулей для дробного числа")]
    public void Trimming_Trailing_Zeros()
    {
        ExecuteSequence(
            (".", "", "0."),
            ("5", "", "0.5"),
            ("0", "", "0.50"),
            ("0", "", "0.500"),
            ("0", "", "0.5000"),
            ("+", "0.5+", "0.5")
        );
    }

    [Fact(DisplayName = "При вводе операции для числа обрезаются нули на конце Для нуля")]
    public void Trimming_Trailing_Zeros_For_Zero()
    {
        ExecuteSequence(
            (".", "", "0."),
            ("0", "", "0.0"),
            ("0", "", "0.00"),
            ("0", "", "0.000"),
            ("+", "0+", "0")
        );
    }

    [Fact(DisplayName = "К числу с точкой нельзя добавить вторую точку")]
    public void EnterDotMultipleTimesAddsOnlyOneDot()
    {
        ExecuteSequence(
            (".", "", "0."),
            (".", "", "0."),
            (".", "", "0.")
        );
    }

    [Fact(DisplayName = "Корректное нажатие операции после ввода числа")]
    public void Correct_Operation_Entering_After_Enterin_Number()
    {
        ExecuteSequence(
            ("1", "", "1"),
            ("0", "", "10"),
            ("+", "10+", "10")
        );
    }

    [Fact(DisplayName = "EnterNumber: замена начального нуля при вводе числа")]
    public void EnterNumber_Replaces_Initial_Zero()
    {
        ExecuteSequence(
            ("0", "", "0"),
            ("5", "", "5")
        );
    }

    [Fact(DisplayName = "EnterNumber: множественные нули остаются одним нулём")]
    public void Multiple_Zeros_Kept_As_Single_Zero()
    {
        ExecuteSequence(
            ("0", "", "0"),
            ("0", "", "0"),
            ("0", "", "0")
        );
    }

    [Fact(DisplayName = "EnterOperation: замена предыдущей операции")]
    public void Operation_Replaces_Previous_Operation()
    {
        ExecuteSequence(
            ("5", "", "5"),
            ("+", "5+", "5"),
            ("-", "5-", "5")
        );
    }

    [Fact(DisplayName = "простое вычисление 2+2=4, Когда отображается результат и вводится число, то оно перекрывает резуьтат")]
    public void Execute_Performs_Calculation_And_Enter_Number()
    {
        ExecuteSequence(
            ("2", "", "2"),
            ("+", "2+", "2"),
            ("2", "2+", "2"),
            ("=", "2+2", "4"),
            ("5", "5", "5")
            ); 
    }


    [Fact(DisplayName = "простое вычисление 2+2=4, Когда отображается результат и вводится число, то оно перекрывает резуьтат")]
    public void Execute_Performs_Calculationn_And_Enter_Dot()
    {
        ExecuteSequence(
            ("2", "", "2"),
            ("+", "2+", "2"),
            ("2", "2+", "2"),
            ("=", "2+2", "4"),
            (".", "0.", "0.")
            );
    }


    [Fact(DisplayName = "Execute: вычисление с последовательными операциями")]
    public void Execute_With_Multiple_Operations()
    {
        ExecuteSequence(
            ("10", "", "10"),
            ("+", "10+", "10"),
            ("5", "10+5", "5"),
            ("*", "15*", "15"),
            ("2", "15*2", "2"),
            ("=", "30", "30")
        );
    }

    [Fact(DisplayName = "ClearMainBuffer: сброс основного буфера")]
    public void ClearMainBuffer_Resets_MainBuffer()
    {
        ExecuteSequence(
            ("1234", "", "1234"),
            ("CE", "", "0")
        );
    }

    [Fact(DisplayName = "полный сброс буферов")]
    public void ClearAll_Resets_Both_Buffers()
    {
        ExecuteSequence(
            ("3", "", "3"),
            ("+", "3+", "3"),
            ("AC", "", "0")
        );
    }

    [Fact(DisplayName = "EraseLast: удаление последнего символа")]
    public void EraseLast_Removes_Last_Character()
    {
        ExecuteSequence(
            ("1", "", "1"),
            ("2", "", "12"),
            ("3", "", "123"),
            ("←", "", "12"),
            ("←", "", "1"),
            ("←", "", "0"),
            ("←", "", "0")
        );
    }

    [Fact(DisplayName = "EraseLast: удаление последнего символа после execute")]
    public void EraseLast_Removes_Last_Character_After_Execute()
    {
        ExecuteSequence(
            ("1", "", "1"),
            ("=", "", "1"),
            ("←", "", "1"),
            ("←", "", "1")
        );
    }

    [Fact(DisplayName = "EnterOperation: ввод операции без ввода операнда")]
    public void Operation_Without_Operand_Uses_Zero()
    {
        ExecuteSequence(
            ("+", "0+", "0")
        );
    }

    [Fact(DisplayName = "Execute: вычисление без операции возвращает число")]
    public void Execute_Without_Operation_Keeps_Value()
    {
        ExecuteSequence(
            ("7", "", "7"),
            ("=", "", "7")
        );
    }

    [Fact(DisplayName = "EnterDot: игнорирование точки в середине числа")]
    public void Dot_In_Middle_Of_Number_Ignored()
    {
        ExecuteSequence(
            ("1", "", "1"),
            ("5", "", "15"),
            (".", "", "15."),
            ("3", "", "15.3"),
            (".", "", "15.3")
        );
    }

    [Fact(DisplayName = "DivideByZero: деление на ноль")]
    public void Divide_By_Zero_Shows_Error()
    {
        ExecuteSequence(
            ("1", "", "1"),
            ("/", "1/", "1"),
            ("0", "1/0", "0"),
            ("=", "???", "???")
        );
        Assert.False(true); //Тест пока не реализован
    }

    [Fact(DisplayName = "Ограничение на длинну буфера буфера при длинном числе")]
    public void Long_Number_Input_Limited()
    {
        var numberWithMaxLength = string.Concat(Enumerable.Repeat("9", CalculatorWithBuffer.MAX_MAIN_BUFFER_LENGTH));
        ExecuteSequence(
            (numberWithMaxLength, "", numberWithMaxLength),
            ("9", "", numberWithMaxLength)
        );
    }

    [Fact(DisplayName = "повторное нажатие '='")]
    public void Repeat_Execute_Operation()
    {
        ExecuteSequence(
            ("2", "", "2"),
            ("+", "2+", "2"),
            ("4", "2+4", "6"),
            ("=", "2+4", "6"),
            ("=", "2+4", "6")
        );
    }

    // Вспомогательный метод для последовательных действий
    private void ExecuteSequence(params (string action, string expectedHistory, string expectedDisplay)[] steps)
    {
        var calc = CreateCalculator();
        foreach (var step in steps)
        {
            if (int.TryParse(step.action, out int number))
                calc.EnterNumber(number);
            else
            {
                switch (step.action)
                {
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
                    case "CE":
                        calc.ClearMainBuffer();
                        break;
                    case "AC":
                        calc.ClearMainAndHistoryBufers();
                        break;
                    case "←": 
                        calc.EraseLast();
                        break;
                    default:
                        throw new ArgumentException($"Unknown action: {step.action}");
                }
            }

            calc.HistoryBuffer.Replace(" ", "").Should().Be(step.expectedHistory);
            calc.MainBuffer.Replace(" ", "").Should().Be(step.expectedDisplay);
        }
    }
}
