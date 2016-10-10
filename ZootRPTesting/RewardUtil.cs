using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    public static class RewardUtil
    {
        public static IReward CreateMoneyReward(ulong money)
        {
            RewardParts rp = new RewardParts();
            rp.Add(RewardType.Money, money);
            return new Reward(rp);
        }

        public static IReward CreateLevelExpReward(uint levelExp)
        {
            RewardParts rp = new RewardParts();
            rp.Add(RewardType.LevelExp, levelExp);
            return new Reward(rp);
        }

        public static IReward ComposeRewards(params IReward[] rewards)
        {
            RewardParts rp = new RewardParts();
            string description = String.Empty;
            foreach (var r in rewards)
            {
                rp.Add(r.Type, r.GetRewardValue(r.Type));
                description += r.Description + Environment.NewLine;
            }
            if (String.IsNullOrWhiteSpace(description))
            {
                return new Reward(rp);
            }
            else
            {
                return new Reward(rp, description);
            }
        }
    }
}
