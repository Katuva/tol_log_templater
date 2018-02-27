using System;
using System.ComponentModel;
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
        public BindingList<Template> Templates { get; set; }

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
                if ((pList.ProcessName == "ThroneOfLies") && (pList.MainModule.ModuleName == "ThroneOfLies.exe"))
                    return pList.MainWindowHandle;

            return IntPtr.Zero;
        }

        public void LoadTemplates()
        {
            Templates = new BindingList<Template>();

            foreach (string file in Directory.EnumerateFiles("templates"))
            {
                string contents = File.ReadAllText(file);

                var regex = new Regex("###NAME_START###(.+?)###NAME_END###", RegexOptions.Singleline);
                var match = regex.Match(contents);
                string name = match.Groups[1].Value.Trim();

                regex = new Regex("###PAGE1_START###(.+?)###PAGE1_END###", RegexOptions.Singleline);
                match = regex.Match(contents);
                string page1 = match.Groups[1].Value.Trim(Environment.NewLine.ToCharArray());

                regex = new Regex("###PAGE2_START###(.+?)###PAGE2_END###", RegexOptions.Singleline);
                match = regex.Match(contents);
                string page2 = match.Groups[1].Value.Trim(Environment.NewLine.ToCharArray());

                Templates.Add(new Template(file, name, page1, page2));
            }
        }

        private bool FocusGame()
        {
            IntPtr iPtrHwnd;

            iPtrHwnd = WinGetHandle();

            if (iPtrHwnd == IntPtr.Zero)
            {
                System.Windows.MessageBox.Show("Throne of Lies doesn't appear to be running.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            SetForegroundWindow(iPtrHwnd);
            return true;
        }

        private void PasteTemplate(string page)
        {
            Template template = CheckTemplateSelected();

            if (template == null)
                return;
            
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

        private Template CheckTemplateSelected()
        {
            Template template = (Template)cmbTemplates.SelectedItem;

            if (template != null)
            {
                return template;
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a template from the list.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return null;
            }
        }

        private void btnPage1_Click(object sender, RoutedEventArgs e)
        {
            if (FocusGame())
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
            if (FocusGame())
                PasteTemplate("page2");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Template template = CheckTemplateSelected();

            if (template == null)
                return;

            MessageBoxResult dialogResult = System.Windows.MessageBox.Show("Are you sure you want to delete " + template.Name + "?", "Confirm delete...", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (dialogResult == MessageBoxResult.Yes)
            {
                File.Delete(template.Filename);
                Templates.Remove((Template)cmbTemplates.SelectedItem);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Template template = CheckTemplateSelected();

            if (template == null)
                return;

            Editor editor = new Editor(template);
            editor.Owner = this;
            editor.ShowDialog();
        }
    }
}
