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
        public Client(HostID hostID, string ID, string PW)
        {
            this.ID = ID;
            this.PW = PW;
            this.hostID = hostID;
        }
        public string ID, PW;
        public HostID hostID;
        public int roomNum;
    }
}
