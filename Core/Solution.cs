using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    /// <summary>
    /// Represents a solution and provides methods to retrieve related information such as the project file path,
    /// framework type, and executable path. The class also supports building the project and retrieving the executable path
    /// if the build is successful. It attempts to build the project if it has not been built yet.
    /// </summary>
    public class Solution
    {
        private readonly string SolutionPath;
        public readonly bool IsBuilt;
        private readonly string? CsprojPath = null;
        private readonly string? FrameworkType = null;

        /// <summary>
        /// Gets the path of the executable for the solution if it exists.
        /// </summary>
        /// <remarks>
        /// This property combines the expected folder structure to find the executable (.exe) file 
        /// based on the .csproj file's path and the framework type.
        /// </remarks>
        /// <returns>Returns the full path to the executable file if it exists, otherwise <see cref="string.Empty"/>.</returns>
        public string? ExecutablePath { get; private set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Solution"/> class. It attempts to build the solution and retrieve
        /// related information such as the project file path, framework type, and executable path.
        /// </summary>
        /// <param name="solutionPath">The path to the solution file.</param>
        /// <exception cref="ArgumentNullException">Thrown when the solution path is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the solution file does not exist.</exception>
        public Solution(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
                throw new ArgumentNullException(nameof(solutionPath));
            if (!File.Exists(solutionPath))
                throw new ArgumentException($"{solutionPath} does not exists!");
            SolutionPath = solutionPath;

            IsBuilt = BuildSolution();

            if (IsBuilt)
            {
                CsprojPath = GetCsprojPath();
                FrameworkType = GetFrameworkType();
                ExecutablePath = GetExecutablePath();
            }
            else
            {
                Debug.WriteLine("Build failed. Cannot retrieve executable path.");
                Console.WriteLine("Build failed. Cannot retrieve executable path.");
            }
        }

        /// <summary>
        /// Runs the dotnet build process and returns a boolean indicating success.
        /// </summary>
        /// <returns>Returns true if the build is successful, otherwise false.</returns>
        public bool BuildSolution()
        {
            string caughtAt = "BuildSolution()";
            try
            {
                ProcessStartInfo processStartInfo = new()
                {
                    FileName = "dotnet",
                    Arguments = $"build \"{SolutionPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process? process = Process.Start(processStartInfo);
                if (process == null)
                {
                    Debug.WriteLine("Failed to start the build process.");
                    Console.WriteLine("Failed to start the build process.");
                    return false;
                }

                // Read the output and error streams synchronously
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Debug.WriteLine("Build succeeded.");
                    Console.WriteLine(output); // Optional: Log the success output.
                    return true;
                }
                else
                {
                    Debug.WriteLine($"Build failed: {error}");
                    Console.WriteLine($"Build failed: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception@[{caughtAt}]\nAn error occurred during the build: " + ex.Message);
                Console.WriteLine($"Exception@[{caughtAt}]\nAn error occurred during the build: " + ex.Message);
                return false;
            }
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
        private string? GetCsprojPath()
        {
            string caughtAt = "GetCsprojPath()";
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
                        Console.WriteLine("The .sln file does not have a .csproj path defined!");
                        return null;
                    }
                };
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine($"Exception@[{caughtAt}]\nFile not found: " + ex.Message);
                Console.WriteLine($"Exception@[{caughtAt}]\nFile not found: " + ex.Message);
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Exception@[{caughtAt}]\nAn I/O error occurred: " + ex.Message);
                Console.WriteLine($"Exception@[{caughtAt}]\nAn I/O error occurred: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception@[{caughtAt}]\nAn unexpected error occurred: " + ex.Message);
                Console.WriteLine($"Exception@[{caughtAt}]\nAn unexpected error occurred: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Fetches the framework type from Csproj file.
        /// </summary>
        /// <remarks>
        /// The method searches for pattern <TargetFramework>*</TargetFramework> to determine the framework used.
        /// By default, Visual Studio creates the project's build in the target framework's folder.
        /// </remarks>
        /// <returns>Returns the framework type if it exists, otherwise <see cref="string.Empty"/>.</returns>
        private string? GetFrameworkType()
        {
            string caughtAt = "GetFrameworkType()";
            string pattern = @"<TargetFramework>(.*?)</TargetFramework>";
            Debug.WriteLine($"Regex Pattern: {pattern}");

            try
            {
                using (StreamReader sr = new(CsprojPath))
                {
                    string csprojFile = sr.ReadToEnd();
                    Match match = Regex.Match(csprojFile, pattern);
                    if (match.Success)
                    {
                        string frameworkType = match.Groups[1].Value;
                        Debug.WriteLine(frameworkType);
                        return frameworkType;
                    }
                    else
                    {
                        Debug.WriteLine("The .csproj file does not have <TargetFramework> defined!");
                        return string.Empty;
                    }
                };
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine($"Exception@[{caughtAt}]\nFile not found: " + ex.Message);
                Console.WriteLine($"Exception@[{caughtAt}]\nFile not found: " + ex.Message);
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Exception@[{caughtAt}]\nAn I/O error occurred: " + ex.Message);
                Console.WriteLine($"Exception@[{caughtAt}]\nAn I/O error occurred: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception@[{caughtAt}]\nAn unexpected error occurred: " + ex.Message);
                Console.WriteLine($"Exception@[{caughtAt}]\nAn unexpected error occurred: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Determines if the project's build is an executable.
        /// </summary>
        /// <remarks>
        /// The method searches for the pattern <OutputType>*</OutputType> to determine if the project has an executable output.
        /// </remarks>
        /// <returns>Returns <see langword="true"/> if the project is executable, otherwise <see langword="false"/>.</returns>
        private bool IsExecutable()
        {
            string caughtAt = "IsExecutable()";
            string pattern = @"<OutputType>Exe</OutputType>";
            Debug.WriteLine($"Regex Pattern: {pattern}");

            try
            {
                using (StreamReader sr = new(CsprojPath))
                {
                    string csprojFile = sr.ReadToEnd();
                    Match match = Regex.Match(csprojFile, pattern);
                    if (match.Success)
                    {
                        Debug.WriteLine("The output is Exe!");
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("The .csproj file either does not have <OutputType> defined or is not an Executable!");
                        Console.WriteLine("The .csproj file either does not have <OutputType> defined or is not an Executable!");
                        return false;
                    }
                };
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine($"Exception@[{caughtAt}]\nFile not found: " + ex.Message);
                Console.WriteLine($"Exception@[{caughtAt}]\nFile not found: " + ex.Message);
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Exception@[{caughtAt}]\nAn I/O error occurred: " + ex.Message);
                Console.WriteLine($"Exception@[{caughtAt}]\nAn I/O error occurred: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception@[{caughtAt}]\nAn unexpected error occurred: " + ex.Message);
                Console.WriteLine($"Exception@[{caughtAt}]\nAn unexpected error occurred: " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Gets the path of the executable for the solution if it exists.
        /// </summary>
        /// <remarks>
        /// This property combines the expected folder structure to find the executable (.exe) file 
        /// based on the .csproj file's path and the framework type.
        /// </remarks>
        /// <returns>Returns the full path to the executable file if it exists, otherwise <see cref="string.Empty"/>.</returns>
        private string? GetExecutablePath()
        {
            if (string.IsNullOrEmpty(CsprojPath))
            {
                Debug.WriteLine("Could not found .csproj file!");
                Console.WriteLine("Could not found .csproj file!");
                return null;
            }
            if (string.IsNullOrEmpty(FrameworkType))
            {
                Debug.WriteLine("FrameworkType isn't defined!");
                Console.WriteLine("FrameworkType isn't defined!");
                return null;
            }
            if (!IsExecutable())
            {
                Debug.WriteLine("The solution does not provide an executable output!");
                Console.WriteLine("The solution does not provide an executable output!");
                return null;
            }
            string fileName = SolutionPath[SolutionPath.LastIndexOf('\\')..];
            string executablePath = $"{CsprojPath[..(CsprojPath.LastIndexOf('\\') + 1)]}" +
                $"bin\\Debug\\{FrameworkType}" +
                $"{fileName[..fileName.LastIndexOf('.')]}.exe";
            Debug.WriteLine(executablePath);
            return executablePath;
        }
    }
}