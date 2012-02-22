using System;
using Castle.DynamicProxy;
using Duplex.Infrastructure.Aspectable;

namespace Duplex.MVVM
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ObservablePropertyAspect : Attribute, IAspect
    {
        #region IAspect
        public IInterceptor Invoker
        {
            get
            {
                return new ObservablePropertyInterceptor();
            }
        }
        #endregion
    }
}
