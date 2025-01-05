using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace AppTester.Utils
{
    public static class FileManager
    {
        public static string GetFile(string extension)
        {
            OpenFileDialog dialog = new()
            {
                Filter = $"All files(*.{extension}) | *.{extension}",
                Multiselect = false
            };
            if (dialog.ShowDialog() == false)
            {
                Console.WriteLine($"No file was selected!");
                return string.Empty;
            }
            string path = dialog.FileName;
            Console.WriteLine($"Selected path: {path}");
            return path;
        }
        public static string[] GetFiles(string extension)
        {
            OpenFileDialog dialog = new()
            {
                Filter = $"All files(*.{extension}) | *.{extension}",
                Multiselect = true
            };
            if (dialog.ShowDialog() == false)
            {
                Console.WriteLine($"No files were selected!");
                return [];
            }
            string[] paths = dialog.FileNames;
            foreach (string path in paths)
            {
                Console.WriteLine($"Selected path: {path}");
            }
            return paths;
        }
        public static string[] GetFilesFromFolder(string extension)
        {
            OpenFolderDialog dialog = new();
            if (dialog.ShowDialog() == false)
            {
                Console.WriteLine($"No folder was selected!");
                return [];
            }
            try
            {
                string path = dialog.FolderName;
                string extensionPattern = @$"\.{extension}$";
                Console.WriteLine($"Fetching .{extension} files from {path}...");
                string[] paths = Directory.GetFiles(path)
                                  .Where(file => Regex.IsMatch(file, extensionPattern, RegexOptions.IgnoreCase))
                                  .ToArray();
                foreach (string p in paths)
                {
                    Console.WriteLine($"{p}");
                }
                return paths;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }
    }
}
