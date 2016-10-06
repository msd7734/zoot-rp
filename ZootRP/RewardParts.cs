using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public class RewardParts
    {
        private Dictionary<RewardType, object> _dict;

        public RewardParts() { }

        public void Add(RewardType type, object reward)
        {
            this._dict.Add(type, reward);
        }

        public List<KeyValuePair<RewardType, object>> GetAll()
        {
            var res = new List<KeyValuePair<RewardType, object>>();
            foreach (var kvp in this._dict)
            {
                res.Add(kvp);
            }
            return res;
        }
    }
}
