namespace Calculator.Core.Error;

public record CustomError(ErrorType Type, string Message)
{
    public static readonly CustomError None = new(ErrorType.None, string.Empty);

    public static CustomError OperationNotFound(string operation)
        => new(ErrorType.OperationNotFound, $"Operation '{operation}' not found");

    public static CustomError InsufficientArguments(string operation, int expected, int actual)
        => new(ErrorType.InsufficientArguments,
            $"Operation '{operation}' requires {expected} arguments, provided {actual}");

    public static CustomError InvalidArgument(string operation, string problem, int? position = null)
        => new(ErrorType.InvalidArgument,
            position.HasValue
                ? $"{operation}: {problem} (argument {position})"
                : $"{operation}: {problem}");

    public static CustomError CalculationError(string operation, string problem)
        => new(ErrorType.CalculationError, $"{operation}: {problem}");
}
