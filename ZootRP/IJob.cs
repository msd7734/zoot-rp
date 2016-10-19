using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core.Prereq;

namespace ZootRP.Core
{
    public interface IJob
    {
        string Name { get; }
        string Description { get; }
        IPrerequisite Prerequesite { get; }

        IReward Work(IPlayer player);
    }
}
