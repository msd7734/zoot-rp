using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public class ProgressiveData<T>
    {
        public IProgression Progression { get; private set; }
        public T Value { get; private set; }

        public ProgressiveData(T value, IProgression progression)
        {
            Value = value;
            Progression = progression;
        }
    }
}
