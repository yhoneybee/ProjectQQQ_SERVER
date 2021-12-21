using Nettention.Proud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER
{
    class Client
    {
        public Client(HostID hostID, string ID, string nickName, string PW)
        {
            this.ID = ID;
            this.nickName = nickName;
            this.PW = PW;
            this.hostID = hostID;
        }
        public string ID, nickName, PW;
        public HostID hostID;
        public int roomNum;
    }
}
