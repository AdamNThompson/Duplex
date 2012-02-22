using System;

namespace Duplex
{
    public class ProcessAsyncAttribute : Attribute
    {
        public int IntervalMilSec { get; set; }
    }
}
