using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Listen
{
    internal static class Program
    {
        private const string AccesToken = "";

        [STAThread]
        public static void Main()
        {
            var player = new SpotifyPlayer(new SpotifyClient(AccesToken));
            ClipboardNotifier.ClipboardUpdate += player.HandleClipboardUpdate;

            Application.Run();
        }
    }
}