using System.Windows;
using Core;

namespace AppTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Call Shutdown to close the application
            Solution sol = new Solution("D:\\projects\\KomplexBeadando\\KomplexBeadando.sln");
            Application.Current.Shutdown();
        }
    }

}
