using System;

namespace Calculator.Avalonia.Models;

//public static class CalculatorUtils
//{
//    public static void AppendNumber(CalculatorContext context, int number)
//    {
//        // Если буфер уже заполнен - выходим
//        if (context.MainBuffer.Length >= CalculatorWithBuffer.MAX_MAIN_BUFFER_LENGTH)
//            return;

//        string numberStr = number.ToString();

//        // Рассчитываем, сколько символов можно добавить
//        int availableSpace = CalculatorWithBuffer.MAX_MAIN_BUFFER_LENGTH - context.MainBuffer.Length;
//        int charsToTake = Math.Min(availableSpace, numberStr.Length);
//        string truncatedNumber = numberStr.Substring(0, charsToTake);

//        // Основная логика добавления числа:
//        if (context.MainBuffer == "0" ||
//            !string.IsNullOrEmpty(context.PendingOperation))
//        {
//            // Заменяем буфер если:
//            //  это первый ввод (буфер равен 0)
//            //  или после выбора операции
//            context.MainBuffer = truncatedNumber;
//        }
//        else
//        {
//            // Иначе добавляем к существующему значению
//            context.MainBuffer += truncatedNumber;
//        }
//    }

//    public static void UpdateHistoryWithOperation(CalculatorContext context, string operationSymbol)
//    {
//        context.HistoryBuffer = $"{context.CurrentValue} {operationSymbol}";
//    }
//}