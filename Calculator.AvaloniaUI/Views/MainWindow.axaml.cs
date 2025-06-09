using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Linq;

namespace Calculator.AvaloniaUI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            OperationsListBox.ItemsSource = App.ServiceProvider.GetService<Calculator.Core.Calculator>()?.GetAvailableOperations();
        }

       
    }
}