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
    }
}