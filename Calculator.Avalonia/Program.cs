using Avalonia;
using Avalonia.ReactiveUI;
using Calculator.Avalonia.Views;
using Calculator.Core;
using Calculator.Core.Interfaces;
using Calculator.Core.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
        var operations = new OperationsBuilder().AddAll().Build();
        var constants = new ConstantsBuilder().AddMathConstants().Build();

        services.AddTransient<IExpressionCalculator>(provider => new ExpressionsCalculatorFacade(operations, constants));
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
