using Equin.ApplicationFramework;
using SteamTools.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamTools.ViewModels
{
    public class SteamInfoViewModel : ThreadSafeBindingList<ViewSteamInfo>
    {
        private SteamApiService _service;
        public SteamInfoViewModel(SynchronizationContext syncContext) : base(syncContext)
        {
            _service = new SteamApiService();
        }

        public void GetById(string id)
        {
            Task.Run(async () =>
            {

                var info = await _service.GetSteamInfoById(id);
                var current = this.GetOne(x => x.Id == Convert.ToInt32(id));
                if (current == null)
                    this.Add(info);
                else
                {
                    current.ChangeNumber = info.ChangeNumber;
                    current.Name = info.Name;
                    current.TimeUpdate = info.TimeUpdate;
                }

            });
        }

        public void Reload()
        {
            Task.Run(async () => {
                var listId = this.GetAll().Select(x => x.Id).ToArray();
                foreach (var id in listId)
                {
                    var info = await _service.GetSteamInfoById(id.ToString());
                    var current = this.GetOne(x => x.Id == Convert.ToInt32(id));
                    if (current == null)
                        this.Add(info);
                    else
                    {
                        current.ChangeNumber = info.ChangeNumber;
                        current.Name = info.Name;
                        current.TimeUpdate = info.TimeUpdate;
                    }
                }
            });
            
        }

        public void LoadData()
        {
            Task.Run(async () =>
            {
                var listId = _service.ReadDataFromDisk();
                if (listId == null) return;
                foreach (var item in listId)
                {
                    var viewData = await _service.GetSteamInfoById(item);
                    this.Add(viewData);
                }
            });
        }

        public void SaveData()
        {
            var listId = this.GetAll().Select(x => x.Id).ToArray();
            _service.SaveToDisk(listId);
        }

    }
}
