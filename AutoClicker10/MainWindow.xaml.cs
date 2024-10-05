using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace AutoClicker10
{
    public partial class MainWindow : Window
    {
        private POINT[] clickPositions = new POINT[10];  // 10 positions
        private Key[] hotkeys = new Key[10];  // 10 hotkeys
        private bool isRunning = false;

        public MainWindow()
        {
            InitializeComponent();

            // Set default positions and hotkeys for the positions 
            Pos1X.Text = "2000";
            Pos1Y.Text = "400";
            Hotkey1.Text = "NumPad0";

            Pos2X.Text = "2070";
            Pos2Y.Text = "400";
            Hotkey2.Text = "NumPad1";

            Pos3X.Text = "2140";
            Pos3Y.Text = "400";
            Hotkey3.Text = "NumPad2";

            Pos4X.Text = "2220";
            Pos4Y.Text = "400";
            Hotkey4.Text = "NumPad3";

            Pos5X.Text = "2290";
            Pos5Y.Text = "400";
            Hotkey5.Text = "NumPad4";

            Pos6X.Text = "2370";
            Pos6Y.Text = "400";
            Hotkey6.Text = "NumPad5";

            Pos7X.Text = "2450";
            Pos7Y.Text = "400";
            Hotkey7.Text = "NumPad6";

            Pos8X.Text = "2530";
            Pos8Y.Text = "400";
            Hotkey8.Text = "NumPad7";

            Pos9X.Text = "2600";
            Pos9Y.Text = "400";
            Hotkey9.Text = "NumPad8";

            Pos10X.Text = "2680";
            Pos10Y.Text = "400";
            Hotkey10.Text = "NumPad9";

            // Register Start (F) and Stop (G) hotkeys
            RegisterHotKey(new WindowInteropHelper(this).Handle, 10, 0, KeyInterop.VirtualKeyFromKey(Key.F));
            RegisterHotKey(new WindowInteropHelper(this).Handle, 11, 0, KeyInterop.VirtualKeyFromKey(Key.G));

            ComponentDispatcher.ThreadPreprocessMessage += HandleHotkeyPress;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Parse and store the click positions and hotkeys for all 10 positions
                clickPositions[0] = new POINT(int.Parse(Pos1X.Text), int.Parse(Pos1Y.Text));
                clickPositions[1] = new POINT(int.Parse(Pos2X.Text), int.Parse(Pos2Y.Text));
                clickPositions[2] = new POINT(int.Parse(Pos3X.Text), int.Parse(Pos3Y.Text));
                clickPositions[3] = new POINT(int.Parse(Pos4X.Text), int.Parse(Pos4Y.Text));
                clickPositions[4] = new POINT(int.Parse(Pos5X.Text), int.Parse(Pos5Y.Text));
                clickPositions[5] = new POINT(int.Parse(Pos6X.Text), int.Parse(Pos6Y.Text));
                clickPositions[6] = new POINT(int.Parse(Pos7X.Text), int.Parse(Pos7Y.Text));
                clickPositions[7] = new POINT(int.Parse(Pos8X.Text), int.Parse(Pos8Y.Text));
                clickPositions[8] = new POINT(int.Parse(Pos9X.Text), int.Parse(Pos9Y.Text));
                clickPositions[9] = new POINT(int.Parse(Pos10X.Text), int.Parse(Pos10Y.Text));

                // Parse and store the hotkeys for all 10 positions
                hotkeys[0] = (Key)Enum.Parse(typeof(Key), Hotkey1.Text, true);
                hotkeys[1] = (Key)Enum.Parse(typeof(Key), Hotkey2.Text, true);
                hotkeys[2] = (Key)Enum.Parse(typeof(Key), Hotkey3.Text, true);
                hotkeys[3] = (Key)Enum.Parse(typeof(Key), Hotkey4.Text, true);
                hotkeys[4] = (Key)Enum.Parse(typeof(Key), Hotkey5.Text, true);
                hotkeys[5] = (Key)Enum.Parse(typeof(Key), Hotkey6.Text, true);
                hotkeys[6] = (Key)Enum.Parse(typeof(Key), Hotkey7.Text, true);
                hotkeys[7] = (Key)Enum.Parse(typeof(Key), Hotkey8.Text, true);
                hotkeys[8] = (Key)Enum.Parse(typeof(Key), Hotkey9.Text, true);
                hotkeys[9] = (Key)Enum.Parse(typeof(Key), Hotkey10.Text, true);

                // Register hotkeys for all 10 positions
                for (int i = 0; i < hotkeys.Length; i++)
                {
                    RegisterHotKey(new WindowInteropHelper(this).Handle, i + 1, 0, KeyInterop.VirtualKeyFromKey(hotkeys[i]));
                }

                isRunning = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                for (int i = 1; i <= 10; i++)
                {
                    UnregisterHotKey(new WindowInteropHelper(this).Handle, i);
                }
                isRunning = false;
            }
        }

        private void HandleHotkeyPress(ref MSG msg, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg.message == WM_HOTKEY)
            {
                int id = msg.wParam.ToInt32();

                if (id >= 1 && id <= 10)
                {
                    int clickIndex = id - 1;
                    SetCursorPos(clickPositions[clickIndex].X, clickPositions[clickIndex].Y);
                    MouseClick();
                    SetCursorPos(960, 520);  // Reset cursor position
                }
                else if (id == 10)
                {
                    StartButton_Click(null, null);  // Start on F hotkey
                }
                else if (id == 11)
                {
                    StopButton_Click(null, null);  // Stop on G hotkey
                }
            }
        }

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", SetLastError = true)]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private void MouseClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            for (int i = 1; i <= 10; i++)
            {
                UnregisterHotKey(new WindowInteropHelper(this).Handle, i);
            }

            UnregisterHotKey(new WindowInteropHelper(this).Handle, 10);
            UnregisterHotKey(new WindowInteropHelper(this).Handle, 11);

            ComponentDispatcher.ThreadPreprocessMessage -= HandleHotkeyPress;
        }

        private struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
