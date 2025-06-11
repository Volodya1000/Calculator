namespace Calculator.Avalonia.ViewModels;

using Calculator.Core;
using Calculator.Core.Interfaces;
using ReactiveUI;
using System;
using System.Reactive;


public class MainWindowViewModel : ReactiveObject
{
    private readonly IExpressionCalculator _calculator;

    public MainWindowViewModel(IExpressionCalculator calculator)
    {
        _calculator = calculator;

        ClearCommand = ReactiveCommand.Create(() =>
        {
            ExpressionBuffer = "";
            ShownValue = "0";
        });

        RemoveLastNumberCommand = ReactiveCommand.Create(() =>
        {
            if (ExpressionBuffer.Length > 0)
            {
                ExpressionBuffer = ExpressionBuffer[..^1];
            }
        });

        ToggleDegRadCommand = ReactiveCommand.Create(() =>
        {
            IsDegSelected = !IsDegSelected;
        });

        ExecuteOperation = ReactiveCommand.Create(() =>
        {
            try
            {
                ShownValue = _calculator.EvaluateExpression(ExpressionBuffer).Value.ToString();
            }
            catch (Exception ex)
            {
                ShownValue = "Error";
            }
        });

        EnterSymbolCommand = ReactiveCommand.Create<string>(symbol =>
        {
            ExpressionBuffer += symbol;
        });

        EnterFunctionCommand = ReactiveCommand.Create<string>(function =>
        {
            ExpressionBuffer += function + "(";
        });
    }

    private string _shownValue = "0";
    private string _expressionBuffer ="";

    public string ShownValue
    {
        get => _shownValue;
        set => this.RaiseAndSetIfChanged(ref _shownValue, value);
    }

    public string ExpressionBuffer
    {
        get => _expressionBuffer;
        set => this.RaiseAndSetIfChanged(ref _expressionBuffer, value);
    }

    private bool _isDegSelected = true;

    public bool IsDegSelected
    {
        get => _isDegSelected;
        set => this.RaiseAndSetIfChanged(ref _isDegSelected, value);
    }

    // --- Команды ---
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveLastNumberCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleDegRadCommand { get; }
    public ReactiveCommand<Unit, Unit> ExecuteOperation { get; }

    public ReactiveCommand<string, Unit> EnterSymbolCommand { get; }
    public ReactiveCommand<string, Unit> EnterFunctionCommand { get; }

}