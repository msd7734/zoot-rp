using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScheduleWidget.ScheduledEvents;

namespace ZootRP.Core
{
    public interface IActivity
    {
        Schedule Schedule { get; }
    }
}
