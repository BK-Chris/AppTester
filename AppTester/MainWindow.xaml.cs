using Core;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace AppTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Solution? Solution;
        public MainWindow()
        {
            InitializeComponent();

            Console.SetOut(new TextBoxWriter(ConsoleOutputTextBox));
            Console.WriteLine(Properties.Resources.WelcomeString);
        }
        // Functions
        private static string? GetFilePath(string ext, bool multiselect)
        {
            OpenFileDialog dialog = new()
            {
                Filter = $"All files(*.{ext}) | *.{ext}",
                Multiselect = multiselect
            };
            if (dialog.ShowDialog().HasValue)
            {
                string path = dialog.FileName;
                Console.WriteLine($"Selected {path}");
                return path;
            }
            else
            {
                Console.WriteLine($"No file was selected!");
                return null;
            }
        }
        private static string? GetFolderPath()
        {
            OpenFolderDialog dialog = new();
            if (dialog.ShowDialog().HasValue)
            {
                string path = dialog.SafeFolderName;
                Console.WriteLine($"Selected {path}");
                return path;
            }
            else
            {
                Console.WriteLine($"No folder was selected!");
                return null;
            }
        }


        // Eventlisteners

        private void AddSolution_Click(object sender, RoutedEventArgs e)
        {
            string? solutionPath = GetFilePath("sln", false);
            if (solutionPath == null)
                return;
            try
            {
                Solution = new Solution(solutionPath);
                SolutionPathTextBox.Text = solutionPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void RunTests_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddManual_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OverwritePreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdatePreview_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}