using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Listen
{
    public class SpotifyClient
    {
        private class Parameters
        {
            public Parameters(string trackUri)
            {
                Uris.Add(trackUri);
            }

            [JsonProperty(PropertyName = "uris")] public List<string> Uris = new List<string>();
        }

        private const string PlayResumePlaybackEndpoint = "https://api.spotify.com/v1/me/player/play";
        private readonly string _accessToken;

        public SpotifyClient(string accessToken)
        {
            _accessToken = accessToken;
        }

        private static HttpClient CreateHttpClient(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return client;
        }

        private HttpResponseMessage DoRequest(string uri, HttpMethod method,
            HttpContent content = null)
        {
            using (var client = CreateHttpClient(_accessToken))
            {
                var requestMsg = new HttpRequestMessage(method, uri);
                if (content != null)
                {
                    requestMsg.Content = content;
                }

                return client.SendAsync(requestMsg).Result;
            }
        }

        public void PlayTrack(string trackUri)
        {
            Console.WriteLine($"Attempting to play: {trackUri}");
            var content = new StringContent(JsonConvert.SerializeObject(new Parameters(trackUri)), Encoding.UTF8,
                "application/json");

            var response = DoRequest(PlayResumePlaybackEndpoint, HttpMethod.Put, content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {(int)response.StatusCode} {response.StatusCode} Msg: Could not play track: {trackUri} ");
            }
        }
    }
}