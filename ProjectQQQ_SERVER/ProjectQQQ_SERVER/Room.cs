using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectQQQ_SERVER
{
    [Serializable]
    class Room
    {
        public Room(string name, string pw)
        {
            this.name = name;
            this.pw = pw;
        }
        public string name, pw;
        public int id;
        public List<Client> clients = new List<Client>();
    }
}
