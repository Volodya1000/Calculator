using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ComandsListBox.ItemsSource = App.ServiceProvider.GetService<Calculator.Core.Calculator>()?.GetAvailableOperations();
        }
    }
}