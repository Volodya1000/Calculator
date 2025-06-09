using Calculator.Avalonia.Models.Interfaces;

namespace Calculator.Avalonia.Models.CalculatorStates;

public class ReadyForInputState : ICalculatorState
{
    public void EnterNumber(CalculatorContext context, int number)
    {
        context.MainBuffer = number.ToString();
        context.CurrentState = new EnteringNumberState(); // Переход в состояние ввода числа
    }

    public void EnterOperation(CalculatorContext context, OperationType operation)
    {
        context.CurrentValue = 0;
        context.PendingOperation = OperationHelper.GetSymbol(operation).ToString();
        context.PendingOperationType = operation;
        context.HistoryBuffer = $"0 {context.PendingOperation}";
        context.CurrentState = new OperationSelectedState(); // Переход в состояние выбора операции
    }

    //public void ExecuteUnary(CalculatorContext context, OperationType operation) =>
    //    context.MainBuffer = "Error";

    public void ExecuteBinary(CalculatorContext context) =>
        context.MainBuffer = "Error";

    public void EnterDot(CalculatorContext context) =>
        context.MainBuffer = "0.";

    public void EraseLast(CalculatorContext context) =>
        context.MainBuffer = "0";

    public void ClearMainBuffer(CalculatorContext context) =>
        context.CurrentState = new ReadyForInputState();

    public void ClearAll(CalculatorContext context) =>
        context.CurrentState = new ReadyForInputState();
}