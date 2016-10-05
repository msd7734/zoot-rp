using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public interface IJob
    {
        string Name { get; }
        string Description { get; }

        IReward HighYield { get; }
        IReward LowYield { get; }
    }
}
