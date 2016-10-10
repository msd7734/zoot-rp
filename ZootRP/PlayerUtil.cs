using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZootRP.Core
{
    public enum PlayerStat { Health, Endurance, Dexterity, Ingenuity, Charisma }

    public static class PlayerUtil
    {
        public static Dictionary<PlayerStat, UInt32> GetStatValues(IPlayer player)
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

        public static Dictionary<PlayerStat, ProgressiveData<uint>> GetProgressData(IPlayer player)
        {
            return new Dictionary<PlayerStat, ProgressiveData<uint>>()
            {
                { PlayerStat.Health, player.Health },
                { PlayerStat.Endurance, player.Endurance },
                { PlayerStat.Dexterity, player.Dexterity },
                { PlayerStat.Ingenuity, player.Ingenuity },
                { PlayerStat.Charisma, player.Charisma }
            };
        }

        public static uint GetStat(PlayerStat stat, IPlayer player)
        {
            return GetStatValues(player)[stat];
        }

        public static ProgressionType GetProgressionType(IPlayer player, PlayerStat stat)
        {
            return GetProgressData(player)[stat].Progression.Rate;
        }

        public static void PrintPlayerStats(IPlayer player, TextWriter outputWriter, bool printProgressType = false)
        {
            var dict = PlayerUtil.GetStatValues(player);
            if (printProgressType)
            {
                var progData = GetProgressData(player);
                foreach (var kv in dict)
                {
                    outputWriter.WriteLine("{0}: {1} ({2})",
                        kv.Key.ToString(), kv.Value, progData[kv.Key].Progression.Rate);
                }
            }
            else
            {
                foreach (var kv in dict)
                {
                    outputWriter.WriteLine("{0}: {1}", kv.Key.ToString(), kv.Value);
                }
            }
            
        }

        public static void PrintPlayerStats(IPlayer player, bool printProgressType = false)
        {
            PrintPlayerStats(player, Console.Out, printProgressType);
        }
    }
}
