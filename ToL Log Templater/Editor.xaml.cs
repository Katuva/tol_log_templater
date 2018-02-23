using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        public Editor()
        {
            InitializeComponent();
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

        private void txtTemplateName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtTemplateName.SelectAll();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Guid id = Guid.NewGuid();

            Directory.CreateDirectory("templates");

            using (StreamWriter writer = new StreamWriter("templates/" + id))
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

            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtTemplateName.Focus();
        }
    }
}
