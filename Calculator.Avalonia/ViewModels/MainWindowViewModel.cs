namespace Calculator.Avalonia.ViewModels;

using System.Reactive;
using ReactiveUI;


public class MainWindowViewModel : ReactiveObject
{
    private string _shownValue = "0";
    private string _expressionBuffer ="2+2*(1-sin(30))";
    private bool _isRadians;

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

    public bool IsRadians
    {
        get => _isRadians;
        set
        {
            if (this.RaiseAndSetIfChanged(ref _isRadians, value))
            {
                if (value)
                    IsDegSelected = false;
            }
        }
    }

    private bool _isDegSelected = true;

    public bool IsDegSelected
    {
        get => _isDegSelected;
        set
        {
            if (this.RaiseAndSetIfChanged(ref _isDegSelected, value))
            {
                if (value)
                    IsRadians = false;
            }
        }
    }

    // --- Команды ---
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveLastNumberCommand { get; }
    public ReactiveCommand<Unit, Unit> AddDecimalPointCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleSignCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleDegRadCommand { get; }
    public ReactiveCommand<Unit, Unit> ExitCommand { get; }

    public ReactiveCommand<string, Unit> AddOperationCommand { get; }
    public ReactiveCommand<string, Unit> ExecuteOperation { get; }
    public ReactiveCommand<string, Unit> AddFunctionCommand { get; }
    public ReactiveCommand<string, Unit> AddConstantCommand { get; }

    public ReactiveCommand<string, Unit> AddNumberCommand { get; }
    public ReactiveCommand<string, Unit> AddParenthesisCommand { get; }
    

    // --- Конструктор ---
    public MainWindowViewModel()
    {
        // --- Инициализация команд ---
        ClearCommand = ReactiveCommand.Create(() => { });
        RemoveLastNumberCommand = ReactiveCommand.Create(() => { });
        AddDecimalPointCommand = ReactiveCommand.Create(() => { });
        ToggleSignCommand = ReactiveCommand.Create(() => { });
        ToggleDegRadCommand = ReactiveCommand.Create(() => { });
        ExitCommand = ReactiveCommand.Create(() => { });

        AddOperationCommand = ReactiveCommand.Create<string>(_ => { });
        ExecuteOperation = ReactiveCommand.Create<string>(_ => { });
        AddFunctionCommand = ReactiveCommand.Create<string>(_ => { });
        AddConstantCommand = ReactiveCommand.Create<string>(_ => { });
        AddNumberCommand = ReactiveCommand.Create<string>(_ => { });
    }
}