using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core.Prereq
{
    public class PlayerPrerequisite : IPrerequisite
    {
        public string Value { get; private set; }

        public PlayerPrerequisite(string expression)
        {
            Value = expression;
        }

        public bool PlayerMeets(IPlayer player)
        {
            return (new PrereqTree(player, Value)).IsMet();
        }
    }
}
