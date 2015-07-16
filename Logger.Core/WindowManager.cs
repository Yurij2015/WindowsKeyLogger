using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Logger.Core
{
    public class WindowManager
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hwnd, StringBuilder s, int nMaxCount);

        [DllImport("User32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static string GetActiveWindowTitle()
        {
            var hwnd = GetForegroundWindow();
            var length = GetWindowTextLength(hwnd);

            var sb = new StringBuilder(length + 1);
            var intLength = GetWindowText(hwnd, sb, sb.Capacity);

            if ((intLength <= 0) || (intLength > sb.Length))
            {
                return "unknown";
            }

            return sb.ToString();
        }
    }
}
