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

        IPlayerIdentifier Identifier { get; }
        ICharacter Character { get; }
        IJob Job { get; }
        IResidence Residence { get; }
        
        uint Health { get; }
        uint Endurance { get; }
        uint Dexterity { get; }
        uint Ingenuity { get; }
        uint Charisma { get; }
        
        PlayerStat[] FastStats { get; }
        PlayerStat[] SlowStats { get; }
        PlayerStat[] AverageStats { get; }

        ulong Money { get; }
        
        uint Level { get; }
        uint ExpToNextLevel { get; }
        uint LevelExp { get; }

        void GiveReward(IReward reward);
        void AwardLevelExp(uint exp);
        void AwardMoney(ulong money);
    }
}
