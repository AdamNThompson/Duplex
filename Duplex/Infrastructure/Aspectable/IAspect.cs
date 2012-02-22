using Castle.DynamicProxy;

namespace Duplex.Infrastructure.Aspectable
{
    interface IAspect
    {
        IInterceptor Invoker { get; }
    }
}
