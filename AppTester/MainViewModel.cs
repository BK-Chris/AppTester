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
        private string _selectedInputPath = string.Empty;
        private string _selectedOutputPath = string.Empty;
        private string _selectedSolution = Properties.Resources.SolutionPathString;

        private string _selectedPreviewInputPath = Properties.Resources.NoSelectionPreviewString;
        private string _selectedPreviewOutputPath = Properties.Resources.NoSelectionPreviewString;
        private string _selectedInputPreviewItem = Properties.Resources.NoSelectionPreviewString;
        private string _selectedOutputPreviewItem = Properties.Resources.NoSelectionPreviewString;

        private ObservableCollection<string> _inputPaths = [];
        private ObservableCollection<string> _outputPaths = [];

        public Solution? SolutionObj;

        public ObservableCollection<string> InputPaths
        {
            get => _inputPaths;
            set
            {
                if (_inputPaths != value)
                {
                    _inputPaths = value;
                    OnPropertyChanged();
                    ((RelayCommand)EmptyListCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public ObservableCollection<string> OutputPaths
        {
            get => _outputPaths;
            set
            {
                if (_outputPaths != value)
                {
                    _outputPaths = value;
                    OnPropertyChanged();
                    ((RelayCommand)EmptyListCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public string SolutionPath
        {
            get => _selectedSolution;
            set
            {
                if (_selectedSolution != value)
                {
                    _selectedSolution = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SelectedInputPath
        {
            get => _selectedInputPath;
            set
            {
                if (_selectedInputPath == value) return;

                _selectedInputPath = value;
                OnPropertyChanged();
                ((RelayCommand)PreviewCommand).RaiseCanExecuteChanged();
                ((RelayCommand)MoveUpCommand).RaiseCanExecuteChanged();
                ((RelayCommand)MoveDownCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }
        public string SelectedOutputPath
        {
            get => _selectedOutputPath;
            set
            {
                if (_selectedOutputPath == value) return;

                _selectedOutputPath = value;
                OnPropertyChanged();
                ((RelayCommand)PreviewCommand).RaiseCanExecuteChanged();
                ((RelayCommand)MoveUpCommand).RaiseCanExecuteChanged();
                ((RelayCommand)MoveDownCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }
        public string SelectedPreviewInputPath
        {
            get => _selectedPreviewInputPath;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = Properties.Resources.NoSelectionPreviewString;
                }
                if (_selectedPreviewInputPath != value)
                {
                    _selectedPreviewInputPath = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SelectedPreviewOutputPath
        {
            get => _selectedPreviewOutputPath;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = Properties.Resources.NoSelectionPreviewString;
                }
                if (_selectedPreviewOutputPath != value)
                {
                    _selectedPreviewOutputPath = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged(nameof(SelectedInputPreview));
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
        public ICommand EmptyListCommand { get; }
        public ICommand OverwriteCommand { get; }
        public ICommand PreviewCommand { get; }
        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DeleteAllCommand { get; }

        public MainViewModel()
        {
            // Initialize commands
            // TopGrid
            AddSolutionCommand = new RelayCommand(AddSolution);
            RunTestsCommand = new RelayCommand(RunTests);

            // InputGrid and OutputGrid
            AddFilesCommand = new RelayCommand(AddFiles);
            AddFolderCommand = new RelayCommand(AddFolder);
            EmptyListCommand = new RelayCommand(EmptyListHandler, CanExecuteEmptyList);
            OverwriteCommand = new RelayCommand(OverwritePreview, CanExecuteOverwrite);


            // List interactions
            PreviewCommand = new RelayCommand(PreviewHandler, CanExecutePreview);
            MoveUpCommand = new RelayCommand(MoveUpHandler, CanExecuteMovement);
            MoveDownCommand = new RelayCommand(MoveDownHandler, CanExecuteMovement);
            DeleteCommand = new RelayCommand(DeleteHandler, CanExecuteDelete);
            DeleteAllCommand = new RelayCommand(EmptyListHandler, CanExecuteEmptyList);
        }

        // TopGrid buttons
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

        // InputGrid and OutputGrid buttons
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
            ((RelayCommand)EmptyListCommand).RaiseCanExecuteChanged();
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
            ((RelayCommand)EmptyListCommand).RaiseCanExecuteChanged();
        }
        private void EmptyListHandler(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                Console.WriteLine(Properties.Resources.NoSelectionPreviewString);
                InputPaths = [];
                SelectedInputPath = string.Empty;
                SelectedPreviewInputPath = Properties.Resources.NoSelectionPreviewString;
                SelectedInputPreview = Properties.Resources.NoSelectionPreviewString;
            }

            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                OutputPaths = [];
                SelectedOutputPath = string.Empty;
                SelectedPreviewOutputPath = Properties.Resources.NoSelectionPreviewString;
                SelectedOutputPreview = Properties.Resources.NoSelectionPreviewString;
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

        // List interactions
        private void PreviewHandler(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                Console.WriteLine(SelectedInputPath);
                try
                {
                    using StreamReader inputFile = new(SelectedInputPath);
                    SelectedPreviewInputPath = SelectedInputPath;
                    SelectedInputPreview = inputFile.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                Console.WriteLine(SelectedOutputPath);
                try
                {
                    using StreamReader outputFile = new(SelectedOutputPath);
                    SelectedPreviewOutputPath = SelectedOutputPath;
                    SelectedOutputPreview = outputFile.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                return;
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
                string? pathToDelete = SelectedInputPath;
                if (string.IsNullOrEmpty(pathToDelete)) return;

                InputPaths.Remove(pathToDelete);
                if (pathToDelete.Equals(SelectedPreviewInputPath))
                {
                    SelectedInputPath = string.Empty;
                    SelectedPreviewInputPath = Properties.Resources.NoSelectionPreviewString;
                    SelectedInputPreview = Properties.Resources.NoSelectionPreviewString;
                }
            }
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                string? pathToDelete = SelectedOutputPath;
                if (string.IsNullOrEmpty(pathToDelete)) return;

                OutputPaths.Remove(pathToDelete);
                if (pathToDelete.Equals(SelectedPreviewOutputPath))
                {
                    SelectedOutputPath = string.Empty;
                    SelectedPreviewOutputPath = Properties.Resources.NoSelectionPreviewString;
                    SelectedOutputPreview = Properties.Resources.NoSelectionPreviewString;
                }
            }
        }

        // CanExecute implementations
        private bool CanExecuteOverwrite(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                return !string.IsNullOrEmpty(SelectedInputPreview) && File.Exists(SelectedPreviewInputPath);
            }
            else if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                return !string.IsNullOrEmpty(SelectedOutputPreview) && File.Exists(SelectedPreviewOutputPath);
            }
            else
            {
                return false;
            }
        }
        private bool CanExecuteEmptyList(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                return InputPaths.Count > 0;
            }
            else if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                return OutputPaths.Count > 0;
            }
            else
            {
                return false;
            }
        }
        private bool CanExecutePreview(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                return IsValidSelection(SelectedInputPath);
            }
            else if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                return IsValidSelection(SelectedOutputPath);
            }
            else
            {
                return false;
            }
        }
        private bool CanExecuteMovement(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                return InputPaths.Count > 1 && IsValidSelection(SelectedInputPath);
            }
            else if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                return OutputPaths.Count > 1 && IsValidSelection(SelectedOutputPath);
            }
            else
            {
                return false;
            }
        }
        private bool CanExecuteDelete(object parameter)
        {
            if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Input)
            {
                return IsValidSelection(SelectedInputPath);
            }
            else if (Utilities.GetIOTypeFromCommandParameter(parameter) == IOType.Output)
            {
                return IsValidSelection(SelectedOutputPath);
            }
            else
            {
                return false;
            }
        }

        // Utils
        private static bool IsValidSelection(string selectedPath)
        {
            return !string.IsNullOrEmpty(selectedPath) && !selectedPath.Equals(Properties.Resources.NoSelectionPreviewString);
        }
    }
}
