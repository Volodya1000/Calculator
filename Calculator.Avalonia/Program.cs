using Avalonia;
using Avalonia.ReactiveUI;
using Calculator.Avalonia.Views;
using Calculator.Core;
using Calculator.Core.Builders;
using Calculator.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Avalonia;

internal sealed class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    [STAThread]
    public static void Main(string[] args)
    {
        // Configure DI container
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var constantsDictionary = new ConstantsBuilder().AddMathConstants().Build();

        services.AddSingleton(constantsDictionary);

        // Зарегистрировать калькулятор
        var calculator = new Calculator.Core.Calculator().AddAssembly("Calculator.Core.dll");
        services.AddSingleton(calculator);

        services.AddSingleton<IExpressionCalculator, ExpressionsEvaluatorFacade>();

        services.AddTransient<MainWindow>();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
