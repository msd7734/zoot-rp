using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    public class Player : IPlayer
    {

        public event PlayerStateChange LevelUpEvent;

        #region Accessors

        public IPlayerIdentifier Identifier
        {
            get;
            private set;
        }

        public ICharacter Character
        {
            get;
            private set;
        }

        public IJob Job
        {
            get;
            private set;
        }

        public IResidence Residence
        {
            get;
            private set;
        }

        public uint Health
        {
            get;
            private set;
        }

        public uint Endurance
        {
            get;
            private set;
        }

        public uint Dexterity
        {
            get;
            private set;
        }

        public uint Ingenuity
        {
            get;
            private set;
        }

        public uint Charisma
        {
            get;
            private set;
        }

        public ulong Money
        {
            get;
            private set;
        }

        public uint Level
        {
            get;
            private set;
        }

        public PlayerStats.Stat[] FastStats
        {
            get;
            private set;
        }

        public PlayerStats.Stat[] SlowStats
        {
            get;
            private set;
        }

        public PlayerStats.Stat[] AverageStats
        {
            get;
            private set;
        }

        public uint ExpToNextLevel
        {
            get
            {
                uint toNext = PlayerStats.ExpToNextLevel(Level);
                return (toNext > LevelExp) ? (toNext - LevelExp) : 0;
            }
        }

        public uint LevelExp
        {
            get;
            private set;
        }

        #endregion

        private static readonly int STAT_STARTING_MAX = 20;

        public Player(string name)
        {
            Identifier = new PlayerIdentifier(Guid.NewGuid(), name);
            Level = 1;
            LevelExp = 0;
            Money = 20;

            GenerateStats();
        }

        private void GenerateStats()
        {
            // Get a 16-byte array from guid
            byte[] seed = Identifier.Id.ToByteArray();

            Health = StatFromSeeds(seed[0], seed[1]);
            Endurance = StatFromSeeds(seed[2], seed[3]);
            Dexterity = StatFromSeeds(seed[4], seed[5]);
            Ingenuity = StatFromSeeds(seed[6], seed[7]);
            Charisma = StatFromSeeds(seed[8], seed[9]);

            // use remaining bytes to determine something else?

            var progressions = new List<List<PlayerStats.Stat>>()
            {
                new List<PlayerStats.Stat>(),
                new List<PlayerStats.Stat>(),
                new List<PlayerStats.Stat>()
            };
            var statsDict = PlayerStats.GetStatsDict(this);
            var keys = statsDict.Keys.ToArray();
            for (int i = 0; i < keys.Length; ++i)
            {
                progressions[seed[i] % 3].Add(keys[i]);
            }

            FastStats = progressions[0].ToArray();
            SlowStats = progressions[1].ToArray();
            AverageStats = progressions[2].ToArray();
        }

        private uint StatFromSeeds(byte b1, byte b2)
        {
            return (uint) ((b1 ^ b2) % STAT_STARTING_MAX) + 1;
        }

        public void GiveReward(IReward reward)
        {
            AwardLevelExp(reward.LevelExp);
            AwardMoney(reward.Money);
        }

        public void AwardLevelExp(uint exp)
        {
            LevelExp += exp;
        }

        public void AwardMoney(ulong money)
        {
            Money += money;
        }
    }
}
