using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core.Prereq
{
    public enum Comparator
    {
        Unknown,
        EqualTo,
        GreaterThan,
        LessThan,
        GreaterThanEqualTo,
        LessThanEqualTo
    }

    public enum LogicalOperator
    {
        Unknown,
        And,
        Or
    }

    public interface IPrerequisite
    {
        string Value { get; }

        bool PlayerMeets(IPlayer player);
        // what should be responsible for checking whether a given player meets the prereq?
    }
}
