using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Calculator.AvaloniaUI.ViewModels;
using Calculator.AvaloniaUI.Views;
using Calculator.Core.Operations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Calculator.AvaloniaUI;

public partial class App : Application
{
    public static  IServiceProvider ServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        var operations = new OperationsBuilder().AddAll().Build();

        collection.AddSingleton<Calculator.Core.Calculator>(provider =>
            new Calculator.Core.Calculator(operations));


        ServiceProvider = collection.BuildServiceProvider();


        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

}