using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    public class Species : ISpecies
    {
        public string Name
        {
            get;
            private set;
        }

        private static readonly string[] SPECIES_TYPES = { "Fox", "Bunny" };

        public Species(byte seed)
        {
            Name = SPECIES_TYPES[seed % SPECIES_TYPES.Length];
        }
    }
}
