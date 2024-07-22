using MusicPlayerApp.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Diagnostics;
using System.ComponentModel;
using System.Windows;

namespace MusicPlayerApp.MVVM.ViewModel
{
    internal class MainViewModel: INotifyPropertyChanged
    {

        
        private static HttpListener _httpListener;
        private static String _accessToken;
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Item> _songs;
        public ObservableCollection<Item> Songs
        {
            get => _songs;
            set
            {
                _songs = value;
                OnPropertyChanged(nameof(Songs));
            }
        }

        public MainViewModel()
        {
            Songs = new ObservableCollection<Item>();
            StartHttpServer();

            
            string clientId = "9c548af21bb64c1da6ef6c9b5793780f";
            string redirectUri = "http://localhost:8888/callback";
            string scope = "user-read-private user-read-email";
            string authUrl = $"https://accounts.spotify.com/authorize?client_id={clientId}&response_type=code&redirect_uri={redirectUri}&scope={scope}";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = authUrl,
                UseShellExecute = true
            });

            Console.WriteLine("Waiting for authorization code...");
            Console.ReadLine();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StartHttpServer()
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://localhost:8888/callback/");
            _httpListener.Start();
            _httpListener.BeginGetContext(new AsyncCallback(OnRequestReceive), _httpListener);
        }

        private void OnRequestReceive(IAsyncResult result)
        {
            var context = _httpListener.EndGetContext(result);
            var request = context.Request;
            var response = context.Response;

            var query = request.QueryString;
            string authorizationCode = query["code"];

            Console.WriteLine($"Authorization Code: {authorizationCode}");

            // Send a response to the browser
            string responseString = "<html><body>Authorization code received. You can close this window.</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith(task =>
            {
                responseOutput.Close();
                _httpListener.Stop();
                Console.WriteLine("HTTP server stopped.");
            });

            // Now you can use the authorization code to get the access token
            GetAccessToken(authorizationCode).Wait();
        }
        private async Task GetAccessToken(string authorizationCode)
        {
            string clientId = "9c548af21bb64c1da6ef6c9b5793780f";
            string clientSecret = "d583cf844b2f44f8adfc75ad60d2d855";
            string redirectUri = "http://localhost:8888/callback";

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", authorizationCode),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret)
                };

                request.Content = new FormUrlEncodedContent(postData);

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                string accessToken = json.access_token;
                _accessToken = accessToken;
                await GetNewReleases();
            }
        }

        public async Task GetNewReleases()
        {
            String Url = "https://api.spotify.com/v1/browse/new-releases";

            RestClient client = new RestClient();
            var request = new RestRequest(Url, Method.Get);
            client.AddDefaultHeader("Authorization", $"Bearer {_accessToken}");

            
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<TrackModel>(response.Content);
                foreach(var song in data.Albums.Items)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Songs.Clear();
                        foreach (var item in data.Albums.Items)
                        {
                            Songs.Add(item);
                        }
                    });
                }
            }
            
            
       




        }
    
}
}
