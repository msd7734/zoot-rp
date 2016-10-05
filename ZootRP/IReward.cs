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
        long Money { get; }
        long LevelExp { get; }
        String Description { get; }
    }
}
