using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    [Flags]
    public enum RewardType
    {
        Money,
        Exp
    }

    public interface IReward
    {
        public long Money;
        public long Exp;
        public String Description;
    }
}
