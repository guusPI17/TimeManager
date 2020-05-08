using CalendarLight.Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Management;

namespace TimeMenedger
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            checkOneLogin();
            DisplayCalendar displayCalendar = new DisplayCalendar(getSidUser());

            border.Child = displayCalendar.display();
            Canvas.SetTop(border, 0);
        }

        #region Login

        private void checkOneLogin()
        {
            if (!File.Exists(@"sid.txt"))
            {
                FileStream fs = new FileStream(@"sid.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                fs.Close();
            }
        }
        private string getSidUser()
        {
            string fileName = @"sid.txt";
            Boolean answer = false;
            string sid = WindowsIdentity.GetCurrent().User.Value;
            string[] arStr = File.ReadAllLines(fileName);
            string allSid = string.Empty;
            foreach(string i in arStr)
            {
                if(i == sid)
                {
                    answer = true;
                    break;
                }
            }
            if (!answer)
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.Open, FileAccess.Write)))
                {
                    (sw.BaseStream).Seek(0, SeekOrigin.End);
                    sw.WriteLine(sid);
                }
            }
            return sid;
        }

        #endregion

        #region InvisibleWindow
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int value);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr window, int index);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public static void HideFromAltTab(IntPtr Handle)
        {
            SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle,
                GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }
        private void loadWind(object sender, EventArgs e)
        {
            HideFromAltTab(new WindowInteropHelper(this).Handle);
        }
        #endregion

        #region Bottommost

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOACTIVATE = 0x0010;

        private void ToBack()
        {
            var handle = new WindowInteropHelper(this).Handle;
            SetWindowPos(handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ToBack();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            ToBack();
        }
        
        #endregion

        #region Move

        private bool winDragged = false;
        private Point lmAbs = new Point();

        void Window_MouseDown(object sender, MouseEventArgs e)
        {
            winDragged = true;
            this.lmAbs = e.GetPosition(this);
            this.lmAbs.Y = Convert.ToInt16(this.Top) + this.lmAbs.Y;
            this.lmAbs.X = Convert.ToInt16(this.Left) + this.lmAbs.X;
            Mouse.Capture(this);
        }

        void Window_MouseUp(object sender, MouseEventArgs e)
        {
            winDragged = false;
            Mouse.Capture(null);
        }

        void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (winDragged)
            {
                Point MousePosition = e.GetPosition(this);
                Point MousePositionAbs = new Point();
                MousePositionAbs.X = Convert.ToInt16(this.Left) + MousePosition.X;
                MousePositionAbs.Y = Convert.ToInt16(this.Top) + MousePosition.Y;
                this.Left = this.Left + (MousePositionAbs.X - this.lmAbs.X);
                this.Top = this.Top + (MousePositionAbs.Y - this.lmAbs.Y);
                this.lmAbs = MousePositionAbs;
            }
        }
        #endregion
    
    }
}
