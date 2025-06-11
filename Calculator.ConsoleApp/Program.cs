using Calculator.ConsoleApp;
using Calculator.Core.ExpressionEvaluator.Tokinezation;
using Calculator.Core.Operations;
//Сразу использую DependencyInjection чтоб на будущее было удобнее переделать например в AvaloniaUI
using Microsoft.Extensions.DependencyInjection;

//var services = new ServiceCollection();

//var operations = new OperationsBuilder().AddAll().Build();

//services.AddSingleton<Calculator.Core.Calculator>(provider =>
//    new Calculator.Core.Calculator(operations));

//services.AddSingleton<ConsoleProcessor>();
//var serviceProvider = services.BuildServiceProvider();

//var consoleProcessor = serviceProvider.GetRequiredService<ConsoleProcessor>();
//consoleProcessor.Run();

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Инициализация токенизатора
        var tokenizer = new ExpressionTokenizer(
            functions: new List<string> { "sin", "cos", "tan", "sqrt", "log" },
            constants: new List<string> { "pi", "e" }
        );

        // Список тестовых выражений
        var testExpressions = new[]
        {
            "3 + 4 * 2 / (1 - 5)^2",
            "sin(pi/2) + cos(0)",
            "sqrt(16) * log(e^3)",
            "a * x^2 + b * x + c",
            "invalid @ symbol",
            "2.5 * (3 + e) / tan(0.5)"
        };

        // Тестирование каждого выражения
        foreach (var expr in testExpressions)
        {
            TestExpression(tokenizer, expr);
        }
    }

    static void TestExpression(ExpressionTokenizer tokenizer, string expression)
    {
        Console.WriteLine($"\nТестируем выражение: {expression}");
        Console.WriteLine(new string('-', 50));

        try
        {
            var tokens = tokenizer.Tokenize(expression);

            Console.WriteLine($"{"Тип",-20} | {"Значение",-10} | Позиция");
            Console.WriteLine(new string('-', 40));

            foreach (var token in tokens)
            {
                Console.WriteLine($"{token.Type,-20} | {token.Value,-10} | [{token.Start}-{token.End}]");
            }
        }
        catch (ArgumentException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка: {ex.Message}");
            Console.ResetColor();
        }
    }
}