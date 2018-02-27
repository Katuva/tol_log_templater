using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ToL_Log_Templater
{
    /// <summary>
    /// Interaction logic for Unicode.xaml
    /// </summary>
    public partial class Unicode : Window
    {
        public Unicode()
        {
            InitializeComponent();

            LoadButtons();
        }

        private void LoadButtons()
        {
            string[] lines = File.ReadAllLines("unicode");

            foreach (string line in lines)
            {
                Button btnUnicode = new Button();
                btnUnicode.Content = line.Trim();
                btnUnicode.Width = 44;
                btnUnicode.Height = 34;

                btnUnicode.Click += UnicodeButton_Click;

                spContainer.Children.Add(btnUnicode);
            }
        }

        void UnicodeButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText((sender as Button).Content.ToString());
        }

        private void btnUnicodeEdit_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("notepad.exe", "unicode");
        }
    }
}
