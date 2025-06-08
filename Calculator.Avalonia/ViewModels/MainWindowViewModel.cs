using Calculator.Core;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Reactive;

namespace Calculator.Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly Calculator.Core.Calculator _calculator;

    private double _firstValue;
    


    #region Comand definition

    //int - тип входного параметра, а Unit - указывает что команда не возвращает резульат.
    //Тоесть как void Но void это ключевое слово, а не тип, поэтому тип Unit
    public ReactiveCommand<int, Unit> AddNumberCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveLastNumberComand { get; }
    #endregion

    public MainWindowViewModel()
    {
        _calculator = App.ServiceProvider.GetService<Calculator.Core.Calculator>();

        AddNumberCommand = ReactiveCommand.Create<int>(AddNumber);
        RemoveLastNumberComand = ReactiveCommand.Create(RemoveLastNumber); // так как RemoveLastNumber не принимает аргуметов, то угловые скобки не нужны
    }


    #region binging properties

    private double _secondValue;
    public double ShownValue
    {
        get => _secondValue;
        set => this.RaiseAndSetIfChanged(ref _secondValue, value);
    }

    private string _operation = "";
    public string Operation
    {
        get => _operation;
        set => this.RaiseAndSetIfChanged(ref _operation, value);
    }

    #endregion

    #region ComandImplimentation

    public void Exit()
    {
        Environment.Exit(0);
    }

    private void AddNumber(int value)
    {

        ShownValue = ShownValue * 10 + value;
    }

    private void RemoveLastNumber()
    {
        ShownValue = Math.Floor(ShownValue / 10); 
    }


    private void ExecuteOperation(string operation)
    {
        if (_calculator.OperationExists(operation))
        {
            var result = _calculator.Call(operation, _secondValue, _firstValue);
        
            if(result.IsSuccess)
            {
                _firstValue= result.Value;
                ShownValue = 0;
            }
        }
        if (operation == "=")
        {
            ShownValue = _firstValue;
            _operation ="+";
            _firstValue = 0;
        }
        else
        {
            _operation = operation;
        }
    }
    #endregion
}
