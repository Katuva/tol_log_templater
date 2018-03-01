using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ToL_Log_Templater
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;

        private const int WS_MINIMIZEBOX = 0x20000;

        private Template template;

        public Editor()
        {
            InitializeComponent();

            template = new Template();

            DataContext = template;
        }

        public Editor(Template template)
        {
            InitializeComponent();

            this.template = template;

            DataContext = this.template;

            txtTemplateName.IsEnabled = false;
        }

        private IntPtr _windowHandle;
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            _windowHandle = new WindowInteropHelper(this).Handle;

            DisableMinimizeButton();
        }

        protected void DisableMinimizeButton()
        {
            if (_windowHandle == IntPtr.Zero)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            SetWindowLong(_windowHandle, GWL_STYLE, GetWindowLong(_windowHandle, GWL_STYLE) & ~WS_MINIMIZEBOX);
        }

        private void SaveFile(string filename, bool append = false)
        {
            using (StreamWriter writer = new StreamWriter(filename, append))
            {
                writer.WriteLine("###NAME_START###");
                writer.WriteLine(txtTemplateName.Text);
                writer.WriteLine("###NAME_END###");
                writer.WriteLine("###PAGE1_START###");
                writer.WriteLine(txtPage1.Text);
                writer.WriteLine("###PAGE1_END###");
                writer.WriteLine("###PAGE2_START###");
                writer.WriteLine(txtPage2.Text);
                writer.WriteLine("###PAGE2_END###");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // This is nasty, temporary until I can be bothered to create proper models/event/mvvm crap.
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            if (template.Filename == null)
            {
                template.Filename = "templates\\" + Guid.NewGuid().ToString();

                // yuck
                mainWindow.Templates.Add(template);
            }
            else
            {
                // vomits
                mainWindow.Templates[mainWindow.cmbTemplates.SelectedIndex].Page1 = template.Page1;
                mainWindow.Templates[mainWindow.cmbTemplates.SelectedIndex].Page2 = template.Page2;
            }

            Directory.CreateDirectory("templates");

            SaveFile(template.Filename);

            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (template.Filename == null)
                txtTemplateName.Focus();
            else
                txtPage1.Focus();
        }

        private void btnUnicode_Click(object sender, RoutedEventArgs e)
        {
            Unicode unicodeWindow = new Unicode();
            unicodeWindow.Owner = this;

            unicodeWindow.Show();
        }
    }
}
