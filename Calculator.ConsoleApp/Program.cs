using Calculator.ConsoleApp;
using Calculator.Core.Operations;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

var operations = MathOperationsFactory.CreateOperations();

services.AddSingleton<Calculator.Core.Calculator>(provider =>
    new Calculator.Core.Calculator(operations));

services.AddSingleton<ConsoleProcessor>();
var serviceProvider = services.BuildServiceProvider();

var consoleProcessor = serviceProvider.GetRequiredService<ConsoleProcessor>();
consoleProcessor.Run();