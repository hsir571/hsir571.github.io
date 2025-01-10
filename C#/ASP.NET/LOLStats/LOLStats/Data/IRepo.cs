using Newtonsoft.Json.Linq;
using RestSharp; 

namespace LOLStats.Data
{
    public interface IRepo
    {

        Task<string> GetPuuid();
        Task<IEnumerable<string>> GetChampionMastery();
        Task<JToken> GetChampionInfo(string champion);

		Task<JToken> GetAllChampions();
	}
}
