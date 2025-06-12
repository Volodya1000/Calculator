using Calculator.Core.Interfaces;
using ReactiveUI;
using System.Globalization;
using System.Reactive;

namespace Calculator.Avalonia.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly IExpressionCalculator _calculator;

        public MainWindowViewModel(IExpressionCalculator calculator)
        {
            _calculator = calculator;

            ExecuteOperation = ReactiveCommand.Create(() =>
            {
                var result = _calculator.EvaluateExpression(Expression);
                if (result.IsSuccess)
                {
                    ShownValue = result.Value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    ShownValue = result.Error.Message;
                }
            });

            EnterSymbolCommand = ReactiveCommand.Create<string>(symbol =>
            {
                Expression += symbol;
            });

            ClearCommand = ReactiveCommand.Create(() =>
            {
                Expression = "";
                ShownValue = "0";
            });

            RemoveLastNumberCommand = ReactiveCommand.Create(() =>
            {
                if (!string.IsNullOrEmpty(Expression))
                {
                    string updatedExpression = _calculator.GetStringAfterErasingLastToken(Expression);
                    Expression = updatedExpression;
                }
            });

            EnterFunctionCommand = ReactiveCommand.Create<string>(function =>
            {
                Expression += function + "(";
            });
        }

        private string _expression = "";
        public string Expression
        {
            get => _expression;
            set => this.RaiseAndSetIfChanged(ref _expression, value);
        }

        private string _shownValue = "0";
        public string ShownValue
        {
            get => _shownValue;
            set => this.RaiseAndSetIfChanged(ref _shownValue, value);
        }

        public ReactiveCommand<Unit, Unit> ClearCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveLastNumberCommand { get; }
        public ReactiveCommand<Unit, Unit> ExecuteOperation { get; }
        public ReactiveCommand<string, Unit> EnterSymbolCommand { get; }
        public ReactiveCommand<string, Unit> EnterFunctionCommand { get; }
    }
}
