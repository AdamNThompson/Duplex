using System;

namespace Duplex.Transport
{
    public interface IDuplexTransport
    {
        IObservable<string> AsyncInboundStream();
        IObservable<Exception> AsyncExceptionStream();
        void Send(string data);
    }
}