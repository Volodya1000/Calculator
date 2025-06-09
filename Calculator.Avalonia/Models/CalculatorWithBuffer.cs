using Calculator.Core.ResultPattern;
using DynamicData.Kernel;
using System;
using System.Data;
using System.Globalization;

namespace Calculator.Avalonia.Models;

public class CalculatorWithBuffer
{
    public static readonly int MAX_MAIN_BUFFER_LENGTH = 9;


    /// <summary>
    /// Отображает историю последних вычислений
    /// </summary>
    public string HistoryBuffer { get; private set; } = "";

    //public string HistoryBuffer => MakeHistoryBuffer();

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

        if (MainBufferShowsResult)
        {
            MainBuffer = number.ToString(); //Когда отображается результат и вводится число, то оно перекрывает резуьтат
            MainBufferShowsResult = false;
            HistoryBuffer = "";
        }

        //обрезанное число чтоб после конкатанации длина не привысила максимальную

        string numberStr= number.ToString(); 
        int canTake = MAX_MAIN_BUFFER_LENGTH - MainBuffer.Length;
        string cuttedNumber = numberStr.Substring(0,Math.Min(canTake, numberStr.Length));
        if (MainBuffer=="0"|| lastOperation!="")
        {
            MainBuffer= cuttedNumber;
        }
        else
        {
            MainBuffer += cuttedNumber;
        }
    }

    public void EnterDot()
    {
        if (MainBufferShowsResult)
        {
            MainBuffer = "0.";
            HistoryBuffer = "";
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

        if (MainBufferShowsResult)
        {

        }
        else if (lastOperation == "")
        {
            firstValue = MainBufferToDouble();
            string firstValueWithDot = firstValue.ToString().Replace(",", ".");

            HistoryBuffer = firstValueWithDot + " " + OperationHelper.GetSymbol(op);
            MainBuffer = firstValueWithDot;
            lastOperation = OperationHelper.GetSymbol(op).ToString();

        }
        else
        {
            if (MainBufferToDouble() == firstValue) //сначала была нажата одна операция, а потом другая и нужно поменять в HistoryBuffer символ операции
            {
                HistoryBuffer = firstValue.ToString() + OperationHelper.GetSymbol(op);
                lastOperation = OperationHelper.GetSymbol(op).ToString();
            }
            //в HistoryBuffer есть выражение из операции и двух операндов и его уже можно вычислить,
            //но ползователь не нажал = а выбрал новую операцию, поэтому нужно вычислить предыдущее
            else
            {
                secondValue = MainBufferToDouble();
                var result = _executer.Call(lastOperation, firstValue, secondValue);
                if (result.IsSuccess)
                {
                    string resultWithDot = result.Value.ToString().Replace(",", ".");
                    HistoryBuffer = resultWithDot + OperationHelper.GetSymbol(op);
                    MainBuffer = resultWithDot;
                    MainBufferShowsResult = false; //возможжно лишняя строка
                    lastOperation = OperationHelper.GetSymbol(op).ToString();
                    firstValue = result.Value;
                }
                else
                {
                    // Дописать
                }
            }
        }
    }

    /// <summary>
    /// Нажатие кнопки вычисления '='
    /// </summary>
    public void Execute()
    {
        if (MainBufferShowsResult)
            return;
        else if(lastOperation == "")
        {
            MainBufferShowsResult = true;
            return;
        }

        secondValue = MainBufferToDouble();
        string secondValueWithDot = secondValue.ToString().Replace(",", ".");
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
        HistoryBuffer = "";
        lastOperation = "";
    }

    #region Math Functions
    #endregion

    #region helpers

    private double MainBufferToDouble() => double.Parse(MainBuffer, CultureInfo.InvariantCulture);

    #endregion
}
