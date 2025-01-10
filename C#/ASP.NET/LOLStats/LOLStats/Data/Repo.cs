

using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.Text.Json.Nodes;

namespace LOLStats.Data
{
    public class Repo:IRepo
    {
        private readonly string API_KEY = "RGAPI-5ef75a69-bdf7-405e-b489-62600a59d1a9";
       
       public async Task<string> GetPuuid()
        {
            string api = "https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/Distilled%20Water/OCE";
            var options = new RestClientOptions(api);
            
            RestClient client = new RestClient(options);
            var request = new RestRequest().AddHeader("X-Riot-Token",API_KEY);
            var response = await client.GetAsync(request);
            JObject jsonContent = JObject.Parse(response.Content);
            string puuid = jsonContent["puuid"].ToString();
            return puuid; 
        }

        public async Task<IEnumerable<string>> GetChampionMastery()
        {
            // not implemented yet 
            return null; 
        }

        public async Task<JToken> GetChampionInfo(string champion)
        {
            string api = "https://ddragon.leagueoflegends.com/cdn/14.21.1/data/en_US/champion/" + champion + ".json"; 
            var options = new RestClientOptions(api);
            RestClient client = new RestClient(options);
            var request = new RestRequest(api);
            var response = await client.GetAsync(request);
            JObject jsonContent = JObject.Parse(response.Content);
            var data = jsonContent["data"][champion]; 
            return data;
        }

        public async Task<JToken> GetAllChampions()
        {
            string api = "https://ddragon.leagueoflegends.com/cdn/14.21.1/data/en_US/champion.json";
            RestClient client = new RestClient();
            var request = new RestRequest(api);
            var response = await client.GetAsync(request);
            JObject JsonContent = JObject.Parse(response.Content);
            var data = JsonContent["data"];
            return data;
		}
    }
}
