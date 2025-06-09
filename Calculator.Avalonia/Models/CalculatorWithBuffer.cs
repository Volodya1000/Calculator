using System;

namespace Calculator.Avalonia.Models;

public class CalculatorWithBuffer
{
    public static readonly int MAX_MAIN_BUFFER_LENGTH = 9;

    /// <summary>
    /// Отображает историю последних вычислений
    /// </summary>
    public string HistoryBuffer { get; private set; } = "";

    /// <summary>
    /// Отображает резултат или введённый операнд
    /// </summary>
    public string MainBuffer { get; private set; } = "0";


    public void EnterNumber(int number)
    {
        throw new NotImplementedException();
    }

    public void EnterDot()
    { 
        throw new NotImplementedException(); 
    }

    public void EnterOperation(OperationType op)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Нажатие кнопки вычисления '='
    /// </summary>
    public void Execute()
    {
        throw new NotImplementedException();
    }


    /// <summary>
    ///На калькуляторе обозначается ←
    /// </summary>
    public void EraseLast()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// На калькуляторе обозначается CE - Clear Entry
    /// </summary>
    public void ClearMainBuffer()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// На калькуляторе обозначается AC - All Clear
    /// </summary>
    public void ClearMainAndHistoryBufers()
    {
        throw new NotImplementedException();
    }

    #region Math Functions
    #endregion
}
