using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
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
        And,
        Or
    }

    public enum ComparablePlayerVal
    {
        Health,
        Endurance,
        Dexterity,
        Charisma,
        Ingenuity,
        Level,
        Job,
        Species,
        Residence
    }

    public interface IPrerequesite
    {

    }
}
