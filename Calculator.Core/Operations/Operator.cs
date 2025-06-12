namespace Calculator.Core.Operations;

public class Operator:Operation
{
    public int Precedence { get; }

    public Operator(string name, Func<double[], double> function, int requiredArgs,int precedence) :
        base(name, function, requiredArgs)
    {
        Precedence = precedence;
    }
}
