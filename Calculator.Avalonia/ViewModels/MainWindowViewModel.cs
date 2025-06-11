using Calculator.Core.Interfaces;
using ReactiveUI;
using System.Text;
using System.Reactive;

namespace Calculator.Avalonia.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly IExpressionCalculator _calculator;

    public MainWindowViewModel(IExpressionCalculator calculator)
    {
        _calculator = calculator;

        ClearCommand = ReactiveCommand.Create(() =>
        {
            ExpressionBuilder.Clear();
            ShownValue = "0";
            this.RaisePropertyChanged(nameof(Expression));
        });

        RemoveLastNumberCommand = ReactiveCommand.Create(() =>
        {
            if (ExpressionBuilder.Length > 0)
            {
                ExpressionBuilder.Length--;
                this.RaisePropertyChanged(nameof(Expression));
            }
        });

        ToggleDegRadCommand = ReactiveCommand.Create(() =>
        {
            IsDegSelected = !IsDegSelected;
        });

        ExecuteOperation = ReactiveCommand.Create(() =>
        {
            var result = _calculator.EvaluateExpression(ExpressionBuilder.ToString());
            if (result.IsSuccess)
                ShownValue = result.Value.ToString();
            else
                ShownValue = result.Error.Message;
        });

        EnterSymbolCommand = ReactiveCommand.Create<string>(symbol =>
        {
            ExpressionBuilder.Append(symbol);
            this.RaisePropertyChanged(nameof(Expression));
        });

        EnterFunctionCommand = ReactiveCommand.Create<string>(function =>
        {
            ExpressionBuilder.Append(function).Append('(');
            this.RaisePropertyChanged(nameof(Expression));
        });
    }

    private StringBuilder ExpressionBuilder { get; } = new StringBuilder();

    // Для привязки к интерфейсу
    public string Expression => ExpressionBuilder.ToString();

    private string _shownValue = "0";
    private bool _isDegSelected = true;

    public string ShownValue
    {
        get => _shownValue;
        set => this.RaiseAndSetIfChanged(ref _shownValue, value);
    }

    public bool IsDegSelected
    {
        get => _isDegSelected;
        set => this.RaiseAndSetIfChanged(ref _isDegSelected, value);
    }

    public ReactiveCommand<Unit, Unit> ClearCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveLastNumberCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleDegRadCommand { get; }
    public ReactiveCommand<Unit, Unit> ExecuteOperation { get; }

    public ReactiveCommand<string, Unit> EnterSymbolCommand { get; }
    public ReactiveCommand<string, Unit> EnterFunctionCommand { get; }
}