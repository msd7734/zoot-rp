using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    public class Player : IPlayer
    {

        public event PlayerStateChange LevelUpEvent;

        public IPlayerIdentifier Identifier
        {
            get;
            private set;
        }

        public ICharacter Character
        {
            get;
            private set;
        }

        public IJob Job
        {
            get;
            private set;
        }

        public IResidence Residence
        {
            get;
            private set;
        }

        public uint Health
        {
            get;
            private set;
        }

        public uint Endurance
        {
            get;
            private set;
        }

        public uint Dexterity
        {
            get;
            private set;
        }

        public uint Ingenuity
        {
            get;
            private set;
        }

        public uint Charisma
        {
            get;
            private set;
        }

        public ulong Money
        {
            get;
            private set;
        }

        public uint Level
        {
            get;
            private set;
        }

        public uint ExpToNextLevel
        {
            get
            {
                return PlayerStats.ExpToNextLevel(Level);
            }
        }

        public uint LevelExp
        {
            get;
            private set;
        }

        private static readonly int STAT_STARTING_MAX = 20;

        public Player(string name)
        {
            Identifier = new PlayerIdentifier(Guid.NewGuid(), name);
            Level = 1;
            LevelExp = 0;
            Money = 20;

            GenerateStats();
        }

        private void GenerateStats()
        {
            // Get a 16-byte array from guid
            byte[] seed = Identifier.Id.ToByteArray();
            uint health, endurance, ingenuity, dexterity, charisma;

            StatFromSeeds(seed[0], seed[1], out health);
            StatFromSeeds(seed[2], seed[3], out endurance);
            StatFromSeeds(seed[4], seed[5], out ingenuity);
            StatFromSeeds(seed[6], seed[7], out dexterity);
            StatFromSeeds(seed[8], seed[9], out charisma);

            // use remaining bytes to determine something else? 

            Health = health;
            Endurance = endurance;
            Ingenuity = ingenuity;
            Dexterity = dexterity;
            Charisma = charisma;
        }

        private void StatFromSeeds(byte b1, byte b2, out uint stat)
        {
            stat = (uint) ((b1 ^ b2) % STAT_STARTING_MAX) + 1; 
        }

        public void AwardExp(uint exp)
        {
            throw new NotImplementedException();
        }

        public void AwardMoney(ulong money)
        {
            throw new NotImplementedException();
        }
    }
}
