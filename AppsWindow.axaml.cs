using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MyDesktop
{
    public partial class AppsWindow : Window
    {
        public AppsWindow()
        {
            InitializeComponent();
            
            // Подписываемся на события
            this.Opened += OnOpened;
            this.Deactivated += OnDeactivated;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOpened(object? sender, EventArgs e)
        {
            // Математика для центрирования окна над панелью задач
            var screen = Screens.Primary;
            if (screen != null)
            {
                var screenWidth = screen.WorkingArea.Width;
                var screenHeight = screen.WorkingArea.Height;

                // Ширина окна задана в XAML (600), высота (400)
                double windowWidth = this.Width; 
                double windowHeight = this.Height;

                // Центр по горизонтали
                double x = (screenWidth - windowWidth) / 2;
                
                // Снизу, но с отступом 80px (чтобы не перекрывать док)
                double y = screenHeight - windowHeight - 80;

                this.Position = new PixelPoint((int)x, (int)y);
            }
        }

        // Если кликнули мимо окна - закрываем его
        private void OnDeactivated(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void Close_Click(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // === Обработчики запуска приложений ===

        private void VSCode_Click(object? sender, RoutedEventArgs e)
        {
            AppLauncher.Run(AppConfig.VSCodePathWin, AppConfig.VSCodeCommandLinux);
            this.Close(); // Закрываем меню после клика
        }

        private void Terminal_Click(object? sender, RoutedEventArgs e)
        {
            // В Windows cmd, в Linux - gnome-terminal
            AppLauncher.Run("cmd.exe", AppConfig.TerminalCommandLinux);
            this.Close();
        }

        private void Calc_Click(object? sender, RoutedEventArgs e)
        {
            AppLauncher.Run(AppConfig.CalcPathWin, AppConfig.CalcCommandLinux);
            this.Close();
        }

        private void Opera_Click(object? sender, RoutedEventArgs e)
        {
            AppLauncher.Run(AppConfig.OperaPathWin, AppConfig.OperaCommandLinux);
            this.Close();
        }
    }
}
