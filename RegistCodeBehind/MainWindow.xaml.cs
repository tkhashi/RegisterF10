using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace RegistCodeBehind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DisplayPage _displayPageControl;

        public MainWindow()
        {
            InitializeComponent();
            _displayPageControl = this.DisplayPageControl;
        }

        // F10の生入力を検知する用
        private IntPtr GetWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WmInput) InputWm(lParam);

            return IntPtr.Zero;
        }

        private void InputWm(IntPtr lParam)
        {
            var window = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(s => s.IsActive);
            if (window == null) return;

            var headerSize = Marshal.SizeOf(typeof(RawInputHeader));
            var size = Marshal.SizeOf(typeof(RawInput));
            GetRawInputData(lParam, 0x10000003, out RawInput input, ref size, headerSize);

            var vm = (DisplayPageViewModel) _displayPageControl.DataContext;
            var isF10 = input.Keyboard.VKey == VkF10;
            var isKeyDown = input.Keyboard.Message == WmKeyDown;
            var canExecuteFunction10 = vm.F10Command.CanExecute();
            if (isF10 && isKeyDown && canExecuteFunction10)
            {
                vm.F10Command.Execute();
            }
        }

        private const int WmInput = 0xFF;
        private const int WmKeyDown = 0x100;
        private const int VkF10 = 0x79;

        [DllImport("user32.dll")]
        private static extern int RegisterRawInputDevices(
            RawInputDevice[] devices,
            int number,
            int size);

        [DllImport("user32.dll")]
        private static extern int GetRawInputData(
            IntPtr rawInput,
            int command,
            out RawInput data,
            ref int size,
            int headerSize);

        private struct RawInputDevice
        {
            public short UsagePage;
            public short Usage;
            public int Flags;
            public IntPtr Target;
        }

        private struct RawInputHeader
        {
            public int Type;
            public int Size;
            public IntPtr Device;
            public IntPtr WParam;
        }

        private struct RawInput
        {
            public RawInputHeader Header;
            public RawKeyboard Keyboard;
        }

        private struct RawKeyboard
        {
            public short MakeCode;
            public short Flags;
            public short Reserved;
            public short VKey;
            public int Message;
            public long ExtrInformation;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var handle = new WindowInteropHelper(this).Handle;
            var devices = new RawInputDevice[1];
            // デスクトップへの入力を指定
            devices[0].UsagePage = 0x01;
            // キーボードからの入力を指定 
            devices[0].Usage = 0x06;
            // ウィンドウが前面にいない場合もTargetへの入力を受け取るよう指定
            devices[0].Flags = 0x00000100;
            // 生入力を受け取るウィンドウの指定
            devices[0].Target = handle;
            RegisterRawInputDevices(devices, 1, Marshal.SizeOf(typeof(RawInputDevice)));
            // GetWndProcフック
            var hwndSourceHook = new HwndSourceHook(GetWndProc);
            var hwndSource = (HwndSource) PresentationSource.FromVisual(this);
            hwndSource?.AddHook(hwndSourceHook);
        }
    }
}