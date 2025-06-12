using Calculator.ConsoleApp;

Calculator.Core.Calculator calculator = new Calculator.Core.Calculator()
{
    ["pow"] = Math.Pow,
    ["negate"] = (double x) => -x,
    ["average"] = (double[] x) => x.Average()
}.AddAssembly("CalculatorExternalAssembly.dll");

var consoleProcessor = new ConsoleProcessor(calculator); 
consoleProcessor.Run();

