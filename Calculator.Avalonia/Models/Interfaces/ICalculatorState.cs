namespace Calculator.Avalonia.Models.Interfaces;

public interface ICalculatorState
{
    void EnterNumber(char digit);
    void EnterDot();
    void EnterConstant(ConstantType constant);
    void EnterBinaryOperation(string op);
    void EnterUnaryPreffixOperation(string op);
    void EnterUnaryPostfixOperation(string op);
    void Execute();
    void EraseLast();
}


