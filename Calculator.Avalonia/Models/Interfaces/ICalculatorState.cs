namespace Calculator.Avalonia.Models.Interfaces;

public interface ICalculatorState
{
    void EnterNumber(CalculatorContext context, int number);
    void EnterOperation(CalculatorContext context, OperationType operation);
    //void ExecuteUnary(CalculatorContext context, OperationType operation);
    void ExecuteBinary(CalculatorContext context);
    void EnterDot(CalculatorContext context);
    void EraseLast(CalculatorContext context);
    
    void ClearMainBuffer(CalculatorContext context); // CE
    void ClearAll(CalculatorContext context);        // AC
}