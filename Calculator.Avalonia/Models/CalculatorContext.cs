using Calculator.Avalonia.Models.CalculatorStates;
using Calculator.Avalonia.Models.Interfaces;
using System.Collections.Generic;
using System.Globalization;

namespace Calculator.Avalonia.Models;

public class CalculatorContext
{
    internal double? PreviousValue { get; set; }
    internal string BinaryOperation { get; set; }
    internal string CurrentInput { get; set; } = "0";

    // Вычисляемые свойства для отображения
    public string Display => CurrentInput;

    public string Buffer
    {
        get
        {
            IList<string> parts = new List<string>();

            // Предыдущее значение и бинарная операция
            if (PreviousValue.HasValue)
                parts.Add(PreviousValue.Value.ToString(CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(BinaryOperation))
                parts.Add(BinaryOperation);

            // Текущий ввод
            parts.Add(CurrentInput);

            return string.Join(" ", parts);
        }
    }

    // Текущее состояние 
    internal ICalculatorState State { get; set; }

    public CalculatorContext()
    {
        State = new ReadyForInputState(this);
    }

    // Диспетчеры действий
    public void EnterNumber(char digit) => State.EnterNumber(digit);
    public void EnterDot() => State.EnterDot();
    public void EnterBinaryOperation(string op) => State.EnterBinaryOperation(op);
    public void EnterUnaryPreffixOperation(string op) => State.EnterUnaryPreffixOperation(op);
    public void Execute() => State.Execute();
    public void EraseLast() => State.EraseLast();

    public void ClearEntry() => CurrentInput = "0";

    public void ClearAll()
    {
        PreviousValue = null;
        BinaryOperation = null;
        CurrentInput = "0";
        TransitionTo(new ReadyForInputState(this));
    }

    internal void TransitionTo(ICalculatorState newState) => State = newState;

    internal bool TryParseInput(out double value)
    {
        if (ConstantFactory.IsSymbol(CurrentInput))
        {
            value = ConstantFactory.GetValue(CurrentInput);
            return true;
        }
        return double.TryParse(CurrentInput, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }


    internal void SetInput(double value) =>
         CurrentInput = value.ToString(CultureInfo.InvariantCulture);

}


//public class CalculatorContext
//{
//    public ICalculatorState CurrentState { get; set; }
//    public string MainBuffer { get; set; } = "0";
//    public string HistoryBuffer { get; set; } = "";
//    public double? CurrentValue { get; set; } = null;
//    public string PendingOperation { get; set; } = "";
//    public OperationType? PendingOperationType { get; set; } = null;
//    public Calculator.Core.Calculator CoreCalculator { get; }

//    public CalculatorContext(Calculator.Core.Calculator calculator)
//    {
//        CoreCalculator = calculator;
//        CurrentState = new ReadyForInputState(); // Начальное состояние
//    }
//}
