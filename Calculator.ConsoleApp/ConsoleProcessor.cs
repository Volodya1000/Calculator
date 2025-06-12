using Calculator.Core;
using Calculator.Core.ResultPattern;
using System.Globalization;

namespace Calculator.ConsoleApp;

public class ConsoleProcessor
{
    private readonly ConsoleColor _errorColor = ConsoleColor.Red;
    private readonly ConsoleColor _resultColor = ConsoleColor.Green;
    private readonly ConsoleColor _messageColor = ConsoleColor.White;

    private readonly Core.OneOperationCalculatorFacade _calculatorFacade;

    public ConsoleProcessor(Core.OneOperationCalculatorFacade calculatorFacade)
    {
        _calculatorFacade = calculatorFacade ?? throw new ArgumentNullException(nameof(calculatorFacade)); ;
    }

    public void Run()
    {
        Console.WriteLine("For exit enter \"exit\", for operations list enter \"help\"");
        while (true)
        {
            Console.ForegroundColor = _messageColor;
            Console.WriteLine("op:");

            string? operationInput = Console.ReadLine()?.Trim().ToLower();

            if (String.IsNullOrEmpty(operationInput))
                continue;

            if (operationInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (operationInput.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                DisplayAvailableOperations();
                continue;
            }

            if (!_calculatorFacade.OperationExists(operationInput))
            {
                Console.ForegroundColor = _errorColor;
                Console.WriteLine($"Operation \"{operationInput}\" does not exists");
                continue;
            }

            Console.ForegroundColor = _messageColor;
            Console.WriteLine("args:");

            string? argInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(argInput))
            {
                Console.ForegroundColor = _errorColor;
                Console.WriteLine("No arguments provided");
                continue;
            }

            if (!ParseArguments(argInput, out double[] doubleArgs))
            {
                Console.ForegroundColor = _errorColor;
                Console.WriteLine("Arguments should be double type");
                continue;
            }
            bool succes = ParseArguments(argInput, out doubleArgs);

            var result = _calculatorFacade.Call(operationInput, doubleArgs);

            DisplayResult(result);
        }
    }

    private void DisplayResult(Result<double> result)
    {
        if (result.IsSuccess)
        {
            Console.ForegroundColor = _resultColor;
            Console.WriteLine($"Result: {result.Value}");
        }
        else
        {
            Console.ForegroundColor = _errorColor;
            Console.WriteLine($"Calculator error: {result?.Error?.Message}");
        }
    }

    private bool ParseArguments(string argInput, out double[] doubleArgs)
    {
        string[] stringArgs = argInput.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        doubleArgs = new double[stringArgs.Length];

        for (int i = 0; i < stringArgs.Length; i++)
        {
            if (!double.TryParse(stringArgs[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedValue))
            {
                doubleArgs = Array.Empty<double>(); 
                return false;
            }
            doubleArgs[i] = parsedValue;
        }

        return true;
    }

    private void DisplayAvailableOperations()
    {
        Console.ForegroundColor = _messageColor;
        Console.WriteLine("Available operations:");

        foreach (var operation in _calculatorFacade.GetAvailableOperations())
        {
            Console.WriteLine($"  {operation}");
        }

        Console.ResetColor();
    }

}
