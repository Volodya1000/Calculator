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
        // Инициализация токенизатора с константой "e" и функцией "exp"
        var tokenizer = new ExpressionTokenizer(
            functions: new List<string> { "sin", "cos", "exp", "log" },
            constants: new List<string> { "pi", "e" }
        );

        // Добавляем специальный тестовый пример
        var testExpressions = new[]
        {
            "e + exp(e)",  // Проверяем различение e и exp
            "3*e - exp(2)",
            "exp(e^pi)",
            "expert",      // Проверяем, чтобы exp не находился внутри других слов
            "some_variable"
        };

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