namespace Calculator.Core.Builders;

public class ConstantsBuilder
{
    private readonly Dictionary<string, double> _constants = new(StringComparer.OrdinalIgnoreCase);

    public ConstantsBuilder AddConstant(string key, double value)
    {
        _constants.TryAdd(key, value);
        return this;
    }

    public ConstantsBuilder AddMathConstants()
    {
        return this
            .AddConstant("pi", Math.PI)
            .AddConstant("e", Math.E);
    }

    public Dictionary<string, double> Build()
    {
        return new Dictionary<string, double>(_constants, StringComparer.OrdinalIgnoreCase);
    }
}