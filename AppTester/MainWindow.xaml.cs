using AppTester.Utils;
using Core;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AppTester
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Console.SetOut(new TextBoxWriter(ConsoleOutputTextBox));
            Console.WriteLine(Properties.Resources.WelcomeString);
        }

        private void ListBox_MouseDoubleClick_Input(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                var viewModel = DataContext as MainViewModel;
                if (viewModel?.PreviewCommand?.CanExecute("input") == true)
                {
                    viewModel.PreviewCommand.Execute("input");
                }
            }
        }

        private void ListBox_MouseDoubleClick_Output(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                var viewModel = DataContext as MainViewModel;
                if (viewModel?.PreviewCommand?.CanExecute("output") == true)
                {
                    viewModel.PreviewCommand.Execute("output");
                }
            }
        }
    }
}