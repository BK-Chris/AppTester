using Core;
using System.Windows;

namespace AppTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Solution? solution;
        public App()
        {
            /* Test for Core.dll
            try
            {
                solution = new("D:\\projects\\KomplexBeadando\\KomplexBeadando.sln");
                string executablePath = solution.ExecutablePath;
                string input = "4 6\r\n10 10 10 10 10\r\n1 1 1 1 1\r\n0 0 0 0 10\r\n0 0 0 10 10\r\n0 0 10 10 10\r\n0 10 10 10 10\r\n10 10 10 10 10";
                string expectedOutput = "4";
                bool result = Tester.TestInputFromString(executablePath, input, expectedOutput).GetAwaiter().GetResult();

                Debug.WriteLine($"The result from direct input: {result}");

                string beInputs = "D:\\elte\\progalap\\beadandok\\komplex\\bunghardt_krisztian_b40t6a_komplex_masodik_fazis\\tesztek\\be";
                string kiInputs = "D:\\elte\\progalap\\beadandok\\komplex\\bunghardt_krisztian_b40t6a_komplex_masodik_fazis\\tesztek\\ki";

                string[] inputs = Directory.EnumerateFiles(beInputs).ToArray();
                string[] outputs = Directory.EnumerateFiles(kiInputs).ToArray();

                for (int i = 0; i < inputs.Length; i++)
                {
                    result = Tester.TestInputFromFile(executablePath, inputs[i], outputs[i]).GetAwaiter().GetResult();
                    Debug.WriteLine($"The result of test #{i+1}: {result}");
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Console.WriteLine(ex.Message);
            }

            Application.Current.Shutdown();
            */
        }
    }

}
