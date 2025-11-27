using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace MyDesktop
{
    public partial class MainWindow : Window
    {
        // === Окна ===
        private AppsWindow? _appsWindow;
        private SettingsWindow? _settingsWindow;
        private FilesWindow? _filesWindow;
        private TerminalWindow? _terminalWindow; // <--- НОВОЕ

        private readonly DispatcherTimer _clockTimer;
        private bool _isDragging = false;
        private PixelPoint _dragStartPoint;
        private PixelPoint _windowStartPoint;

        public MainWindow()
        {
            InitializeComponent();
            this.SystemDecorations = SystemDecorations.None;
            this.Topmost = true; 
            this.Background = Brushes.Transparent;
            this.CanResize = false;
            this.Opened += OnOpened;

            _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _clockTimer.Tick += (sender, e) =>
            {
                var now = DateTime.Now;
                if (ClockBlock != null) ClockBlock.Text = now.ToString("HH:mm");
                if (DateBlock != null) DateBlock.Text = now.ToString("ddd, dd MMM");
            };
            _clockTimer.Start();
        }

        private void OnOpened(object? sender, EventArgs e)
        {
            var screen = Screens.Primary;
            if (screen != null)
            {
                this.Width = screen.WorkingArea.Width;
                this.Position = new PixelPoint(screen.WorkingArea.X, screen.WorkingArea.Height - (int)this.Height);
            }
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
            _isDragging = true;
            _dragStartPoint = this.PointToScreen(e.GetPosition(this));
            _windowStartPoint = this.Position;
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (!_isDragging) return;
            var currentPoint = this.PointToScreen(e.GetPosition(this));
            var dx = currentPoint.X - _dragStartPoint.X;
            var dy = currentPoint.Y - _dragStartPoint.Y;
            this.Position = new PixelPoint(_windowStartPoint.X + dx, _windowStartPoint.Y + dy);
        }

        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e) { _isDragging = false; }

        private void Apps_Click(object? sender, RoutedEventArgs e)
        {
            if (_appsWindow == null || !_appsWindow.IsVisible) { _appsWindow = new AppsWindow(); _appsWindow.Show(); }
            else _appsWindow.Activate();
        }

        // === НОВАЯ ФУНКЦИЯ ДЛЯ ТЕРМИНАЛА ===
        private void Terminal_Click(object? sender, RoutedEventArgs e)
        {
            if (_terminalWindow == null || !_terminalWindow.IsVisible) 
            { 
                _terminalWindow = new TerminalWindow(); 
                _terminalWindow.Show(); 
            }
            else _terminalWindow.Activate();
        }

        private void Settings_Click(object? sender, RoutedEventArgs e)
        {
            if (_settingsWindow == null || !_settingsWindow.IsVisible) { _settingsWindow = new SettingsWindow(); _settingsWindow.Show(); }
            else _settingsWindow.Activate();
        }

        private void Files_Click(object? sender, RoutedEventArgs e)
        {
            if (_filesWindow == null || !_filesWindow.IsVisible) { _filesWindow = new FilesWindow(); _filesWindow.Show(); }
            else _filesWindow.Activate();
        }

        private void Browser_Click(object? sender, RoutedEventArgs e) { AppLauncher.Run(AppConfig.OperaPathWin, AppConfig.OperaCommandLinux); }
        private void Chrome_Click(object? sender, RoutedEventArgs e) { AppLauncher.Run(AppConfig.ChromePathWin, AppConfig.ChromeCommandLinux); }
        private void Exit_Click(object? sender, RoutedEventArgs e) { this.Close(); }
    } 
}
