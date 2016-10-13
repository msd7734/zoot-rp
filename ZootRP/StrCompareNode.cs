using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public class StrCompareNode : CompareNode
    {
        public StrCompareNode(string playerValue, string compareValue) : base(playerValue, compareValue, Comparator.EqualTo)
        { }
    }
}
