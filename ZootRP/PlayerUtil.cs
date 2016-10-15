using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZootRP.Core
{
    /// <summary>
    /// The values for player properties classified as "stats": physical/mental skill levels.
    /// </summary>
    public enum PlayerStat { Health, Endurance, Dexterity, Ingenuity, Charisma }

    /// <summary>
    /// Player properties that can be compared (with other players or for prerequesites) and are stored as integers.
    /// </summary>
    public enum PlayerIntegerProperty { Level, Health, Endurance, Dexterity, Ingenuity, Charisma }

    /// <summary>
    /// Player properties that can be compared (with other players or for prerequesites) and are stored as strings.
    /// </summary>
    public enum PlayerStringProperty { Job, Species, Residence }

    /// <summary>
    /// Provide utility data and methods that an IPlayer implementation should have, but not have to implement.
    /// </summary>
    public static class PlayerUtil
    {
        /// <summary>
        /// List of comparable player integer property names as strings.
        /// <see cref="PlayerIntegerProperty"/>
        /// </summary>
        public static readonly List<string> ComparableIntValues =
            Enum.GetNames(typeof(PlayerIntegerProperty)).ToList<string>();

        /// <summary>
        /// List of comparable player string property names as strings.
        /// <see cref="PlayerStringProperty"/>
        /// </summary>
        public static readonly List<string> ComparableStrValues =
            Enum.GetNames(typeof(PlayerStringProperty)).ToList<string>();

        /// <summary>
        /// Get all the stats of a player in convenient dictionary form.
        /// <see cref="PlayerStat"/>
        /// </summary>
        /// <param name="player">The player whose stats to return.</param>
        /// <returns>A dictionary to access player stats via PlayerStat enum keys.</returns>
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

        /// <summary>
        /// Get the progression data for all of a player's stats in convenient dictionary form.
        /// <see cref="IProgression"/>
        /// </summary>
        /// <param name="player">The player whose progression data to return.</param>
        /// <returns>A dictionary to access player progression data via PlayerStat enum keys.</returns>
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

        /// <summary>
        /// Get all Func references to all of a player's integer comparable properties methods.
        /// Used primarily for evaluating prerequesites.
        /// <see cref=" PlayerIntegerProperty"/>
        /// </summary>
        /// <param name="player">The player whose stat Funcs to get.</param>
        /// <returns>A dictionary to access player stat functions via PlayerIntegerProperty enum keys.</returns>
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

        /// <summary>
        /// Get all Func references to all of a player's string comparable properties methods.
        /// Used primarily for evaluating prerequesites.
        /// <see cref=" PlayerStringProperty"/>
        /// </summary>
        /// <param name="player">The player whose stat Funcs to get.</param>
        /// <returns>A dictionary to access player stat functions via PlayerStringProperty enum keys.</returns>
        public static Dictionary<PlayerStringProperty, Func<string>> GetStringPropertyFuncs(IPlayer player)
        {
            return new Dictionary<PlayerStringProperty, Func<string>>()
            {
                { PlayerStringProperty.Job, () => player.Job.Name },
                { PlayerStringProperty.Species, () => player.Character.Species.Name },
                { PlayerStringProperty.Residence, () => player.Residence.Name }
            };
        }

        /// <summary>
        /// Get a stat value from a player given a PlayerStat enum value.
        /// </summary>
        /// <param name="stat">The type of stat to get.</param>
        /// <param name="player">The player to get a stat from.</param>
        /// <returns>The integer value of a player stat.</returns>
        public static uint GetStat(PlayerStat stat, IPlayer player)
        {
            return GetStatValues(player)[stat];
        }

        /// <summary>
        /// Get progression data from a player given a ProgressionType enum value.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="stat"></param>
        /// <returns></returns>
        public static ProgressionType GetProgressionType(IPlayer player, PlayerStat stat)
        {
            return GetProgressData(player)[stat].Rate;
        }

        /// <summary>
        /// Print the stats of a player to a given stream.
        /// </summary>
        /// <param name="player">The player whose stats to print.</param>
        /// <param name="outputWriter">The writer for the stream to print to.</param>
        /// <param name="printProgressType">Whether to also print what type of progression rate each stat has.</param>
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

        /// <summary>
        /// Print the stats of a player to standard out.
        /// </summary>
        /// <param name="player">The player whose stats to print.</param>
        /// <param name="printProgressType">Whether to also print what type of progression rate each stat has.</param>
        public static void PrintPlayerStats(IPlayer player, bool printProgressType = false)
        {
            PrintPlayerStats(player, Console.Out, printProgressType);
        }
    }
}
