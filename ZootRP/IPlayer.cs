using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public delegate void PlayerStateChange(IPlayer sender, PlayerUpdateEventArgs e);

    public interface IPlayer
    {
        event PlayerStateChange LevelUpEvent;

        // Main data objects that 
        IPlayerIdentifier Identifier { get; }
        ICharacter Character { get; }
        IJob Job { get; }
        IResidence Residence { get; }
        
        // Player stats wrapped in their advancement functions
        ProgressiveData<uint> Health { get; }
        ProgressiveData<uint> Endurance { get; }
        ProgressiveData<uint> Dexterity { get; }
        ProgressiveData<uint> Ingenuity { get; }
        ProgressiveData<uint> Charisma { get; }

        // I think the interface doesn't need to guarantee the use of an IProgression?
        // If that's the case, why have it in Core, though...

        // IGrowable<T> interface that gives IProgression GetProgression()
        // This can then be applied to a wrapper class of a stat

        ulong Money { get; }
        
        uint Level { get; }
        ProgressiveData<uint> LevelExp { get; }

        uint GetHealth();
        uint GetEndurance();
        uint GetDexterity();
        uint GetIngenuity();
        uint GetCharisma();
        uint GetLevelExp();

        uint ExpToNextLevel();

        void GiveReward(IReward reward);
        void AwardLevelExp(uint exp);
        void AwardMoney(ulong money);
    }
}
