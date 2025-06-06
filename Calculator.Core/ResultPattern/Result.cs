using Calculator.Core.Error;

namespace Calculator.Core.ResultPattern;

public class Result<T>
{
    private readonly T? _value;

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("There is no value for failure");

            return _value!;
        }

        private init => _value = value;
    }

    public CalculatorException? Error { get; }

    private Result(T value)
    {
        _value = value;
        IsSuccess = true;
    }

    private Result(CalculatorException error)
    {
        if (error == null)
            throw new ArgumentException("Invalid error");
        Error = error;
        IsSuccess = false;
    }

    public static Result<T> Success(T value) => new Result<T>(value);

    public static Result<T> Failure(CalculatorException error) => new Result<T>(error);
}
