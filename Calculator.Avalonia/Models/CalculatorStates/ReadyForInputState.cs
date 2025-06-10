using Calculator.Avalonia.Models.Interfaces;

namespace Calculator.Avalonia.Models.CalculatorStates;

public class ReadyForInputState : CalculatorStateBase
{
    public ReadyForInputState(CalculatorContext context) : base(context) { }

    public override void EnterNumber(char number)
    {
        Context.CurrentInput = number.ToString();
        Context.TransitionTo(new EnteringNumberState(Context));

    }

    public override void EnterDot()
    {
        Context.CurrentInput = "0.";
        Context.TransitionTo(new EnteringNumberState(Context));
    }

    public override void EnterConstant(ConstantType constant)
    {
        var symbol = ConstantFactory.GetSymbol(constant);
        Context.CurrentInput = symbol;
        Context.TransitionTo(new EnteringNumberState(Context));
    }



    public override void EnterBinaryOperation(string op)
    {
        Context.PreviousValue = 0;
        Context.BinaryOperation = op;
        //Context.TransitionTo(new BinaryOperationPendingState(Context));

    }

    public override void EnterUnaryPreffixOperation(string op)
    {
        //На экране в буфере должен исчезнуть ноль и отобразиться название операции и (
    }

    public override void EnterUnaryPostfixOperation(string op)
    {
        //Вычисление унарной операции от начального числа 0
    }


    public override void Execute() 
    {
        //Ничего не делаем 

        //Нужно ли переопределять метод ???
    }

    public override void EraseLast()
    {
        var s = Context.CurrentInput;
        Context.CurrentInput = s.Length > 1 ? s[..^1] : "0";
    }




}
