using System;
using System.Collections.Generic;
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

namespace AudioEditor
{
    public partial class FadeOutWindow : Window
    {
        public FadeOutWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            double parsedArg1;
            double parsedArg2;

            if ((double.TryParse(Arg1TextBox.Text, out parsedArg1)) && (double.TryParse(Arg2TextBox.Text, out parsedArg2)))
            {
                var command = new FadeOutCommand(parsedArg1, parsedArg2);
                MainWindow.commandManager.ExecuteCommand(command);
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка: Аргумент 1 или 2 должен быть числом типа double.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
