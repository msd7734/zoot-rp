using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    public static class StringLib
    {
        public static readonly Dictionary<RewardType, string> RewardTypeStrings
            = new Dictionary<RewardType, string>()
        {
            { RewardType.None, String.Empty},
            { RewardType.Money, "Bucks" },
            { RewardType.LevelExp, "character EXP" }
        };
    }
}
