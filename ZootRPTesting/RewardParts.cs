using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    public class RewardParts
    {
        private Dictionary<RewardType, object> _dict;

        public RewardParts() 
        {
            this._dict = new Dictionary<RewardType, object>();
        }

        public void Add(RewardType type, object reward)
        {
            this._dict.Add(type, reward);
        }

        public object Get(RewardType type)
        {
            return this._dict[type];
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
