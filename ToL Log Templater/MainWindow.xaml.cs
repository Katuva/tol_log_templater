using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;

namespace ToL_Log_Templater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Template> Templates { get; set; }

        public MainWindow()
        {
            LoadTemplates();

            InitializeComponent();

            DataContext = this;
        }

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static IntPtr WinGetHandle()
        {
            foreach (Process pList in Process.GetProcesses())
                if (pList.MainWindowTitle.Contains("Throne of Lies"))
                    return pList.MainWindowHandle;

            return IntPtr.Zero;
        }

        private void LoadTemplates()
        {
            Templates = new List<Template>();

            foreach (string file in Directory.EnumerateFiles("templates"))
            {
                string contents = File.ReadAllText(file);

                var regex = new Regex("###NAME_START###(.+?)###NAME_END###", RegexOptions.Singleline);
                var match = regex.Match(contents);
                string name = match.Groups[1].Value.Trim();

                regex = new Regex("###PAGE1_START###(.+?)###PAGE1_END###", RegexOptions.Singleline);
                match = regex.Match(contents);
                string page1 = match.Groups[1].Value.Trim();

                regex = new Regex("###PAGE2_START###(.+?)###PAGE2_END###", RegexOptions.Singleline);
                match = regex.Match(contents);
                string page2 = match.Groups[1].Value.Trim();

                Templates.Add(new Template(file, name, page1, page2));
            }
        }

        private void FocusGame()
        {
            IntPtr iPtrHwnd;

            iPtrHwnd = WinGetHandle();

            if (iPtrHwnd == IntPtr.Zero)
            {
                System.Windows.MessageBox.Show("Throne of Lies doesn't appear to be running.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            SetForegroundWindow(iPtrHwnd);
        }

        private void PasteTemplate(string page)
        {
            Template template = (Template)cmbTemplates.SelectedItem;

            switch (page)
            {
                case "page1":
                    System.Windows.Clipboard.SetText(template.Page1);
                    break;
                case "page2":
                    System.Windows.Clipboard.SetText(template.Page2);
                    break;
                default:
                    break;
            }

            SendKeys.SendWait("^{v}");
        }

        private void btnPage1_Click(object sender, RoutedEventArgs e)
        {
            FocusGame();
            PasteTemplate("page1");
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Editor editor = new Editor();
            editor.Owner = this;
            editor.ShowDialog();
        }

        private void btnPage2_Click(object sender, RoutedEventArgs e)
        {
            FocusGame();
            PasteTemplate("page2");
        }
    }
}
