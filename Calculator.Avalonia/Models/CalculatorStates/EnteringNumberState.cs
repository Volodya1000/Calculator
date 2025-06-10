using System;

namespace Calculator.Avalonia.Models.CalculatorStates;

public class EnteringNumberState : CalculatorStateBase
{
    public EnteringNumberState(CalculatorContext context) : base(context) { }

    public override void EnterNumber(char digit)
    {
        if (Context.CurrentInput == "0") Context.CurrentInput = digit.ToString();
        else Context.CurrentInput += digit;
    }


    public override void EnterDot()
    {
        if (!Context.CurrentInput.Contains(".")) Context.CurrentInput += ".";
    }

    public override void EnterUnaryPreffixOperation(string op)
    {
        if (!Context.TryParseInput(out var x)) throw new FormatException();
        //Context.SetInput(ApplyUnaryPrefix(op, x));
    }

    public override void EnterUnaryPostfixOperation(string op)
    {
        if (!Context.TryParseInput(out var x)) throw new FormatException();
        //Context.SetInput(ApplyUnaryPostfix(op, x));
    }


    public void EraseLast(CalculatorContext context)
    {
        //if (context.MainBuffer.Length > 1)
        //    context.MainBuffer = context.MainBuffer[..^1];
        //else
        //    context.MainBuffer = "0";
    }

    public void Clear(CalculatorContext context)
    {
        //context.MainBuffer = "0";
        //context.HistoryBuffer = "";
        //context.CurrentValue = null;
        //context.PendingOperation = "";
        //context.PendingOperationType = null;
        //context.CurrentState = new ReadyForInputState();
    }
}