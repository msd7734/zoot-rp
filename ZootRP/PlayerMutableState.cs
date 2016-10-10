using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ZootRP.Core
{
    /// <summary>
    /// Save a "snapshot" of all the mutable state of an IPlayer.
    /// Any methods that would mutate player state or access state that is intended to be immutable
    ///     will throw an ImmutableStateAccessException.
    /// </summary>
    public class PlayerMutableState : IPlayer
    {
        private readonly ulong _money;
        private readonly uint _level;
        private readonly uint _health;
        private readonly uint _endurance;
        private readonly uint _dexterity;
        private readonly uint _ingenuity;
        private readonly uint _charisma;
        private readonly uint _levelExp;
        private readonly uint _expToNextLvl;

        public PlayerMutableState(IPlayer player)
        {
            this._money = player.Money;
            this._level = player.Level;
            this._health = player.GetHealth();
            this._endurance = player.GetEndurance();
            this._dexterity = player.GetDexterity();
            this._ingenuity = player.GetIngenuity();
            this._charisma = player.GetCharisma();
            this._levelExp = player.GetLevelExp();
            this._expToNextLvl = player.ExpToNextLevel();
        }

        public event PlayerStateChange LevelUpEvent;

        public IPlayerIdentifier Identifier
        {
            get
            {
                throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
            }
        }

        public ICharacter Character
        {
            get
            {
                throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
            }
        }

        public IJob Job
        {
            get
            {
                throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
            }
        }

        public IResidence Residence
        {
            get
            {
                throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
            }
        }

        public ulong Money
        {
            get { return this._money; }
        }

        public uint Level
        {
            get { return this._level; }
        }

        public uint GetHealth()
        {
            return this._health;
        }

        public uint GetEndurance()
        {
            return this._endurance;
        }

        public uint GetDexterity()
        {
            return this._dexterity;
        }

        public uint GetIngenuity()
        {
            return this._ingenuity;
        }

        public uint GetCharisma()
        {
            return this._charisma;
        }

        public uint GetLevelExp()
        {
            return this._levelExp;
        }

        public uint ExpToNextLevel()
        {
            return this._expToNextLvl;
        }

        public IProgression<uint> GetHealthProgression()
        {
            throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
        }

        public IProgression<uint> GetEnduranceProgression()
        {
            throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
        }

        public IProgression<uint> GetDexterityProgression()
        {
            throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
        }

        public IProgression<uint> GetIngenuityProgression()
        {
            throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
        }

        public IProgression<uint> GetCharismaProgression()
        {
            throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
        }

        public IProgression<uint> GetLevelExpProgression()
        {
            throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
        }

        public void GiveReward(IReward reward)
        {
            throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
        }

        public void AwardLevelExp(uint exp)
        {
            throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
        }

        public void AwardMoney(ulong money)
        {
            throw new ImmutableStateAccessException(MethodBase.GetCurrentMethod());
        }
    }

    public class ImmutableStateAccessException : Exception
    {
        private static readonly string MSG = 
            "Attempted to access immutable state ({0}) in a snapshot of an IPlayer's mutable state.";

        public ImmutableStateAccessException(MethodBase method) :
            base(String.Format(MSG, method.ToString()))
        {
            
        }
    }
}
