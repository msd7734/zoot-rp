using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public class CompareNode : IPrereqNode
    {

        private IComparable left, right;
        private Comparator comparator;

        public CompareNode(IComparable x, IComparable y, Comparator c) : base()
        {
            left = x;
            right = y;
            comparator = c;
        }

        public bool Compare()
        {
            // less than zero: left precedes right
            // equal to zero: left equals right
            // greater than zero: left comes after right
            int res = left.CompareTo(right);
            switch (comparator)
            {
                case Comparator.EqualTo:
                    return (res == 0);
                case Comparator.GreaterThan:
                    return (res > 0);
                case Comparator.GreaterThanEqualTo:
                    return (res >= 0);
                case Comparator.LessThan:
                    return (res < 0);
                case Comparator.LessThanEqualTo:
                    return (res <= 0);
                default:
                    return false;
            }
        }
    }
}
