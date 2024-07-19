using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public String testing = "Im testing this out";
        private static HttpListener _httpListener;
        private static String _accessToken;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            StartHttpServer();

            // Open the Spotify authorization URL
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

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public String Test
        {
            get => testing;
            set
            {
                if (value == testing)
                {
                    return;
                }
                testing = value;
                OnPropertyChanged();
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Test = "wow i made it work";
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void StartHttpServer()
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("http://localhost:8888/callback/");
            _httpListener.Start();
            _httpListener.BeginGetContext(new AsyncCallback(OnRequestReceive), _httpListener);
        }

        private static void OnRequestReceive(IAsyncResult result)
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
        private static async Task GetAccessToken(string authorizationCode)
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
                await getNewReleases(); 
            }
        }

        private static async Task getNewReleases()
        {
            String Url = "https://api.spotify.com/v1/browse/new-releases";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
                var response = await client.GetAsync(Url);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(responseContent); 

                Console.WriteLine(json);


            }
        }
    }
}
