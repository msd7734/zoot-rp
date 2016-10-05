using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public static class PlayerStats
    {
        public static enum Stat { Health, Endurance, Dexterity, Ingenuity, Charisma }

        public static const uint LEVEL_MIN = 1;
        public static const uint LEVEL_MAX = 99;

        public static Dictionary<Stat,UInt32> GetStatsDict(IPlayer player)
        {
            return new Dictionary<Stat, UInt32>()
            {
                { Stat.Health, player.Health },
                { Stat.Endurance, player.Endurance },
                { Stat.Dexterity, player.Dexterity },
                { Stat.Ingenuity, player.Ingenuity },
                { Stat.Charisma, player.Charisma }
            };
        }

        public ulong ExpToNextLevel(uint level)
        {
            return (level >= LEVEL_MAX) ? 0 : Convert.ToUInt64(((level * level) + (25 * level) + 200) - 1);
        }
    }
}
