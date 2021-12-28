using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER
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
        public int num;
        public List<Client> clients = new List<Client>();
    }
}
