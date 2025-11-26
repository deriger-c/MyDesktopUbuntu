using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MyDesktop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void Apps_Click(object? sender, RoutedEventArgs e)
    {
        var win = new AppsWindow();
        win.Show();
    }

    private void Settings_Click(object? sender, RoutedEventArgs e)
    {
        var win = new SettingsWindow();
        win.Show();
    }

    private void Files_Click(object? sender, RoutedEventArgs e)
    {
        var win = new FilesWindow();
        win.Show();
    }

}