using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public enum Comparator
    {
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

    public enum PlayerValue
    {
        Health,
        Endurance,
        Dexterity,
        Charisma,
        Ingenuity,
        Level
    }

    public interface IPrerequesite
    {

    }
}
