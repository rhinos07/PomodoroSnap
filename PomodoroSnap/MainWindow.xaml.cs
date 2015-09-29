using System;
using System.Runtime.InteropServices;
using System.Windows;

using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace PomodoSnap
{
    //public static class IconHelper
    //{
    //    [DllImport("user32.dll")]
    //    static extern int GetWindowLong(IntPtr hwnd, int index);

    //    [DllImport("user32.dll")]
    //    static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    //    [DllImport("user32.dll")]
    //    static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x,
    //int y, int width, int height, uint flags);

    //    [DllImport("user32.dll")]
    //    static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr
    //lParam);

    //    const int GWL_EXSTYLE = -20;
    //    const int WS_EX_DLGMODALFRAME = 0x0001;
    //    const int SWP_NOSIZE = 0x0001;
    //    const int SWP_NOMOVE = 0x0002;
    //    const int SWP_NOZORDER = 0x0004;
    //    const int SWP_FRAMECHANGED = 0x0020;
    //    const uint WM_SETICON = 0x0080;

    //    public static void RemoveIcon(Window window)
    //    {
    //        // Get this window's handle
    //        IntPtr hwnd = new WindowInteropHelper(window).Handle;
    //        // Change the extended window style to not show a window icon
    //        int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
    //        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);
    //        // Update the window's non-client area to reflect the changes
    //        SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
    //    }
    //}

        /// <summary>
        /// Interaktionslogik für MainWindow.xaml
        /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer _timer;
        TimeSpan _time;
        bool _shouldrun = false;
        System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();


        public MainWindow()
        {
            InitializeComponent();

            _time = TimeSpan.FromMinutes(5);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                UpdateTime();


            }, Application.Current.Dispatcher);

            _timer.Start();

            System.IO.Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/PomodoroSnap;component/alarm.ico")).Stream;
            this.nIcon.Icon = new System.Drawing.Icon(iconStream);
            iconStream.Dispose();
            this.nIcon.Visible = true;

            this.Start.Focus();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            //IconHelper.RemoveIcon(this);
        }

        private void UpdateTime()
        {
            if (_shouldrun)
            {
                if (_time > TimeSpan.Zero)
                {
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                    tbTime.Foreground = Brushes.Green;
                }

                if (_time == TimeSpan.Zero)
                {
                    if (tbTime.Foreground != Brushes.Red)
                    {
                        tbTime.Foreground = Brushes.Red;
                        this.nIcon.ShowBalloonTip(5000, "Hi", "Pomodoro timed out", System.Windows.Forms.ToolTipIcon.Info);
                    }
                }
            }
            else
                tbTime.Foreground = Brushes.Gray;

            tbTime.Text = _time.ToString(@"mm\:ss");
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _shouldrun = !_shouldrun;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (!_shouldrun)
            {
                if (_time.Seconds != 0)
                {
                    _time = TimeSpan.FromMinutes(5);
                    return;
                }

                if (_time.Minutes % 5 > 0)
                {
                    _time = TimeSpan.FromMinutes(5);
                    return;
                }


                if (_time == TimeSpan.FromMinutes(25))
                    return;

                _time = _time.Add(TimeSpan.FromMinutes(5));

                UpdateTime();
            }

            _shouldrun = false;
            
        }

        private void tbTime_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Start_Click(sender, e);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
