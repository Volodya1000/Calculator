using Calculator.Avalonia.Models.Interfaces;

namespace Calculator.Avalonia.Models;

public class CalculatorContext
{
    public ICalculatorState CurrentState { get; set; }
    public string MainBuffer { get; set; } = "0";
    public string HistoryBuffer { get; set; } = "";
    public double? CurrentValue { get; set; } = null;
    public string PendingOperation { get; set; } = "";
    public OperationType? PendingOperationType { get; set; } = null;
    public Calculator.Core.Calculator CoreCalculator { get; }

    public CalculatorContext(Calculator.Core.Calculator calculator)
    {
        CoreCalculator = calculator;
        CurrentState = new ReadyForInputState(); // Начальное состояние
    }
}
