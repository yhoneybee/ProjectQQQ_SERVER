using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectQQQ_SERVER
{
    [Serializable]
    public class RoomUser
    {
        public RoomUser(int roomID, string? userID)
        {
            this.roomID = roomID;
            this.userID = userID!;
        }

        public int roomID;
        public string userID;
    }
}
