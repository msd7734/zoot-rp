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

        public PlayerStat[] FastStats
        {
            get;
            private set;
        }

        public PlayerStat[] SlowStats
        {
            get;
            private set;
        }

        public PlayerStat[] AverageStats
        {
            get;
            private set;
        }

        public uint ExpToNextLevel
        {
            get
            {
                if (Level >= LEVEL_MAX)
                {
                    return 0;
                }
                else
                {
                    uint toNext = ((Level * Level) + (25 * Level) + 200) - 1;
                    return (toNext > LevelExp) ? (toNext - LevelExp) : 0;
                }
            }
        }

        public uint LevelExp
        {
            get;
            private set;
        }

        #endregion

        private static readonly uint STAT_STARTING_MAX = 20;
        private static readonly uint STAT_LEVEL_MAX = 50;
        private static readonly uint STAT_MIN = 1;
        private static readonly uint STAT_MAX = 100;

        private  static readonly uint LEVEL_MIN = 1;
        private  static readonly uint LEVEL_MAX = 99;

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

            var progressions = new List<List<PlayerStat>>()
            {
                new List<PlayerStat>(),
                new List<PlayerStat>(),
                new List<PlayerStat>()
            };
            var statsDict = PlayerUtil.GetStatsDict(this);
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
            HandleLevelUp();
        }

        public void AwardLevelExp(uint exp)
        {
            LevelExp += exp;
            HandleLevelUp();
        }

        public void AwardMoney(ulong money)
        {
            Money += money;
        }

        private void HandleLevelUp()
        {
            if (ExpToNextLevel == 0 && Level < LEVEL_MAX)
            {
                // rollover exp
                uint oldExp = LevelExp;
                LevelExp = 0;
                uint rollover = oldExp - ExpToNextLevel;
                LevelExp = rollover;

                Level += 1;

                LevelUpStats(Level - 1);
            }
        }

        private void LevelUpStats(uint previousLevel)
        {
            
        }
    }
}
