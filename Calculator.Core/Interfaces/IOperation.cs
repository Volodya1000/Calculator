namespace Calculator.Core.Interfaces;

public interface IOperation
{
    double Call(params double[] args);
}
