using Newtonsoft.Json;
using SteamTools.GameIdModels;
using SteamTools.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamTools
{
    public class SteamApiService
    {
        private string _dataFilePath = null;
        public SteamApiService()
        {
            _dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"data.json");
        }
        public async Task<ViewSteamInfo> GetSteamInfoById(string gameId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
                HttpResponseMessage response;
                response = await client.GetAsync($"https://api.steamcmd.net/v1/info/{gameId}");
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();

                var jsonDeserialized = JsonConvert.DeserializeObject<Welcome>(json, new JsonSerializerSettings
                {
                    ContractResolver = new CustomContractResolver(gameId)
                });
                return new ViewSteamInfo()
                {
                    Id = Convert.ToInt32(gameId),
                    Name = jsonDeserialized.Data.DynamicProperty.Common.Name,
                    ChangeNumber = jsonDeserialized.Data.DynamicProperty.ChangeNumber,
                    TimeUpdate = DateTimeOffset.FromUnixTimeSeconds(jsonDeserialized.Data.DynamicProperty.Depots.Branches.Public.Timeupdated).DateTime.ToLocalTime()
                };
            }
        }
    
        public string[] ReadDataFromDisk()
        {
            var baseText = File.ReadAllText(_dataFilePath);
            if(string.IsNullOrEmpty(baseText)) return null;
            return JsonConvert.DeserializeObject<string[]>(baseText);
        }

        public void SaveToDisk(int[] listId)
        {
            File.WriteAllText(_dataFilePath,JsonConvert.SerializeObject(listId));
        }
    }
}
