using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SignalR;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Duplex.Transport
{
    public class SignalRTransport : PersistentConnection//, IDuplexTransport
    {

        public ISubject<string> InStreamAsync { get; set; }
        public ISubject<string> OutStreamAsync { get; set; }
        public ISubject<Exception> ExceptionsAsync { get; set; }
        public int ProcessIntervalMilSec { get; set; }


        public SignalRTransport()
        {
            InStreamAsync = new Subject<string>();
            OutStreamAsync = new Subject<string>();
            ExceptionsAsync = new Subject<Exception>();

            OutStreamAsync.Subscribe(SendAsync);
            ExceptionsAsync.Subscribe(HandleException);

            ProcessIntervalMilSec = 1000;
        }


        public void SendAsync(string data)
        {
            Connection.Send(data);
        }

        public void HandleException(Exception ex)
        {
            throw ex;
        }

        public virtual void ProcessLoop()
        {
            OutStreamAsync.OnNext(DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }


        private static readonly Task _emptyTask = EmptyTask();
        protected override Task OnConnectedAsync(SignalR.Hosting.IRequest request, IEnumerable<string> groups, string connectionId)
        {
            Observable.Interval(TimeSpan.FromMilliseconds(ProcessIntervalMilSec))
                .Subscribe(x => ProcessLoop());
            return _emptyTask;
        }
        protected override Task OnReceivedAsync(string connectionId, string data)
        {
            InStreamAsync.OnNext(data);
            return _emptyTask;
        }
        protected override Task OnErrorAsync(Exception e)
        {
            ExceptionsAsync.OnNext(e);
            return _emptyTask;
        }
        static Task EmptyTask()
        {
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }
    }
}
