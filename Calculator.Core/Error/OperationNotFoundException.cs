
namespace Calculator.Core.Error;

public class OperationNotFoundException : CalculatorException
{
    public OperationNotFoundException(string operation) : base(operation,$"Operation '{operation}' not found")
    {
        Operation= operation;
    }
}
