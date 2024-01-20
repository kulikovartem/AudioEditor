using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;


namespace AudioEditor
{
    public partial class MainWindow : Window
    {
        public static CommandManager commandManager;

        public MainWindow()
        {
            InitializeComponent();
            DisableAllButtonsExceptLoad();
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
                MakeFolder(MakeFolderPath("temp"));
                commandManager = new CommandManager(FileCommands.Mp3ToBytes(openFileDialog.FileName));
                FileCommands.BytesToMp3(CommandManager.CurrentTrack);
                EnableAllButtons();
            }
        }
        private static string MakeFolderPath(string pathName)
        {

            // Получаем путь к текущей рабочей директории
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Составляем полный путь к целевой папке
            return System.IO.Path.Combine(CleanAfter(currentDirectory, "bin"), pathName);

        }
        private static string CleanAfter(string path, string word)
        {
            string[] words = path.Split('\\');
            List<string> res = new List<string>();
            var adding = false;
            for (var i = words.Length - 1; i >= 0; i--)
            {
                if (adding)
                    res.Add(words[i]);
                if (words[i] == word)
                    adding = true;
            }
            res.Reverse();
            return string.Join('\\', res);
        }
        private static void MakeFolder(string folderPath)
        {

            // Удаляем папку и создаем
            Directory.Delete(folderPath, true);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

        }

        private void FadeOutButton_Click(object sender, RoutedEventArgs e)
        {
            FadeOutWindow fadeoutwindow = new FadeOutWindow();
            fadeoutwindow.Show();
        }

        private void FadeInButton_Click(object sender, RoutedEventArgs e)
        {
            FadeInWindow fadeinwindow = new FadeInWindow();
            fadeinwindow.Show();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.commandManager.Redo();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.commandManager.Undo();
        }

        private void ChangeSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeSpeedWindow changespeedwin = new ChangeSpeedWindow();
            changespeedwin.Show();
        }

        private void MergeButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio files (*.mp3;*.wav)|*.mp3;*.wav",
                Title = "Select an Audio File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var name = openFileDialog.FileName;
                var command = new MergeCommand(name);
                MainWindow.commandManager.ExecuteCommand(command);
            }
        }

        private void MixButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio files (*.mp3;*.wav)|*.mp3;*.wav",
                Title = "Select an Audio File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var name = openFileDialog.FileName;
                var command = new MixAudioCommand(name);
                MainWindow.commandManager.ExecuteCommand(command);
            }
        }

        private void TrimButton_Click(object sender, RoutedEventArgs e)
        {
            var TrimWindow = new TrimWindow();
            TrimWindow.Show();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Сохранить MP3 файл",
                Filter = "Файлы MP3|*.mp3"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string outputPath = saveFileDialog.FileName;
                FileCommands.BytesToMp3(CommandManager.CurrentTrack, outputPath);

                MessageBox.Show("Файл MP3 сохранен успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ChangeVolumeButton_Click(object sender, RoutedEventArgs e)
        {
            var changevolumewin = new ChangeVolumeWindow();
            changevolumewin.Show();
        }

        private void EnableAllButtons()
        {
            foreach (UIElement element in StackPanel_Left.Children)
            {
                if (element is Button button)
                {
                    button.IsEnabled = true;
                }
            }

            foreach (UIElement element in StackPanel_Right.Children)
            {
                if (element is Button button)
                {
                    button.IsEnabled = true;
                }
            }
        }

        private void DisableAllButtonsExceptLoad()
        {
            foreach (UIElement element in StackPanel_Left.Children)
            {
                if (element is Button button && button != LoadTrackButton)
                {
                    button.IsEnabled = false;
                }
            }

            foreach (UIElement element in StackPanel_Right.Children)
            {
                if (element is Button button && button != LoadTrackButton)
                {
                    button.IsEnabled = false;
                }
            }
        }
    }
}