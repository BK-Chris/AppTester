using System.IO;
using System.Text;
using System.Windows.Controls;

namespace AppTester.Utils
{
    public class TextBoxWriter(TextBox textBox) : TextWriter
    {
        private readonly TextBox _textBox = textBox;
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            // Ensure UI updates occur on the main thread
            _textBox.Dispatcher.Invoke(() =>
            {
                _textBox.AppendText(value.ToString());
                _textBox.ScrollToEnd();
            });
        }

        public override void Write(string? value)
        {
            // Ensure UI updates occur on the main thread
            _textBox.Dispatcher.Invoke(() =>
            {
                _textBox.AppendText(value);
                _textBox.ScrollToEnd();
            });
        }
    }
}