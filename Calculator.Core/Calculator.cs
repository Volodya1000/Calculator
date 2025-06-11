using Calculator.Core.Exceptions.OperationExceptions;
using Calculator.Core.Interfaces;
using Calculator.Core.ResultPattern;

namespace Calculator.Core;

public class Calculator
{
    private readonly Dictionary<string, IOperation> _operations;

    public Calculator(Dictionary<string, IOperation> operations)
    {
        _operations = new Dictionary<string, IOperation>(
            operations,
            StringComparer.OrdinalIgnoreCase);
    }

    public Result<double> Call(string operationName, params double[] args)
    {
        if (!_operations.TryGetValue(operationName, out var operation))
            return Result<double>.Failure(new OperationNotFoundException(operationName));

        try
        {
            return Result<double>.Success(operation.Call(args));
        }
        catch (Exception ex)
        {
            return Result<double>.Failure(ex);
        }
    }

    public bool OperationExists(string operation) => _operations.ContainsKey(operation);

    public IEnumerable<string> GetAvailableOperations()
    {
        return _operations.Keys.OrderBy(k => k);
    }
}