using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectQQQ_SERVER
{
    [Serializable]
    public class Room
    {
        public Room(string? name, int id, string? pw)
        {
            this.name = name!;
            this.id = id;
            this.pw = pw!;
        }
        public string name, pw;
        public int id;
        public List<User> users = new List<User>();
    }
}
