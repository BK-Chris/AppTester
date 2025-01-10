using AppTester.Utils;
using Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppTester
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _selectedSolution = Properties.Resources.SolutionPathString;
        private string _selectedInputPath = Properties.Resources.NoSelectionPreviewString;
        private string _selectedOutputPath = Properties.Resources.NoSelectionPreviewString;
        private string _selectedInputPreviewItem = Properties.Resources.NoSelectionPreviewString;
        private string _selectedOutputPreviewItem = Properties.Resources.NoSelectionPreviewString;
        public Solution? SolutionObj;

        public ObservableCollection<string> InputPaths { get; set; }
        public ObservableCollection<string> OutputPaths { get; set; }

        public string SolutionPath
        {
            get => _selectedSolution;
            set
            {
                if (_selectedSolution != value)
                {
                    _selectedSolution = value;
                    OnPropertyChanged(); // Notify the UI about the change
                }
            }
        }
        public string SelectedInputPath
        {
            get => _selectedInputPath;
            set
            {
                if (_selectedInputPath != value)
                {
                    _selectedInputPath = value;
                    OnPropertyChanged(); // Notify the UI about the change
                }
            }
        }
        public string SelectedOutputPath
        {
            get => _selectedOutputPath;
            set
            {
                if (_selectedOutputPath != value)
                {
                    _selectedOutputPath = value;
                    OnPropertyChanged(); // Notify the UI about the change
                }
            }
        }
        public string SelectedInputPreview
        {
            get => _selectedInputPreviewItem;
            set
            {
                if (_selectedInputPreviewItem != value)
                {
                    _selectedInputPreviewItem = value;
                    OnPropertyChanged();
                    ((RelayCommand)OverwriteCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string SelectedOutputPreview
        {
            get => _selectedOutputPreviewItem;
            set
            {
                if (_selectedOutputPreviewItem != value)
                {
                    _selectedOutputPreviewItem = value;
                    OnPropertyChanged();
                    ((RelayCommand)OverwriteCommand).RaiseCanExecuteChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddSolutionCommand { get; }
        public ICommand RunTestsCommand { get; }
        public ICommand AddFilesCommand { get; }
        public ICommand AddFolderCommand { get; }
        public ICommand OverwriteCommand { get; }
        public ICommand PreviewCommand { get; }
        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel()
        {
            InputPaths = [];
            OutputPaths = [];

            // Initialize commands
            // TopGrid
            AddSolutionCommand = new RelayCommand(AddSolution);
            RunTestsCommand = new RelayCommand(RunTests);

            // InputGrid and OutputGrid
            AddFilesCommand = new RelayCommand(AddFiles);
            AddFolderCommand = new RelayCommand(AddFolder);
            OverwriteCommand = new RelayCommand(OverwritePreview, CanExecuteOverwrite);

            // List interactions
            PreviewCommand = new RelayCommand(PreviewHandler);
            MoveUpCommand = new RelayCommand(MoveUpHandler);
            MoveDownCommand = new RelayCommand(MoveDownHandler);
            DeleteCommand = new RelayCommand(DeleteHandler);
        }

        private void AddSolution(object parameter)
        {
            string solutionPath = FileManager.GetFile("sln");
            if (!string.IsNullOrEmpty(solutionPath))
            {
                SolutionPath = solutionPath;
            }
            else
            {
                SolutionPath = Properties.Resources.SolutionPathString;
            }
        }

        private async void RunTests(object parameter)
        {
            try
            {
                SolutionObj = new Solution(SolutionPath);

                if (SolutionObj.ExecutablePath is null)
                {
                    Console.WriteLine(Properties.Resources.CouldNotGetExecutablePathString);
                    return;
                }

                if (InputPaths.Count != OutputPaths.Count)
                {
                    Console.WriteLine(Properties.Resources.InputOutputMismatchString);
                    return;
                }

                for (int i = 0; i < InputPaths.Count; i++)
                {
                    bool isPassed = await Tester.TestInputFromFile(SolutionObj.ExecutablePath, InputPaths.ElementAt(i), OutputPaths.ElementAt(i));
                    Console.WriteLine($"Testcase #{i + 1} {(isPassed ? "PASSED" : "FAILED")}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void AddFiles(object parameter)
        {
            string[] files = FileManager.GetFiles("txt");
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                foreach (string file in files)
                {
                    InputPaths.Add(file);
                }
            }

            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                foreach (string file in files)
                {
                    OutputPaths.Add(file);
                }
            }
        }

        private void AddFolder(object parameter)
        {
            string[] files = FileManager.GetFilesFromFolder("txt");
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                foreach (string file in files)
                {
                    InputPaths.Add(file);
                }
            }

            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                foreach (string file in files)
                {
                    OutputPaths.Add(file);
                }
            }
        }
        private void OverwritePreview(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                FileManager.OverWrite(SelectedInputPath, SelectedInputPreview);
            }
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                FileManager.OverWrite(SelectedOutputPath, SelectedOutputPreview);
            }
        }


        private void PreviewHandler(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                Console.WriteLine(SelectedInputPath);
                try
                {
                    using StreamReader inputFile = new(SelectedInputPath);
                    SelectedInputPreview = inputFile.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                Console.WriteLine(SelectedOutputPath);
                try
                {
                    using StreamReader outputFile = new(SelectedOutputPath);
                    SelectedOutputPreview = outputFile.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private void MoveUpHandler(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                Utilities.MoveUpElementInAList(InputPaths, SelectedInputPath);
            }
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                Utilities.MoveUpElementInAList(OutputPaths, SelectedOutputPath);
            }
        }
        private void MoveDownHandler(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                Utilities.MoveDownElementInAList(InputPaths, SelectedInputPath);
            }
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                Utilities.MoveDownElementInAList(OutputPaths, SelectedOutputPath);
            }
        }
        private void DeleteHandler(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                InputPaths.Remove(SelectedInputPath);
                SelectedInputPath = Properties.Resources.NoSelectionPreviewString;
                SelectedInputPreview = Properties.Resources.NoSelectionPreviewString;
            }
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                OutputPaths.Remove(SelectedOutputPath);
                SelectedOutputPath = Properties.Resources.NoSelectionPreviewString;
                SelectedOutputPreview = Properties.Resources.NoSelectionPreviewString;
            }
        }


        // CanExecute implementations
        private bool CanExecuteOverwrite(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                return !string.IsNullOrEmpty(SelectedInputPreview) && File.Exists(SelectedInputPath);
            }
            else if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                return !string.IsNullOrEmpty(SelectedOutputPreview) && File.Exists(SelectedOutputPath);
            }
            else
            {
                return false;
            }
        }
    }
}
