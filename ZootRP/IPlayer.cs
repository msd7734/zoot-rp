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
        event PlayerStateChange RewardEvent;

        // Main data objects that 
        IPlayerIdentifier Identifier { get; }
        ICharacter Character { get; }
        IJob Job { get; }
        IResidence Residence { get; }

        ulong Money { get; }
        
        uint Level { get; }

        uint GetHealth();
        uint GetEndurance();
        uint GetDexterity();
        uint GetIngenuity();
        uint GetCharisma();
        uint GetLevelExp();

        uint ExpToNextLevel();

        IProgression<uint> GetHealthProgression();
        IProgression<uint> GetEnduranceProgression();
        IProgression<uint> GetDexterityProgression();
        IProgression<uint> GetIngenuityProgression();
        IProgression<uint> GetCharismaProgression();
        IProgression<uint> GetLevelExpProgression();

        void GiveReward(IReward reward);
        void AwardLevelExp(uint exp);
        void AwardMoney(ulong money);
    }
}
