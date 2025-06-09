using Calculator.Core.ResultPattern;
using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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




    #region flags
    private bool MainBufferShowsResult = false;


    #endregion

    private double firstValue;

    private double secondValue;

    private string lastOperation = "";


    private readonly Calculator.Core.Calculator _executer;

    public CalculatorWithBuffer(Calculator.Core.Calculator calculator)
    {
        _executer = calculator;
    }


    public void EnterNumber(int number)
    {
        if (MainBuffer.Length == MAX_MAIN_BUFFER_LENGTH)
            return;

        if(lastOperation!="") //сразу показывать предворительный результат
        {
            double previewSecondValue= double.Parse(MainBuffer, CultureInfo.InvariantCulture);

            

            var previewResult = _executer.Call(lastOperation, firstValue, previewSecondValue);
            if (previewResult.IsSuccess)
            {
                MainBuffer = previewResult.Value.ToString().Replace(",", ".");
            }
            else
            {
                //ДОПИСАТЬ !!!!!
            }
        }


        if (MainBufferShowsResult)
        {
            MainBuffer = number.ToString(); //Когда отображается результат и вводится число, то оно перекрывает резуьтат
            MainBufferShowsResult = false;
            HistoryBuffer = "";
        }
        else if(MainBuffer=="0")
        {
            MainBuffer= number.ToString();
        }
        else
        {
            MainBuffer += number.ToString();
        }
    }

    public void EnterDot()
    {
        if (MainBufferShowsResult)
        {
            MainBuffer = "0.";
            MainBufferShowsResult = false;
        }
        else
        {
            if(!MainBuffer.Contains("."))
            {
                MainBuffer += ".";
            }

        }
    }

    public void EnterOperation(OperationType op)
    {
        if(MainBuffer.EndsWith("."))
        {
            MainBuffer = MainBuffer.Substring(0, MainBuffer.Length-2);//удаление последнего символа точка
        }

        firstValue = double.Parse(MainBuffer, CultureInfo.InvariantCulture);
        string firstValueWithDot = firstValue.ToString().Replace(",", ".");

        HistoryBuffer = firstValueWithDot + " "+ OperationHelper.GetSymbol(op);
        MainBuffer = firstValueWithDot;

        lastOperation = OperationHelper.GetSymbol(op).ToString();
    }

    /// <summary>
    /// Нажатие кнопки вычисления '='
    /// </summary>
    public void Execute()
    {
        if (MainBufferShowsResult)
            return;
        secondValue = double.Parse(MainBuffer, CultureInfo.InvariantCulture);
        string secondValueWithDot = firstValue.ToString().Replace(",", ".");
        HistoryBuffer += secondValueWithDot;

        var result = _executer.Call(lastOperation, firstValue,secondValue);
        if (result.IsSuccess)
        {
            MainBuffer = result.Value.ToString().Replace(",", ".");
            MainBufferShowsResult = true;
            lastOperation = "";
        }
        else
        {
            //ДОПИСАТЬ !!!!!
        }
    }


    /// <summary>
    ///На калькуляторе обозначается ←
    /// </summary>
    public void EraseLast()
    {
        if (MainBufferShowsResult) //нажатие ← не должно стирать результат
            return;

        if (MainBuffer.Length == 1) //стирая единственный символ всегда получаем 0
            MainBuffer = "0";
        else
            MainBuffer = MainBuffer[..^1];//удаление последнего символа из строки
    }

    /// <summary>
    /// На калькуляторе обозначается CE - Clear Entry
    /// </summary>
    public void ClearMainBuffer()
    {
        MainBuffer = "0";
    }

    /// <summary>
    /// На калькуляторе обозначается AC - All Clear
    /// </summary>
    public void ClearMainAndHistoryBufers()
    {
        MainBufferShowsResult = false;
        MainBuffer = "0";
        HistoryBuffer = "0";
    }

    #region Math Functions
    #endregion
}
