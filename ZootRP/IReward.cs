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

    public class Reward : IReward
    {
        public RewardType Type
        {
            get;
            private set;
        }

        public ulong Money
        { 
            get; 
            private set; 
        }

        public uint LevelExp
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public Reward(RewardType typeFlags, RewardParts rewards)
        {
            Description = String.Empty;
            Type = typeFlags;
            HandleRewardTypes(rewards);
        }

        public Reward(RewardType typeFlags, String description, RewardParts rewards)
        {
            Description = description;
            Type = typeFlags;
            HandleRewardTypes(rewards);
        }

        private void HandleRewardTypes(RewardParts parts)
        {
            var list = parts.GetAll();
            foreach (var kvp in list)
            {
                SetFromType(kvp.Key, kvp.Value);
            }
        }

        private void SetFromType(RewardType type, object reward)
        {
            switch (type)
            {
                case RewardType.Money:
                    Money = (ulong)reward;
                    break;
                case RewardType.LevelExp:
                    LevelExp = (uint)reward;
                    break;
                default:
                    return;
            }
        }
    }
}
