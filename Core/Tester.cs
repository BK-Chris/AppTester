using System.Diagnostics;
using System.Text;

namespace Core
{
    /// <summary>
    /// Provides functionality to test executables by running them in the background, redirecting input and output, and verifying their behavior.
    /// </summary>
    public static class Tester
    {
        /// <summary>
        /// Runs an executable in the background, redirects its input and output, and compares the actual output to the expected output.
        /// </summary>
        /// <param name="executablePath">The full path to the executable to run. Cannot be null or empty.</param>
        /// <param name="input">Input to send to the executable's standard input. If null or empty, no input will be provided.</param>
        /// <param name="expectedOutput">The expected output to compare with the executable's actual output. The comparison is case-insensitive and trims leading/trailing whitespace.</param>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> that represents the asynchronous operation. 
        /// Returns <c>true</c> if the actual output matches the expected output, otherwise returns <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Input/output comparisons are case-insensitive and trim leading/trailing whitespace for accurate matching.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="executablePath"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the process fails to start or if an error occurs during execution.</exception>

        public static async Task<bool> TestInputFromString(string executablePath, string input, string expectedOutput)
        {
            if (!File.Exists(executablePath))
                throw new ArgumentNullException(nameof(executablePath), "Could not find executable!");
            ProcessStartInfo processStartInfo = new()
            {
                FileName = executablePath,
                RedirectStandardInput = true,   // Redirect standard input
                RedirectStandardOutput = true,  // Redirect standard output
                RedirectStandardError = true,   // Redirect standard error
                UseShellExecute = false,       // Must be false to redirect input/output
                CreateNoWindow = true          // Don't show a window for the process
            };

            try
            {
                using Process process = Process.Start(processStartInfo) ?? throw new InvalidOperationException("Failed to start the process.");

                // If input is provided, write it to the standard input stream of the process
                if (!string.IsNullOrEmpty(input))
                {
                    await process.StandardInput.WriteLineAsync(input);
                    process.StandardInput.Close();
                }

                // Read the output asynchronously
                StringBuilder outputBuilder = new();
                Task<string> outputTask = ReadOutputAsync(process.StandardOutput);
                Task<string> errorTask = ReadOutputAsync(process.StandardError);

                // Wait for the process to exit
                await process.WaitForExitAsync();

                string output = await outputTask;
                string error = await errorTask;

                if (process.ExitCode == 0)
                    return output.Trim().Equals(expectedOutput.Trim(), StringComparison.OrdinalIgnoreCase);
                else
                    throw new InvalidOperationException($"Process failed with exit code {process.ExitCode}: {error}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while running the executable: {ex.Message}");
                Console.WriteLine($"An error occurred while running the executable: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Reads input and expected output from files and runs an executable in the background, comparing the actual output to the expected output.
        /// </summary>
        /// <param name="executablePath">The full path to the executable to run. Cannot be null or empty.</param>
        /// <param name="inputPath">The path to the file containing input for the executable. The file must exist.</param>
        /// <param name="expectedOutputPath">The path to the file containing the expected output. The file must exist.</param>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> that represents the asynchronous operation. 
        /// Returns <c>true</c> if the actual output matches the expected output, otherwise returns <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Input/output comparisons are case-insensitive and trim leading/trailing whitespace for accurate matching.
        /// </remarks>
        /// <exception cref="FileNotFoundException">Thrown if either the input or expected output file does not exist.</exception>
        /// <exception cref="IOException">Thrown if an I/O error occurs while reading the files.</exception>
        /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
        public static async Task<bool> TestInputFromFile(string executablePath, string inputPath, string expectedOutputPath)
        {
            string caughtAt = "TestInputFromFile()";
            string input, expectedOutput;
            try
            {
                using (StreamReader sr = new(inputPath))
                {
                    input = sr.ReadToEnd();
                };
                using (StreamReader sr = new(expectedOutputPath))
                {
                    expectedOutput = sr.ReadToEnd();
                };
                return await Tester.TestInputFromString(executablePath, input, expectedOutput);
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
        /// Reads all lines asynchronously from a <see cref="StreamReader"/> and returns them as a single string.
        /// </summary>
        /// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
        /// <returns>A task that represents the asynchronous read operation, containing the full output as a string.</returns>
        private static async Task<string> ReadOutputAsync(StreamReader reader)
        {
            StringBuilder outputBuilder = new();
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                outputBuilder.AppendLine(line);
            }
            return outputBuilder.ToString();
        }
    }
}