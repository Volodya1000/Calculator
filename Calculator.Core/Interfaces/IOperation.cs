namespace Calculator.Core.Interfaces;

public interface IOperation
{
    public int ArgsCount { get; }
    double Call(params double[] args);
}
