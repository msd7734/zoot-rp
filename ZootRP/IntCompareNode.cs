using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public class IntCompareNode : CompareNode
    {
        public IntCompareNode(long x, long y, Comparator comparator) : base(x, y, comparator) { }

        public IntCompareNode(ulong x, ulong y, Comparator comparator) : base(x, y, comparator) { }

        public IntCompareNode(int x, int y, Comparator comparator) : base(x, y, comparator) { }

        public IntCompareNode(uint x, uint y, Comparator comparator) : base(x, y, comparator) { }
    }
}
