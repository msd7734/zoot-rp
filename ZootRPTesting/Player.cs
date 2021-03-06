﻿using System;
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
        public event PlayerStateChange RewardEvent;

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

        private static readonly ProgressionType[] PROGRESSION_RATES =
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
            Money = 0;

            GenerateRandomFeatures();
        }

        private void GenerateRandomFeatures()
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

            this.Character = new Character(Identifier.CanonicalName, new Species(seed[10]));
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

        public IProgression<uint> GetHealthProgression()
        {
            return this._health.Progression;
        }

        public IProgression<uint> GetEnduranceProgression()
        {
            return this._endurance.Progression;
        }

        public IProgression<uint> GetDexterityProgression()
        {
            return this._dexterity.Progression;
        }

        public IProgression<uint> GetIngenuityProgression()
        {
            return this._ingenuity.Progression;
        }

        public IProgression<uint> GetCharismaProgression()
        {
            return this._ingenuity.Progression;
        }

        public IProgression<uint> GetLevelExpProgression()
        {
            return this._levelExp.Progression;
        }

        public uint ExpToNextLevel()
        {
            if (Level >= LEVEL_MAX)
            {
                return 0;
            }
            else
            {
                uint toNext = this._levelExp.Progression.ValueAt(Level);
                return (toNext > GetLevelExp()) ? (toNext - GetLevelExp()) : 0;
            }
        }

        public void GiveReward(IReward reward)
        {
            // Just reporting the reward does not care about the previous state
            PlayerMutableState prevState = new PlayerMutableState(this);
            PlayerUpdateEventArgs args = new PlayerUpdateEventArgs(prevState, reward.Description);
            RewardEvent.Invoke(this, args);

            if ((reward.Type & RewardType.LevelExp) == RewardType.LevelExp)
            {
                AwardLevelExp((uint)reward.GetRewardValue(RewardType.LevelExp));
            }

            if ((reward.Type & RewardType.Money) == RewardType.Money)
            {
                AwardMoney((ulong) reward.GetRewardValue(RewardType.Money));
            }

            // Late report, this is the "most valid" data, but will cause exp to be reported after a level-up
            // PlayerUpdateEventArgs args = new PlayerUpdateEventArgs(prevState, reward.Description);
            // RewardEvent.Invoke(this, args);
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
                // save state to pass through update event
                PlayerMutableState pms = new PlayerMutableState(this);

                // rollover exp
                uint oldExp = this._levelExp.Value;
                this._levelExp.Value = 0;
                uint rollover = oldExp - ExpToNextLevel();
                this._levelExp.Value = rollover;

                Level += 1;

                LevelUpStats();

                var args = new PlayerUpdateEventArgs(pms);
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
    }
}
