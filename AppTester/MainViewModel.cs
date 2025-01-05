using AppTester.Utils;
using Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            InputPaths = new ObservableCollection<string>();
            OutputPaths = new ObservableCollection<string>();

            // Initialize commands
            // TopGrid
            AddSolutionCommand = new RelayCommand(AddSolution);
            RunTestsCommand = new RelayCommand(RunTests);

            // InputGrid and OutputGrid
            AddFilesCommand = new RelayCommand(AddFiles);
            AddFolderCommand = new RelayCommand(AddFolder);
            OverwriteCommand = new RelayCommand(OverwritePreview);

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
                if (InputPaths.Count != OutputPaths.Count)
                {
                    Console.WriteLine(Properties.Resources.InputOutputMismatchString);
                    return;
                }
                SolutionObj = new Solution(SolutionPath);
                if(SolutionObj.ExecutablePath is null)
                {
                    Console.WriteLine(Properties.Resources.CouldNotGetExecutablePathString);
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
            string? whichGrid = parameter.ToString();
            if (string.IsNullOrEmpty(whichGrid))
                return;

            string[] files = FileManager.GetFiles("txt");
            if (string.Compare(whichGrid, "input", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                foreach (string file in files)
                {
                    InputPaths.Add(file);
                }
            }
            else
            {
                foreach (string file in files)
                {
                    OutputPaths.Add(file);
                }
            }
        }

        private void AddFolder(object parameter)
        {
            string? whichGrid = parameter.ToString();
            if (string.IsNullOrEmpty(whichGrid))
                return;

            string[] files = FileManager.GetFilesFromFolder("txt");
            if (string.Compare(whichGrid, "input", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                foreach (string file in files)
                {
                    InputPaths.Add(file);
                }
            }
            else
            {
                foreach (string file in files)
                {
                    OutputPaths.Add(file);
                }
            }
        }
        private void OverwritePreview(object parameter)
        {
            // Logic for overwriting preview
        }

        private void PreviewHandler(object parameter)
        {
            throw new NotImplementedException();
        }
        private void MoveUpHandler(object parameter)
        {
            throw new NotImplementedException();
        }
        private void MoveDownHandler(object parameter)
        {
            throw new NotImplementedException();
        }
        private void DeleteHandler(object parameter)
        {
            throw new NotImplementedException();
        }

    }
}
