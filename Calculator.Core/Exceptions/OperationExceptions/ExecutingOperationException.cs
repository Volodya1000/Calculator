using Calculator.Core.Exceptions.OperationExceptions;

namespace Calculator.Core.Exceptions;

//Он сильно похож на своего предка, но я думаю имеет смысл выделить отдельное исключение для ошибок конкретно при вычислении,
//тоесть когда по какой то причине вычисления не корректны даже при корректных аргументах
public sealed class ExecutingOperationException:OperationException
{
    public ExecutingOperationException(string operation,string problem) : base(operation,problem)
    {
    }
}
