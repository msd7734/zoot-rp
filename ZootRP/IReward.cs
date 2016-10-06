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
        LevelExp
    }

    public interface IReward
    {
        RewardType Type { get; }
        ulong Money { get; }
         
        uint LevelExp { get; }
        string Description { get; }
    }

    
}
