using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public static class PlayerStats
    {
        public enum Stat { Health, Endurance, Dexterity, Ingenuity, Charisma }

        public static readonly uint LEVEL_MIN = 1;
        public static readonly uint LEVEL_MAX = 99;

        public static readonly uint STAT_MIN = 1;
        public static readonly uint STAT_MAX = 99;

        private static Dictionary<IPlayer, Dictionary<Stat, UInt32>> GeneratedStats =
            new Dictionary<IPlayer, Dictionary<Stat, UInt32>>();

        public static Dictionary<Stat,UInt32> GetStatsDict(IPlayer player)
        {
            try
            {
                return GeneratedStats[player];
            }
            catch(KeyNotFoundException knfe)
            {
                var newStatsDict = new Dictionary<Stat, UInt32>()
                {
                    { Stat.Health, player.Health },
                    { Stat.Endurance, player.Endurance },
                    { Stat.Dexterity, player.Dexterity },
                    { Stat.Ingenuity, player.Ingenuity },
                    { Stat.Charisma, player.Charisma }
                };

                GeneratedStats.Add(player, newStatsDict);

                return newStatsDict;
            }
            
        }

        public static uint GetStat(Stat stat, IPlayer player)
        {
            Dictionary<Stat,UInt32> statsDict;

            try
            {
                statsDict = GeneratedStats[player];
            }
            catch (KeyNotFoundException knfe)
            {
                statsDict = GetStatsDict(player);
            }

            return statsDict[stat];
        }

        public static uint ExpToNextLevel(uint currentLevel)
        {
            return (currentLevel >= LEVEL_MAX) ? 0 : ((currentLevel * currentLevel) + (25 * currentLevel) + 200) - 1;
        }
    }
}
