using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    public class Character : ICharacter
    {
        public string Name
        {
            get;
            private set;
        }

        public ISpecies Species
        {
            get;
            private set;
        }

        public Character(string name, ISpecies species)
        {
            Name = name;
            Species = species;
        }
    }
}
