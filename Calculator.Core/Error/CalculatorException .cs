namespace Calculator.Core.Error;

public class CalculatorException : Exception
{
    public ErrorType Type { get; }

    public CalculatorException(ErrorType type, string message) : base(message)
    {
        Type = type;
    }

    public CalculatorException(ErrorType type, string message, Exception inner)
        : base(message, inner)
    {
        Type = type;
    }


    public static CalculatorException OperationNotFound(string operation)
        => new(ErrorType.OperationNotFound, $"Operation '{operation}' not found");

    public static CalculatorException InsufficientArguments(string operation, int expected, int actual)
    => new(ErrorType.InsufficientArguments,
        $"Operation '{operation}' requires {expected} arguments, provided {actual}");

    public static CalculatorException InvalidArgument(string operation, string problem, int? position = null)
        => new(ErrorType.InvalidArgument,
            position.HasValue
                ? $"{operation}: {problem} (argument {position})"
                : $"{operation}: {problem}");

    public static CalculatorException CalculationError(string operation, string problem)
        => new(ErrorType.CalculationError, $"{operation}: {problem}");

}

