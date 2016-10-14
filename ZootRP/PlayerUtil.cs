﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZootRP.Core
{
    public enum PlayerStat { Health, Endurance, Dexterity, Ingenuity, Charisma }

    public enum PlayerIntegerProperty { Level, Health, Endurance, Dexterity, Ingenuity, Charisma }

    public enum PlayerStringProperty { Job, Species, Residence }

    public static class PlayerUtil
    {
        public static readonly List<string> ComparableIntValues =
            Enum.GetNames(typeof(PlayerIntegerProperty)).ToList<string>();

        public static readonly List<string> ComparableStrValues =
            Enum.GetNames(typeof(PlayerStringProperty)).ToList<string>();

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

        public static Dictionary<PlayerStat, IProgression<uint>> GetProgressData(IPlayer player)
        {
            return new Dictionary<PlayerStat, IProgression<uint>>()
            {
                { PlayerStat.Health, player.GetHealthProgression() },
                { PlayerStat.Endurance, player.GetEnduranceProgression() },
                { PlayerStat.Dexterity, player.GetDexterityProgression() },
                { PlayerStat.Ingenuity, player.GetIngenuityProgression() },
                { PlayerStat.Charisma, player.GetCharismaProgression() }
            };
        }

        public static Dictionary<PlayerIntegerProperty, Func<uint>> GetIntegerPropertyFuncs(IPlayer player)
        {
            return new Dictionary<PlayerIntegerProperty, Func<uint>>()
            {
                { PlayerIntegerProperty.Level, () => player.Level },
                { PlayerIntegerProperty.Health, player.GetHealth },
                { PlayerIntegerProperty.Endurance, player.GetEndurance },
                { PlayerIntegerProperty.Dexterity, player.GetDexterity },
                { PlayerIntegerProperty.Ingenuity, player.GetIngenuity },
                { PlayerIntegerProperty.Charisma, player.GetCharisma }
            };
        }

        public static Dictionary<PlayerStringProperty, Func<string>> GetStringPropertyFuncs(IPlayer player)
        {
            return new Dictionary<PlayerStringProperty, Func<string>>()
            {
                { PlayerStringProperty.Job, () => player.Job.Name },
                { PlayerStringProperty.Species, () => player.Character.Species.Name },
                { PlayerStringProperty.Residence, () => player.Residence.Name }
            };
        }

        public static uint GetStat(PlayerStat stat, IPlayer player)
        {
            return GetStatValues(player)[stat];
        }

        public static ProgressionType GetProgressionType(IPlayer player, PlayerStat stat)
        {
            return GetProgressData(player)[stat].Rate;
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
                        kv.Key.ToString(), kv.Value, progData[kv.Key].Rate);
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
