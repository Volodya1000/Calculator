namespace Calculator.Core.Interfaces;

public interface IInfexOperator:IOperation
{
    public new int ArgsCount { get=>2;}

    public int Priority { get; }

    public bool RightAssociativity {  get; }
}
