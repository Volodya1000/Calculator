using Avalonia;
using Avalonia.ReactiveUI;
using Calculator.Avalonia.ViewModels;
using Calculator.Avalonia.Views;
using Calculator.Core;
using Calculator.Core.Interfaces;
using Calculator.Core.Operations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Calculator.Avalonia;

internal sealed class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
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
        var constants = new Dictionary<string, double>();

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
