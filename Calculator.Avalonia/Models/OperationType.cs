using System;

namespace Calculator.Avalonia.Models;

public enum OperationType
{
    Addition,
    Subtraction,
    Dividing,
    Multiplication
}


public static class OperationHelper
{
    public static char GetSymbol(OperationType operation)
    {
        return operation switch
        {
            OperationType.Addition => '+',
            OperationType.Subtraction => '-',
            OperationType.Dividing => '/',
            OperationType.Multiplication => '*',
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
    }
}