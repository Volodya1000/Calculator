using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator.AvaloniaUI.ViewModels;

public class ReactiveViewModelExample : ReactiveObject
{
    public ObservableCollection<string> ConversationLog { get; } = new ObservableCollection<string>();

    public ICommand OpenThePodBayDoorsDirectCommand { get; }

    public ICommand OpenThePodBayDoorsFellowRobotCommand { get; }

    public ICommand OpenThePodBayDoorsAsyncCommand { get; }

    private void AddToConvo(string content)
    {
        ConversationLog.Add(content);
    }

    public ReactiveViewModelExample()
    {
        // Init OpenThePodBayDoorsDirectCommand
      
    }

    // Backing field for RobotName
    private string? _inputText;

    
    public string? InputText
    {
        get => _inputText;
        set => this.RaiseAndSetIfChanged(ref _inputText, value);
    }
 
}
