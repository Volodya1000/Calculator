using ReactiveUI;
using System;
using System.Reactive;

namespace Calculator.Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private double _firstValue;
    private double _secondValue;


    #region Comand definition

    //int - тип входного параметра, а Unit - указывает что команда не возвращает резульат.
    //Тоесть как void Но void это ключевое слово, а не тип, поэтому тип Unit
    public ReactiveCommand<int, Unit> AddNumberCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveLastNumberComand { get; }
    #endregion

    public MainWindowViewModel()
    {
        AddNumberCommand = ReactiveCommand.Create<int>(AddNumber);
        RemoveLastNumberComand = ReactiveCommand.Create(RemoveLastNumber); // так как RemoveLastNumber не принимает аргуметов, то угловые скобки не нужны
    }


    #region ComandImplimentation

    public double ShownValue
    {
        get => _secondValue;
        set => this.RaiseAndSetIfChanged(ref _secondValue, value);
    }

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
    #endregion
}
