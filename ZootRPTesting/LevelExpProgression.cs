using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    public class LevelExpProgression : IProgression<uint>
    {
        public ProgressionRate Rate
        {
            get;
            private set;
        }

        public uint Min
        {
            get;
            private set;
        }

        public uint Max
        {
            get;
            private set;
        }

        public LevelExpProgression()
        {
            Min = 0;
            Max = ValueAt(Player.LEVEL_MAX);

        }

        public uint ValueAt(uint level)
        {
            // x^2 + 25x + 200
            // the minus 1 is to adjust from 226 at level 1 to 225, a purely aesthetic thing :3
            return ((level * level) + (25 * level) + 200) - 1;
        }
    }
}
