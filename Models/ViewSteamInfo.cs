using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTools.Models
{
    public class ViewSteamInfo : BaseNotifyModel
    {
        private string name;
        private long changeNumber;
        private DateTime timeUpdate;
        private int id;

        public int Id
        {
            get => id; set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => name; set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public long ChangeNumber
        {
            get => changeNumber; set
            {
                changeNumber = value;
                OnPropertyChanged();
            }
        }
        public DateTime TimeUpdate
        {
            get => timeUpdate; set
            {
                timeUpdate = value;
                OnPropertyChanged();
            }
        }
    }
}
