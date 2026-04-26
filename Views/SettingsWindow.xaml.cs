using System.Collections.Generic;
using Avalonia.Controls;
using GraphicsLabAvalonia.Plugins;

namespace GraphicsLabAvalonia.Views;

public partial class SettingsWindow : Window
{
    public IDataProcessor? SelectedProcessor { get; private set; }
    public bool ProcessingEnabled => SelectedProcessor != null;

    public SettingsWindow(List<IDataProcessor> processors, IDataProcessor? current)
    {
        InitializeComponent();
        
        var items = new List<object> { "Отключено" };
        items.AddRange(processors);
        ProcessorListBox.ItemsSource = items;
        
        if (current != null)
        {
            var index = processors.IndexOf(current) + 1;
            if (index > 0) ProcessorListBox.SelectedIndex = index;
        }
        else
        {
            ProcessorListBox.SelectedIndex = 0;
        }
    }

    private void OnOkClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SelectedProcessor = ProcessorListBox.SelectedItem as IDataProcessor;
        Close();
    }

    private void OnCancelClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}