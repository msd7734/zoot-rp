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
        None,
        Money,
        LevelExp
    }

    public interface IReward
    {
        RewardType Type { get; }
        string Description { get; }

        object GetRewardValue(RewardType type);
    }

    
}
