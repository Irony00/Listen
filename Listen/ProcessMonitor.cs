using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Listen
{
    public static class ProcessMonitor
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint pid);

        public static string GetActiveProcessName()
        {
            var handle = GetForegroundWindow();
            GetWindowThreadProcessId(handle, out var pid);
            var process = Process.GetProcessById((int) pid);
            return process.ProcessName;
        }
    }
}