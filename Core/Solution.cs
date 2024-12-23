using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Core
{
    public class Solution
    {
        private readonly string SolutionPath;
        private readonly string CsprojPath;
        private readonly string FrameworkType;
        public string ExecutablePath { get; private set; }

        public Solution(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
                throw new ArgumentNullException(nameof(solutionPath));
            if (!File.Exists(solutionPath))
                throw new ArgumentException($"{solutionPath} does not exists!");
            SolutionPath = solutionPath;
            CsprojPath = GetCsprojPath();
            FrameworkType = GetFrameworkType();
            ExecutablePath = GetExecutablePath();
        }

        /// <summary>
        /// Fetches the Csproj path found in the solution file specified by SolutionPath.
        /// </summary>
        /// <remarks>
        /// The function uses Regex to locate the .csproj file within the solution file.
        /// If no match is found or an exception occurs, it returns an empty string.
        /// </remarks>
        /// <returns>The full path to the .csproj file, or <see cref="string.Empty"/> if not found.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the solution file is not found.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while reading the file.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        private string GetCsprojPath()
        {
            string pattern = @"""[^""]*\.csproj""";
            Debug.WriteLine($"Regex Pattern: {pattern}");

            try
            {
                using (StreamReader sr = new(SolutionPath))
                {
                    string slnFile = sr.ReadToEnd();
                    Match match = Regex.Match(slnFile, pattern);
                    if (match.Success)
                    {
                        string csProjPath = $"{SolutionPath[..(SolutionPath.LastIndexOf('\\') + 1)]}{match.Value.Trim('\"')}";
                        Debug.WriteLine(csProjPath);
                        return csProjPath;
                    }
                    else
                    {
                        Debug.WriteLine("The .sln file does not have a .csproj path defined!");
                        return string.Empty;
                    }
                };
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine("Exception during getting Csproj Path\nFile not found: " + ex.Message);
                Console.WriteLine("Exception during getting Csproj Path\nFile not found: " + ex.Message);
            }
            catch (IOException ex)
            {
                Debug.WriteLine("Exception during getting Csproj Path\nAn I/O error occurred: " + ex.Message);
                Console.WriteLine("Exception during getting Csproj Path\nAn I/O error occurred: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception during getting Csproj Path\nAn unexpected error occurred: " + ex.Message);
                Console.WriteLine("Exception during getting Csproj Path\nAn unexpected error occurred: " + ex.Message);
            }
            return string.Empty;
        }

        /// <summary>
        /// Fetches the framework type from Csproj file.
        /// </summary>
        /// <remarks>
        /// The method searches for pattern <TargetFramework>*</TargetFramework> to determine the framework used.
        /// By default visual studio creates the project's build in the targetframework's folder.
        /// </remarks>
        /// <returns>Returns the frameworktype if it exists, otherwise null.</returns>
        private string GetFrameworkType()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Determines if the project's build is an executable.
        /// </summary>
        /// <remarks>The method searches for pattern <OutputType>*</OutputType> to determine if the project has and executable.</remarks>
        /// <returns>Returns true if the project is executable otherwise false.</returns>
        private bool IsExecutable()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Return's the executable's path if it exists combining the expected folder structure to find the exe in it.
        /// </summary>
        /// <remarks>The method uses the CsprojPath concatanated with FrameworkType and the executable's name.</remarks>
        /// <returns>Returns the executable's path if it exists. Otherwise returns null.</returns>
        private string GetExecutablePath()
        {
            throw new System.NotImplementedException();
        }
    }
}