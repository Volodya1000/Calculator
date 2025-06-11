using Calculator.Core.Exceptions;
using Calculator.Core.Exceptions.OperationExceptions;
using Calculator.Core.Interfaces;

namespace Calculator.Core;

public class Operation : IOperation
{
    private readonly Func<double[], double> _function;
    public int ArgsCount { get; init;}

    //Имя позволяет иметь различные ключи для вызова операции и для её описания, напрмер в исключениях. Я бы добавил имя и RequiredArgs в IOperation
    private readonly string _name;

    public Operation(string name, Func<double[], double> function, int requiredArgs)
    {
        _name = name;
        _function = function ?? throw new ArgumentNullException(nameof(function));
        ArgsCount = requiredArgs;
    }

    public double Call(params double[] args)
    {
        if (args == null) throw new ArgumentNullException(nameof(args));
        if (args.Length < ArgsCount)
            throw new InsufficientArgumentsException(_name, ArgsCount, args.Length);

        ValidateArguments(args);
        return ExecuteFunction(args);
    }

    private void ValidateArguments(double[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (double.IsNaN(args[i]))
                throw new InvalidCalculatorArgumentException(_name, "Cannot be NaN", args[i], i + 1);

            if (double.IsInfinity(args[i]))
                throw  new InvalidCalculatorArgumentException(_name, "Cannot be infinity", args[i], i + 1);
        }
    }

    private double ExecuteFunction(double[] args)
    {
        try
        {
            double result = _function(args);

            if (double.IsNaN(result))
                throw new ExecutingOperationException(_name, "Result is not a number (NaN)");

            if (double.IsInfinity(result))
                throw new ExecutingOperationException(_name, "Result is infinite");

            return result;
        }
        catch (InvalidCalculatorArgumentException ex)
        {
            throw new InvalidCalculatorArgumentException(_name, ex.Message,ex.Arg,ex.ArgIndex);
        }
        catch (OperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ExecutingOperationException(_name, $"Operation failed: {ex.Message}");
        }
    }
}