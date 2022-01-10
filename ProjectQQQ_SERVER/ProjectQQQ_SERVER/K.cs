using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectQQQ_SERVER
{
    static class K
    {
        public static List<User> users = new List<User>();
        public static List<Room> rooms = new List<Room>();
        public static List<int> roomIDs = GetRandomInt(100, 0, 100);

        public static List<int> GetRandomInt(int count, int min, int max)
        {
            var randList = new List<int>();
            var resultList = new List<int>(count);

            Random random = new Random();

            for (var i = 0; i < max - min + 1; ++i)
                randList.Add(min + i);

            for (var i = 0; i < count; ++i)
            {
                var index = random.Next(min, max - i);
                resultList[i] = randList[index];
                randList.RemoveAt(index);
            }

            return resultList;
        }
    }
}
