using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public interface ILocation
    {
        String Name { get; }
        String Description { get; }
        IList<IActivity> CurrentActivities { get; }
    }
}
