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
        event PlayerStateChange LevelUpEvent;

        IPlayerIdentifier Identifier { get; set; }
        ICharacter Character { get; set; }
        IJob Job { get; set; }
        
        uint Health { get; set; }
        uint Endurance { get; set; }
        uint Dexterity { get; set; }
        uint Ingenuity { get; set; }
        uint Charisma { get; set; }
        
        ulong Money { get; set; }
        
        uint Level { get; set; }
        ulong ExpToNextLevel { get; set; }
        ulong LevelExp { get; set; }

        void AwardExp(uint exp);
    }
}
