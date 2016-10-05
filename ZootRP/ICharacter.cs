using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public interface ICharacter
    {
        string Name { get; set; }
        ISpecies Species { get; set; }
    }
}
