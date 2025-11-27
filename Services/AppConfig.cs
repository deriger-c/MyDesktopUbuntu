using System;

namespace MyDesktop
{
    public static class AppConfig
    {
        // === Настройки для Windows ===
        public const string OperaPathWin = @"C:\Program Files\Opera GX\opera.exe";
        public const string ChromePathWin = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
        public const string VSCodePathWin = @"C:\Users\denis\AppData\Local\Programs\Microsoft VS Code\Code.exe"; // Проверь свой путь если не сработает
        public const string CalcPathWin = @"calc.exe";

        // === Настройки для Linux ===
        public const string OperaCommandLinux = "opera"; 
        public const string ChromeCommandLinux = "google-chrome";
        public const string VSCodeCommandLinux = "code";
        public const string TerminalCommandLinux = "gnome-terminal"; // Или "x-terminal-emulator"
        public const string CalcCommandLinux = "gnome-calculator";   // Или "galculator"

        // === Ссылки ===
        public const string OperaDownloadUrl = "https://www.opera.com/gx";
    }
}
