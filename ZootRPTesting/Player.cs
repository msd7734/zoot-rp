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
            get
            {
                return CopyProgData(this._health);
            }
            private set
            {
                this._health = value;
            }
        }

        public ProgressiveData<uint> Endurance
        {
            get
            {
                return CopyProgData(this._endurance);
            }
            private set
            {
                this._endurance = value;
            }
        }

        public ProgressiveData<uint> Dexterity
        {
            get
            {
                return CopyProgData(this._dexterity);
            }
            private set
            {
                this._dexterity = value;
            }
        }

        public ProgressiveData<uint> Ingenuity
        {
            get
            {
                return CopyProgData(this._ingenuity);
            }
            private set
            {
                this._ingenuity = value;
            }
        }

        public ProgressiveData<uint> Charisma
        {
            get
            {
                return CopyProgData(this._charisma);
            }
            private set
            {
                this._charisma = value;
            }
        }

        public ProgressiveData<uint> LevelExp
        {
            get
            {
                return CopyProgData(this._levelExp);
            }
            private set
            {
                this._levelExp = value;
            }
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

        #endregion

        public static readonly uint STAT_STARTING_MAX = 20;
        public static readonly uint STAT_MIN = 1;
        public static readonly uint STAT_MAX = 100;
        
        public static readonly uint LEVEL_MIN = 1;
        public static readonly uint LEVEL_MAX = 100;

        private static ProgressionType[] PROGRESSION_RATES =
        {
                ProgressionType.Fast,
                ProgressionType.Slow,
                ProgressionType.Average
        };

        private ProgressiveData<uint> _health;
        private ProgressiveData<uint> _endurance;
        private ProgressiveData<uint> _dexterity;
        private ProgressiveData<uint> _ingenuity;
        private ProgressiveData<uint> _charisma;
        private ProgressiveData<uint> _levelExp;

        public Player(string name)
        {
            Identifier = new PlayerIdentifier(Guid.NewGuid(), name);
            Level = 1;
            this._levelExp = new ProgressiveData<uint>(0, new LevelExpProgression());
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
            this._health = new ProgressiveData<uint>
            (
                health,
                new StatProgression(health, STAT_MAX, ProgressRateFromSeed(seed[0]))
            );

            this._endurance = new ProgressiveData<uint>
            (
                end,
                new StatProgression(end, STAT_MAX, ProgressRateFromSeed(seed[1]))
            );

            this._dexterity = new ProgressiveData<uint>
            (
                dex,
                new StatProgression(dex, STAT_MAX, ProgressRateFromSeed(seed[2]))
            );

            this._ingenuity = new ProgressiveData<uint>
            (
                ing,
                new StatProgression(ing, STAT_MAX, ProgressRateFromSeed(seed[3]))
            );

            this._charisma = new ProgressiveData<uint>
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

        private static ProgressionType ProgressRateFromSeed(byte seed)
        {
            return PROGRESSION_RATES[seed % PROGRESSION_RATES.Length];
        }

        public uint GetHealth()
        {
            return this._health.Value;
        }

        public uint GetEndurance()
        {
            return this._endurance.Value;
        }

        public uint GetDexterity()
        {
            return this._dexterity.Value;
        }

        public uint GetIngenuity()
        {
            return this._ingenuity.Value;
        }

        public uint GetCharisma()
        {
            return this._charisma.Value;
        }

        public uint GetLevelExp()
        {
            return this._levelExp.Value;
        }

        public ProgressiveData<uint> GetProgressDataHealth()
        {
            return this._health;
        }

        public uint ExpToNextLevel()
        {
            if (Level >= LEVEL_MAX)
            {
                return 0;
            }
            else
            {
                uint toNext = LevelExp.Progression.ValueAt(Level);
                return (toNext > GetLevelExp()) ? (toNext - GetLevelExp()) : 0;
            }
        }

        public void GiveReward(IReward reward)
        {
            AwardLevelExp(reward.LevelExp);
            AwardMoney(reward.Money);
        }

        public void AwardLevelExp(uint exp)
        {
            this._levelExp.Value += exp;
            HandleLevelUp();
        }

        public void AwardMoney(ulong money)
        {
            Money += money;
        }

        private void HandleLevelUp()
        {
            while (ExpToNextLevel() == 0 && Level < LEVEL_MAX)
            {
                IPlayer prev = this;

                // rollover exp
                uint oldExp = LevelExp.Value;
                this._levelExp.Value = 0;
                uint rollover = oldExp - ExpToNextLevel();
                this._levelExp.Value = rollover;

                Level += 1;

                LevelUpStats();

                var args = new PlayerUpdateEventArgs(prev);
                LevelUpEvent.Invoke(this, args);
            }
        }

        private void LevelUpStats()
        {
            this._health.ProgressToValue(Level);
            this._endurance.ProgressToValue(Level);
            this._dexterity.ProgressToValue(Level);
            this._ingenuity.ProgressToValue(Level);
            this._charisma.ProgressToValue(Level);
        }

        private ProgressiveData<uint> CopyProgData(ProgressiveData<uint> obj)
        {
            return new ProgressiveData<uint>(obj.Value, obj.Progression);
        }
    }
}
