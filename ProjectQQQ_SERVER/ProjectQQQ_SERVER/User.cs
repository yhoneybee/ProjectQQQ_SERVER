using Nettention.Proud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectQQQ_SERVER
{
    [Serializable]
    public class User
    {
        public User(HostID hostID, string? ID, string? PW)
        {
            this.ID = ID!;
            this.PW = PW!;
            this.hostID = hostID;
        }
        public float x, y, z;
        public bool isReady;
        public bool isHost;
        public string ID, PW;
        public HostID hostID;
        public int roomID;
    }
}
