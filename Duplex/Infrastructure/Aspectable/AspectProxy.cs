using System;
using System.Linq;
using Castle.DynamicProxy;

namespace Duplex.Infrastructure.Aspectable
{
    public class AspectProxy
    {

        public static Object Factory(object obj)
        {
            var generator = new ProxyGenerator();
            var attribs = obj.GetType().GetCustomAttributes(typeof(IAspect), true);

            var interceptors = 
                    (from x in attribs select 
                    ((IAspect)x).Invoker)
                    .Cast<IInterceptor>().ToArray();

            var proxy = generator.CreateClassProxy(obj.GetType(), interceptors);

            return proxy;

        }
    }
}
