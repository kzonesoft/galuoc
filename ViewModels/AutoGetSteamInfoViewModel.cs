
using SteamTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SteamTools.ViewModels
{
    public class AutoGetSteamInfoViewModel : ThreadSafeBindingList<ViewSteamInfo>
    {
        private SteamApiService _service;
        private bool _isRunning = false;

        public Action<int,string> HandleMessage;

        public AutoGetSteamInfoViewModel(SynchronizationContext syncContext) : base(syncContext)
        {
            _service = new SteamApiService();
        }

        public void LoopCheck()
        {
            if (_isRunning) return;
            var listGameFromDisk = _service.ReadDataFromDisk();
            if(listGameFromDisk == null || listGameFromDisk.Length == 0)
            {
                HandleMessage?.Invoke(1,"Không tìm thấy list id. Stop Loop.");
            }
            var listGameId = new List<int>();
            foreach (var idText in listGameFromDisk)
            {
                listGameId.Add(Convert.ToInt32(idText));
            }

            Task unawait = Task.Run(() => FetchDataAndDisplay(listGameId));
        }

        private async Task FetchDataAndDisplay(IEnumerable<int> listGameId)
        {
            while (true)
            {
                HandleMessage?.Invoke(0,"Fetching...");
                foreach (var id in listGameId)
                {
                    var info = await _service.GetSteamInfoById(id.ToString());
                    var exist = this.Exist(x => x.Id == Convert.ToInt32(id));
                    if (!exist)
                        this.Add(info);
                    else
                    {
                        this.Edit(x => x.Id == id,z => 
                        {
                            z.ChangeNumber = info.ChangeNumber;
                            z.Name = info.Name;
                            z.TimeUpdate = info.TimeUpdate;
                        });
                       
                    }
                }
                HandleMessage?.Invoke(0, $"Fetch success from Steam server at : {DateTime.Now.ToLocalTime()}");
                await Task.Delay(TimeSpan.FromMinutes(15));
            }
        }
    }
}
