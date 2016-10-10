using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public class ProgressiveData<T>
    {
        public IProgression<T> Progression { get; private set; }
        public T Value { get; set; }

        public ProgressiveData(T value, IProgression<T> progression)
        {
            Value = value;
            Progression = progression;
        }

        public void ProgressToValue(T value)
        {
            Value = Progression.ValueAt(value);
        }
    }
}
