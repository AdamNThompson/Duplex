using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Duplex.Infrastructure;

namespace Duplex.MVVM
{
    public class ObservablePropertyInterceptor : IInterceptor
    {
        #region IInterceptor members
        public void Intercept(IInvocation invocation)
        {
            // let the original call go 1st
            invocation.Proceed();

            // make sure target is setting a property
            if (!invocation.Method.Name.StartsWith("set_")) return;

            var propertyName = invocation.Method.Name.Substring(4);
            var pi = invocation.TargetType.GetProperty(propertyName);

            // check for the [ObservableProperty] attribute
            if (!pi.HasAttribute<ObservablePropertyAttribute>()) return;

            // get reflected info of interception target
            var info = invocation.TargetType.GetFields(
                BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(f => f.FieldType == typeof(PropertyChangedEventHandler));

            if (info != null)
            {
                //get the INPC field, and invoke it if we managed to get it ok
                var evHandler =
                    info.GetValue(invocation.InvocationTarget) as PropertyChangedEventHandler;
                if (evHandler != null)
                    evHandler.Invoke(invocation.InvocationTarget,
                                     new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
