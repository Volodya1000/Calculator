using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Avalonia.Models;

public enum ConstantType
{
    Pi,
    E
}

public static class ConstantFactory
{
    internal static readonly Dictionary<ConstantType, string> Symbols = new()
    {
        { ConstantType.Pi, "pi" },
        { ConstantType.E, "e" }
    };

    public static string GetSymbol(ConstantType c) => Symbols[c];

    public static bool IsSymbol(string s) => Symbols.Values.Contains(s);

    public static double GetValue(string s)
    {
        var kv = Symbols.FirstOrDefault(kv => kv.Value == s);
        return kv.Key switch
        {
            ConstantType.Pi => Math.PI,
            ConstantType.E => Math.E,
            _ => throw new InvalidOperationException($"Unknown constant '{s}'")
        };
    }
}
