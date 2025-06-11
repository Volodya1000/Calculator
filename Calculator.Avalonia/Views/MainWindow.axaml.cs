using Avalonia.Controls;
using Calculator.Avalonia.ViewModels;
using Calculator.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(Program.ServiceProvider.GetService<IExpressionCalculator>());
        }
    }
}