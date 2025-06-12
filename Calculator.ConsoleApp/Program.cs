using Calculator.ConsoleApp;
using Calculator.Core.Builders;

//Сразу использую DependencyInjection чтоб на будущее было удобнее переделать например в AvaloniaUI
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

var operations = new OperationsBuilder().AddAll().Build();

services.AddSingleton<Calculator.Core.OneOperationCalculatorFacade>(provider =>
    new Calculator.Core.OneOperationCalculatorFacade(operations));

services.AddSingleton<ConsoleProcessor>();
var serviceProvider = services.BuildServiceProvider();

var consoleProcessor = serviceProvider.GetRequiredService<ConsoleProcessor>();
consoleProcessor.Run();
