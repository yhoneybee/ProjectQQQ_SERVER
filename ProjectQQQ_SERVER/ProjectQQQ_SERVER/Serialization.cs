using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectQQQ_SERVER
{
    [Serializable]
    public class Serialization<T>
    {
        public Serialization(List<T> targets) => target = targets;
        public List<T> target;
    }
}
