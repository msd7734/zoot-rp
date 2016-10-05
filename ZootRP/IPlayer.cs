using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public delegate void PlayerStateChange(IPlayer sender, EventArgs e);

    public interface IPlayer
    {
        public event PlayerStateChange LevelUpEvent;

        public IPlayerIdentifier Identifier;
        public ICharacter Character;
        public IJob Job;

        public uint Health;
        public uint Endurance;
        public uint Dexterity;
        public uint Ingenuity;
        public uint Charisma;

        public ulong Money;

        public uint Level;
        public ulong LevelExp;
        public ulong ExpToNextLevel;

        public void AwardExp(uint exp);
    }
}
