using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    // maybe allow these to be extended somehow
    public enum ProgressionRate
    {
        Default,
        Slow,
        Average,
        Fast
    }

    public interface IProgression<T>
    {
        ProgressionRate Rate { get; }
        T Min { get; }
        T Max { get; }
        T ValueAt(T x);
    }
}
