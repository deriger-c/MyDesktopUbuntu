using System;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace MyDesktop
{
    public partial class TerminalWindow : Window
    {
        private string _currentDirectory;
        
        private TextBlock? _outputBlock;
        private ScrollViewer? _outputScroll;
        private TextBlock? _promptBlock;
        private TextBox? _inputBox;

        public TerminalWindow()
        {
            InitializeComponent();

            _outputBlock = this.FindControl<TextBlock>("OutputBlock");
            _outputScroll = this.FindControl<ScrollViewer>("OutputScroll");
            _promptBlock = this.FindControl<TextBlock>("PromptBlock");
            _inputBox = this.FindControl<TextBox>("InputBox");

            _currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            UpdatePrompt();
            
            this.Opened += (s, e) => {
                _inputBox?.Focus();
                
                // === АВТОЗАПУСК NEOFETCH ===
                // --stdout убирает сложные цветовые коды, чтобы текст не ломался
                ProcessCommand("neofetch --stdout"); 
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Close_Click(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InputBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && _inputBox != null && _outputBlock != null && _outputScroll != null && _promptBlock != null)
            {
                var cmd = _inputBox.Text;
                if (string.IsNullOrWhiteSpace(cmd)) return;

                _outputBlock.Text += $"{_promptBlock.Text}{cmd}\n";
                _inputBox.Text = "";

                ProcessCommand(cmd);
                _outputScroll.ScrollToEnd();
            }
        }

        private void ProcessCommand(string cmd)
        {
            string cleanCmd = cmd.Trim();
            
            if (_outputBlock == null) return;

            if (cleanCmd == "clear")
            {
                _outputBlock.Text = "";
                return;
            }

            if (cleanCmd.StartsWith("cd "))
            {
                string newDir = cleanCmd.Substring(3).Trim();
                try 
                {
                    string targetPath = Path.Combine(_currentDirectory, newDir);
                    targetPath = Path.GetFullPath(targetPath);

                    if (Directory.Exists(targetPath))
                    {
                        _currentDirectory = targetPath;
                        UpdatePrompt();
                    }
                    else
                    {
                        _outputBlock.Text += $"bash: cd: {newDir}: No such file or directory\n";
                    }
                }
                catch (Exception ex)
                {
                    _outputBlock.Text += $"bash: cd: {ex.Message}\n";
                }
                return;
            }

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        // Если команда neofetch - добавляем флаг --stdout, иначе запускаем как есть
                        Arguments = cleanCmd.StartsWith("neofetch") 
                                    ? $"-c \"neofetch --stdout\"" 
                                    : $"-c \"{cleanCmd}\"",
                                    
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = _currentDirectory
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(output)) _outputBlock.Text += output;
                if (!string.IsNullOrEmpty(error)) _outputBlock.Text += error;
            }
            catch (Exception ex)
            {
                _outputBlock.Text += $"Error launching process: {ex.Message}\n";
            }
        }

        private void UpdatePrompt()
        {
            if (_promptBlock == null) return;
            string folderName = new DirectoryInfo(_currentDirectory).Name;
            _promptBlock.Text = $"user@desktop: {folderName} $ ";
        }
    }
}
