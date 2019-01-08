using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Listen
{
    public static class ClipboardNotifier
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private static readonly IntPtr HwndMessage = new IntPtr(-3);

        private const int WmClipboardupdate = 0x031D;

        public static event EventHandler<string> ClipboardUpdate;

        private static NotificationForm _form = new NotificationForm();

        private static void OnClipboardUpdate(string e)
        {
            var handler = ClipboardUpdate;

            handler?.Invoke(null, e);
        }

        private class NotificationForm : Form
        {
            public NotificationForm()
            {
                SetParent(Handle, HwndMessage);
                AddClipboardFormatListener(Handle);
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WmClipboardupdate)
                {
                    OnClipboardUpdate(Clipboard.GetText());
                }

                base.WndProc(ref m);
            }
        }
    }
}