using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public enum PlayerStat { Health, Endurance, Dexterity, Ingenuity, Charisma }

    public static class PlayerUtil
    {
        public static Dictionary<PlayerStat, UInt32> GetStatsDict(IPlayer player)
        {
            return new Dictionary<PlayerStat, UInt32>()
            {
                { PlayerStat.Health, player.GetHealth() },
                { PlayerStat.Endurance, player.GetEndurance() },
                { PlayerStat.Dexterity, player.GetDexterity() },
                { PlayerStat.Ingenuity, player.GetIngenuity() },
                { PlayerStat.Charisma, player.GetCharisma() }
            };

        }

        public static uint GetStat(PlayerStat stat, IPlayer player)
        {
            return GetStatsDict(player)[stat];
        }
    }
}
