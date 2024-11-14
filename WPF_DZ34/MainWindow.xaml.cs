using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace WPF_DZ34
{
    public partial class MainWindow : Window
    {
        Timer timer;

        private DispatcherTimer timer2 = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1),
            IsEnabled = true,
        };
        public MainWindow()
        {
            InitializeComponent();
            slider.Value = 0.5;
            player.Volume = slider.Value;
            timer2.Tick += Timer_Tick;

            textBlock.Text = Path.GetFileName(player.Source.ToString());

        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (player.NaturalDuration.HasTimeSpan)
            {
                position.Value = player.Position / player.NaturalDuration.TimeSpan;
                durationText.Text = $"{player.Position.ToString(@"mm\:ss")}/{player.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}";
            }
        }

        private void position_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (player.NaturalDuration.HasTimeSpan)
            {
                player.Position = position.Value * player.NaturalDuration.TimeSpan;
                durationText.Text = $"{player.Position.ToString(@"mm\:ss")}/{player.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}";
            }
            timer2.Start();
        }

        private void position_DragStarted(object sender, DragStartedEventArgs e)
        {
            timer2.Stop();
        }
        private void ButtonPlay_Pause_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btnPlay.Content == "Play")
            {
                player.Play();
                btnPlay.Content = "Pause";
            }
            else
            {
                player.Pause();
                btnPlay.Content = "Play";
            }
        }
        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            if ((string)(fullScreen.Header) == "Fullscreen")
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
                fullScreen.Header = "exit fullscreen";
            }
            else
            {
                fullScreen.Header = "Fullscreen";
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;
            }

        }
        private void volume_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (popup.IsOpen)
                player.Volume = slider.Value;
        }

        private void player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show("Не удаётся воспроизвести файл. Попробуйте ввести другой путь");
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "media element| *.mp4";
            openFileDialog.ShowDialog();
            player.Source = new Uri(openFileDialog.FileName);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            player.Source = null;
            textBlock.Text = null;
        }

        private void player_MediaOpened(object sender, RoutedEventArgs e)
        {
            textBlock.Text = Path.GetFileName(player.Source.ToString());
        }
    }
}