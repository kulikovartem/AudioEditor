using System;
using System.Windows;
using Microsoft.Win32;
using NAudio.Wave;
using CommandInterface;


namespace AudioEditor
{
    public partial class MainWindow : Window
    {
        private WaveOutEvent outputDevice;
        private CommandManager commandManager;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void LoadTrackButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio files (*.mp3;*.wav)|*.mp3;*.wav",
                Title = "Select an Audio File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                commandManager = new CommandManager(FileCommands.LoadAudioFile(openFileDialog.FileName));
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (CommandManager.CurrentTrack == null)
            {
                MessageBox.Show("WaveStream не загружен.");
                return;
            }

            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.Init(CommandManager.CurrentTrack);
            }

            if (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Pause();
            }
            else
            {
                CommandManager.CurrentTrack.Position = 0;
                outputDevice.Play();
            }
        }

        private void ChangeSpeed(object sender, RoutedEventArgs e)
        {
            var dialog = new ParameterWindow();
            if (dialog.ShowDialog() == true)
            {
                string arg1 = dialog.Arg1;
                if (double.TryParse(arg1, out double arg))
                {
                    commandManager.ExecuteCommand(new ChangeSpeedCommand(arg));
                }
            }
        }
    }
}