using System;
using Avalonia.Controls;
using Avalonia.Animation;
using Avalonia.Styling;

namespace MyDesktop
{
    public partial class LaunchpadWindow : Window
    {
        public LaunchpadWindow()
        {
            InitializeComponent();

            // окно всегда поверх остальных
            this.Topmost = true;
        }

        // Анимация плавного закрытия
        protected override async void OnClosing(WindowClosingEventArgs e)
        {
            // Блокируем мгновенное закрытие
            e.Cancel = true;

            // Убедимся, что окно видно перед анимацией
            this.Opacity = 1;

            // Анимация исчезновения
            var anim = new Animation
            {
                Duration = TimeSpan.FromMilliseconds(150),
                FillMode = FillMode.Forward,
                Children =
                {
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters =
                        {
                            new Setter(OpacityProperty, 0)
                        }
                    }
                }
            };

            // Запускаем анимацию
            await anim.RunAsync(this);

            // После анимации закрываем реально
            base.OnClosing(e);
        }
    }
}
