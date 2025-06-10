using Calculator.Avalonia.Models.Interfaces;
using System;

namespace Calculator.Avalonia.Models.CalculatorStates;

public abstract class CalculatorStateBase : ICalculatorState
{
    protected readonly CalculatorContext Context;

    protected CalculatorStateBase(CalculatorContext context) => Context = context;

    public virtual void EnterNumber(char digit) => throw new InvalidOperationException("Недоступно в текущем состоянии.");
    public virtual void EnterDot() => throw new InvalidOperationException("Недоступно в текущем состоянии.");
    public virtual void EnterConstant(ConstantType constant) => throw new InvalidOperationException();
    public virtual void EnterBinaryOperation(string op) => throw new InvalidOperationException("Недоступно в текущем состоянии.");
    public virtual void EnterUnaryPreffixOperation(string op) => throw new InvalidOperationException("Недоступно в текущем состоянии.");
    public virtual void EnterUnaryPostfixOperation(string op) => throw new InvalidOperationException("Недоступно в текущем состоянии.");
    public virtual void Execute() => throw new InvalidOperationException("Недоступно в текущем состоянии.");
    public virtual void EraseLast() => throw new InvalidOperationException();
}
