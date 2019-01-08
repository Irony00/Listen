using System;
using System.Text.RegularExpressions;

namespace Listen
{
    public class SpotifyPlayer
    {
        private const string SpotifyUriRegexPattern =
            @"^(https:\/\/open.spotify.com\/user\/spotify\/playlist\/|spotify:track:|spotify:album:)([a-zA-Z0-9]+)(.*)$";

        private const string ProcessName = "Spotify";
        private readonly SpotifyClient _client;

        public SpotifyPlayer(SpotifyClient client)
        {
            _client = client;
        }

        public void HandleClipboardUpdate(object _, string clipboardText)
        {
            Parse(clipboardText);
        }

        private static bool ActiveWindowIsSpotify() => ProcessMonitor.GetActiveProcessName() == ProcessName;

        private static bool IsSpotifyUri(string uri)
        {
            var match = Regex.Match(uri.Trim(), SpotifyUriRegexPattern, RegexOptions.IgnoreCase);
            Console.WriteLine($"Match = {match.Success}");
            return match.Success;
        }

        private void Parse(string clipboardInput)
        {
            if (!IsSpotifyUri(clipboardInput) || ActiveWindowIsSpotify())
            {
                return;
            }

            var spotifyUri = clipboardInput;

            switch (spotifyUri)
            {
                case string trackUri when trackUri.Contains("track"):
                    _client.PlayTrack(spotifyUri);
                    break;
                default:
                    Console.WriteLine($"Not doing anything with: {spotifyUri}");
                    break;
            }
        }

        private static void ShowDialog(string track)
        {
            var dialog = new Dialog(track);
            dialog.ShowDialog();
        }
    }
}