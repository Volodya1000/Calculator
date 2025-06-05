using Calculator.Core.Error;
using Calculator.Core.Interfaces;

namespace Calculator.Core;

public class Operation : IOperation
{
    private readonly Func<double[], double> _function;
    private readonly int _requiredArgs;
    private readonly string _name;

    public Operation(string name, Func<double[], double> function, int requiredArgs)
    {
        _name = name;
        _function = function ?? throw new ArgumentNullException(nameof(function));
        _requiredArgs = requiredArgs;
    }

    public double Call(params double[] args)
    {
        if (args == null) throw new ArgumentNullException(nameof(args));
        if (args.Length < _requiredArgs)
            throw new CalculatorException(
                CustomError.InsufficientArguments(_name, _requiredArgs, args.Length));

        ValidateArguments(args);
        return ExecuteFunction(args);
    }

    private void ValidateArguments(double[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (double.IsNaN(args[i]))
                throw new CalculatorException(
                    CustomError.InvalidArgument(_name, "Cannot be NaN", i + 1));

            if (double.IsInfinity(args[i]))
                throw new CalculatorException(
                    CustomError.InvalidArgument(_name, "Cannot be infinity", i + 1));
        }
    }

    private double ExecuteFunction(double[] args)
    {
        try
        {
            double result = _function(args);

            if (double.IsNaN(result))
                throw new CalculatorException(
                    CustomError.CalculationError(_name, "Result is not a number (NaN)"));

            if (double.IsInfinity(result))
                throw new CalculatorException(
                    CustomError.CalculationError(_name, "Result is infinite"));

            return result;
        }
        catch (CalculatorException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new CalculatorException(
                CustomError.CalculationError(_name, $"Operation failed: {ex.Message}"), ex);
        }
    }
}