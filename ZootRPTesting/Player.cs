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

        public uint ExpToNextLevel
        {
            get;
            private set;
        }

        public uint LevelExp
        {
            get;
            private set;
        }

        public void AwardExp(uint exp)
        {
            throw new NotImplementedException();
        }

        public void AwardMoney(ulong money)
        {
            throw new NotImplementedException();
        }
    }
}
