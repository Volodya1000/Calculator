namespace Calculator.Core.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class OperationAttribute : Attribute
{
    public string Name { get; }

    public OperationAttribute(string name)
        => Name = name;
}
