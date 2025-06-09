using Calculator.Avalonia.Models.Interfaces;

namespace Calculator.Avalonia.Models.CalculatorStates;

public class EnteringNumberState : ICalculatorState
{
    public void EnterNumber(CalculatorContext context, int number)
    {
        CalculatorUtils.AppendNumber(context, number);
    }

    public void EnterOperation(CalculatorContext context, OperationType operation)
    {
        context.CurrentValue = double.Parse(context.MainBuffer);
        context.PendingOperation = OperationHelper.GetSymbol(operation);
        context.PendingOperationType = operation;
        context.HistoryBuffer = $"{context.CurrentValue} {context.PendingOperation}";
        context.CurrentState = new OperationSelectedState(); // Переход в состояние выбора операции
    }

    public void ExecuteUnary(CalculatorContext context, OperationType operation)
    {
        var value = double.Parse(context.MainBuffer);
        var result = context.CoreCalculator.Call(operation, value);
        if (result.IsSuccess)
        {
            context.MainBuffer = result.Value.ToString();
            context.HistoryBuffer = $"{OperationHelper.GetSymbol(operation)}({value})";
            context.CurrentValue = result.Value;
            context.CurrentState = new ResultDisplayedState(); // Переход в состояние отображения результата
        }
    }

    public void ExecuteBinary(CalculatorContext context)
    {
        context.CurrentValue = double.Parse(context.MainBuffer);
        context.HistoryBuffer = context.MainBuffer;
        context.MainBuffer = context.CurrentValue.ToString();
        context.CurrentState = new ResultDisplayedState(); // Переход в состояние отображения результата
    }

    public void EnterDot(CalculatorContext context)
    {
        if (!context.MainBuffer.Contains("."))
            context.MainBuffer += ".";
    }

    public void EraseLast(CalculatorContext context)
    {
        if (context.MainBuffer.Length > 1)
            context.MainBuffer = context.MainBuffer[..^1];
        else
            context.MainBuffer = "0";
    }

    public void Clear(CalculatorContext context)
    {
        context.MainBuffer = "0";
        context.HistoryBuffer = "";
        context.CurrentValue = null;
        context.PendingOperation = "";
        context.PendingOperationType = null;
        context.CurrentState = new ReadyForInputState();
    }
}