using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    public class Reward : IReward
    {
        public RewardType Type
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        private RewardParts _parts;

        public Reward(RewardParts rewards)
        {
            RewardType typeFlags = RewardType.None;
            foreach (var kv in rewards.GetAll())
            {
                typeFlags = typeFlags | kv.Key;
            }

            if (typeFlags == RewardType.None)
            {
                Description = String.Empty;
            }
            else
            {
                SetDescription(rewards);
            }

            Type = typeFlags;

            this._parts = rewards;
        }

        public Reward(RewardParts rewards, string description)
        {
            RewardType typeFlags = RewardType.None;
            foreach (var kv in rewards.GetAll())
            {
                typeFlags = typeFlags | kv.Key;
            }

            Description = description;
            Type = typeFlags;

            this._parts = rewards;
        }

        public object GetRewardValue(RewardType type)
        {
            return this._parts.Get(type);
        }

        private void SetDescription(RewardParts rewards)
        {
            StringBuilder builder = new StringBuilder();
            bool first = true;
            string toAdd = String.Empty;
            foreach (var kv in rewards.GetAll())
            {
                toAdd = String.Format("{0} {1}", kv.Value, StringLib.RewardTypeStrings[kv.Key]);
                if (!first)
                {
                    toAdd = ", " + toAdd;
                }
                else
                {
                    first = false;
                }
                builder.Append(toAdd);
            }

            Description = builder.ToString();
        }
    }
}
