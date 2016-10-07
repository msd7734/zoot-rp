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

        public ProgressiveData<uint> Health
        {
            get;
            private set;
        }

        public ProgressiveData<uint> Endurance
        {
            get;
            private set;
        }

        public ProgressiveData<uint> Dexterity
        {
            get;
            private set;
        }

        public ProgressiveData<uint> Ingenuity
        {
            get;
            private set;
        }

        public ProgressiveData<uint> Charisma
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

        public static readonly uint STAT_STARTING_MAX = 20;
        public static readonly uint STAT_MIN = 1;
        public static readonly uint STAT_MAX = 100;
        
        public static readonly uint LEVEL_MIN = 1;
        public static readonly uint LEVEL_MAX = 100;

        private static ProgressionRate[] PROGRESSION_RATES =
        {
                ProgressionRate.Fast,
                ProgressionRate.Slow,
                ProgressionRate.Average
        };

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
            uint health, end, dex, ing, chr;

            // Starting values of stats
            health = StatFromSeeds(seed[0], seed[1]);
            end = StatFromSeeds(seed[2], seed[3]);
            dex = StatFromSeeds(seed[4], seed[5]);
            ing = StatFromSeeds(seed[6], seed[7]);
            chr = StatFromSeeds(seed[8], seed[9]);

            // set randomized progression rates for each stat
            Health = new ProgressiveData<uint>
            (
                health,
                new StatProgression(health, STAT_MAX, ProgressRateFromSeed(seed[0]))
            );

            Endurance = new ProgressiveData<uint>
            (
                end,
                new StatProgression(end, STAT_MAX, ProgressRateFromSeed(seed[1]))
            );

            Dexterity = new ProgressiveData<uint>
            (
                dex,
                new StatProgression(dex, STAT_MAX, ProgressRateFromSeed(seed[2]))
            );

            Ingenuity = new ProgressiveData<uint>
            (
                ing,
                new StatProgression(ing, STAT_MAX, ProgressRateFromSeed(seed[3]))
            );

            Charisma = new ProgressiveData<uint>
            (
                chr,
                new StatProgression(chr, STAT_MAX, ProgressRateFromSeed(seed[4]))
            );

            // use remaining bytes to determine something else?
        }

        private uint StatFromSeeds(byte b1, byte b2)
        {
            return (uint) ((b1 ^ b2) % STAT_STARTING_MAX) + 1;
        }

        private static ProgressionRate ProgressRateFromSeed(byte seed)
        {
            return PROGRESSION_RATES[seed % PROGRESSION_RATES.Length];
        }

        public uint GetHealth()
        {
            return Health.Value;
        }
        public uint GetEndurance()
        {
            return Endurance.Value;
        }
        public uint GetDexterity()
        {
            return Dexterity.Value;
        }
        public uint GetIngenuity()
        {
            return Ingenuity.Value;
        }
        public uint GetCharisma()
        {
            return Charisma.Value;
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
