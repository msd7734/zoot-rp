using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public interface IJob
    {
        string Name { get; set; }
        string Description { get; set; }

        IReward HighYield { get; set; }
        IReward LowYield { get; set; }
    }
}
