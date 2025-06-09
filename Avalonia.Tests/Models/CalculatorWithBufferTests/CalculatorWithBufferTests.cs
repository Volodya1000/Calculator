using Calculator.Avalonia.Models;
using Calculator.Core.Operations;

namespace Avalonia.Tests.Models.CalculatorWithBufferTests;

public class CalculatorWithBufferTests
{
    public CalculatorWithBuffer CreateCalculator()
    {
        var operations = new OperationsBuilder().AddAll().Build();
        var calculator = new Calculator.Core.Calculator(operations);
        return new CalculatorWithBuffer(calculator);
    }

    [Fact(DisplayName = "EnterNumber: корректное добавление числа и отбражение в обоих буферах")]
    public void NumberInput_UpdatesDisplay_HistoryRemainsEmpty()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("1", "", "1");
        tester.ExecuteSequence("0", "", "10");
    }

    [Fact(DisplayName = "EnterNumber: после вызова несколько раз подряд на MainBuffer отображается конкатанация чисел")]
    public void After_Calling_Several_Times_MainBuffer_Displays_Concatenation_Of_Numbers()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("1", "", "1");
        tester.ExecuteSequence("0", "", "10");
        tester.ExecuteSequence("1", "", "101");
        tester.ExecuteSequence("5", "", "1015");
        tester.ExecuteSequence("2", "", "10152");
        tester.ExecuteSequence("7", "", "101527");
    }

    [Fact(DisplayName = "Корректное отображение точки после нуля")]
    public void Dot_AfterZero_AddsDecimal()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence(".", "", "0.");
    }

    [Fact(DisplayName = "EnterOperation: обрезка незначащих нулей для дробного числа")]
    public void Trimming_Trailing_Zeros()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence(".", "", "0.");
        tester.ExecuteSequence("5", "", "0.5");
        tester.ExecuteSequence("0", "", "0.50");
        tester.ExecuteSequence("0", "", "0.500");
        tester.ExecuteSequence("0", "", "0.5000");
        tester.ExecuteSequence("+", "0.5+", "0.5");
    }

    [Fact(DisplayName = "При вводе операции для числа обрезаются нули на конце Для нуля")]
    public void Trimming_Trailing_Zeros_For_Zero()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence(".", "", "0.");
        tester.ExecuteSequence("0", "", "0.0");
        tester.ExecuteSequence("0", "", "0.00");
        tester.ExecuteSequence("0", "", "0.000");
        tester.ExecuteSequence("+", "0+", "0");
    }

    [Fact(DisplayName = "К числу с точкой нельзя добавить вторую точку")]
    public void EnterDotMultipleTimesAddsOnlyOneDot()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence(".", "", "0.");
        tester.ExecuteSequence(".", "", "0.");
        tester.ExecuteSequence(".", "", "0.");
    }

    [Fact(DisplayName = "Корректное нажатие операции после ввода числа")]
    public void Correct_Operation_Entering_After_Entering_Number()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("1", "", "1");
        tester.ExecuteSequence("0", "", "10");
        tester.ExecuteSequence("+", "10+", "10");
    }

    [Fact(DisplayName = "EnterNumber: замена начального нуля при вводе числа")]
    public void EnterNumber_Replaces_Initial_Zero()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("0", "", "0");
        tester.ExecuteSequence("5", "", "5");
    }

    [Fact(DisplayName = "EnterNumber: множественные нули остаются одним нулём")]
    public void Multiple_Zeros_Kept_As_Single_Zero()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("0", "", "0");
        tester.ExecuteSequence("0", "", "0");
        tester.ExecuteSequence("0", "", "0");
    }

    [Fact(DisplayName = "EnterOperation: замена предыдущей операции")]
    public void Operation_Replaces_Previous_Operation()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("5", "", "5");
        tester.ExecuteSequence("+", "5+", "5");
        tester.ExecuteSequence("-", "5-", "5");
    }

    [Fact(DisplayName = "Когда отображается результат и вводится число, то оно перекрывает резуьтат")]
    public void Execute_Performs_Calculation_And_Enter_Number()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("2", "", "2");
        tester.ExecuteSequence("+", "2+", "2");
        tester.ExecuteSequence("2", "2+", "2");
        tester.ExecuteSequence("=", "2+2", "4");
        tester.ExecuteSequence("5", "", "5");
        tester.ExecuteSequence("-", "5-", "5");
        tester.ExecuteSequence("2", "5-", "2");
        tester.ExecuteSequence("=", "5-2", "3");
    }

    [Fact(DisplayName = "простое вычисление 2+2=4, Когда отображается результат и вводится число, то оно перекрывает резуьтат")]
    public void Execute_Performs_Calculation_And_Enter_Dot()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("2", "", "2");
        tester.ExecuteSequence("+", "2+", "2");
        tester.ExecuteSequence("2", "2+", "2");
        tester.ExecuteSequence("=", "2+2", "4");
        tester.ExecuteSequence(".", "", "0.");
    }

    [Fact(DisplayName = "Execute: вычисление с последовательными операциями")]
    public void Execute_With_Multiple_Operations()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("10", "", "10");
        tester.ExecuteSequence("+", "10+", "10");
        tester.ExecuteSequence("5", "10+5", "5");
        tester.ExecuteSequence("*", "15*", "15");
        tester.ExecuteSequence("2", "15*2", "2");
        tester.ExecuteSequence("=", "15*2", "30");
    }

    [Fact(DisplayName = "ClearMainBuffer: сброс основного буфера")]
    public void ClearMainBuffer_Resets_MainBuffer()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("1234", "", "1234");
        tester.ExecuteSequence("CE", "", "0");
    }

    [Fact(DisplayName = "полный сброс буферов")]
    public void ClearAll_Resets_Both_Buffers()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("3", "", "3");
        tester.ExecuteSequence("+", "3+", "3");
        tester.ExecuteSequence("AC", "", "0");
    }

    [Fact(DisplayName = "EraseLast: удаление последнего символа")]
    public void EraseLast_Removes_Last_Character()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("1", "", "1");
        tester.ExecuteSequence("2", "", "12");
        tester.ExecuteSequence("3", "", "123");
        tester.ExecuteSequence("←", "", "12");
        tester.ExecuteSequence("←", "", "1");
        tester.ExecuteSequence("←", "", "0");
        tester.ExecuteSequence("←", "", "0");
    }

    [Fact(DisplayName = "EraseLast: удаление последнего символа после execute")]
    public void EraseLast_Removes_Last_Character_After_Execute()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("1", "", "1");
        tester.ExecuteSequence("=", "", "1");
        tester.ExecuteSequence("←", "", "1");
        tester.ExecuteSequence("←", "", "1");
    }

    [Fact(DisplayName = "EnterOperation: ввод операции без ввода операнда")]
    public void Operation_Without_Operand_Uses_Zero()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("+", "0+", "0");
    }

    [Fact(DisplayName = "Execute: вычисление без операции возвращает число")]
    public void Execute_Without_Operation_Keeps_Value()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("7", "", "7");
        tester.ExecuteSequence("=", "", "7");
    }

    [Fact(DisplayName = "EnterDot: игнорирование точки в середине числа")]
    public void Dot_In_Middle_Of_Number_Ignored()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("1", "", "1");
        tester.ExecuteSequence("5", "", "15");
        tester.ExecuteSequence(".", "", "15.");
        tester.ExecuteSequence("3", "", "15.3");
        tester.ExecuteSequence(".", "", "15.3");
    }

    [Fact(DisplayName = "DivideByZero: деление на ноль")]
    public void Divide_By_Zero_Shows_Error()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("1", "", "1");
        tester.ExecuteSequence("/", "1/", "1");
        tester.ExecuteSequence("0", "1/0", "0");
        tester.ExecuteSequence("=", "???", "???");
        Assert.False(true); //Тест пока не реализован
    }

    [Fact(DisplayName = "Ограничение на длинну буфера буфера при длинном числе")]
    public void Long_Number_Input_Limited()
    {
        var numberWithMaxLength = string.Concat(Enumerable.Repeat("9", CalculatorWithBuffer.MAX_MAIN_BUFFER_LENGTH));
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence(numberWithMaxLength, "", numberWithMaxLength);
        tester.ExecuteSequence("9", "", numberWithMaxLength);
    }

    [Fact(DisplayName = "повторное нажатие '='")]
    public void Repeat_Execute_Operation()
    {
        var tester = new CalculatorActionTester();
        tester.ExecuteSequence("2", "", "2");
        tester.ExecuteSequence("+", "2+", "2");
        tester.ExecuteSequence("4", "2+", "4");
        tester.ExecuteSequence("=", "2+4", "6");
        tester.ExecuteSequence("=", "2+4", "6");
    }
}

