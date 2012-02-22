using System;

namespace Duplex.MVVM
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class AsyncWorkerAttribute : Attribute
    {
        public int IntervalMilSec { get; set; }
    }
}
