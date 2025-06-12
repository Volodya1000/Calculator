namespace Calculator.Core.Interfaces;

public interface IInfexOperator:IOperation
{
    public int Priority { get; }
}
