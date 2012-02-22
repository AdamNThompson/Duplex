using System.Globalization;
using Duplex.Infrastructure;
using Duplex.Infrastructure.Aspectable;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.Reactive.Linq;
using SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System;
using System.Reflection;
using System.Linq;

namespace Duplex.MVVM
{
    [Aspect(typeof(ViewModel))]
    public abstract class ViewModel : PersistentConnection, INotifyPropertyChanged
    {
        public ISubject<string> InStreamAsync { get; set; }
        public ISubject<string> OutStreamAsync { get; set; }
        public ISubject<Exception> ExceptionsAsync { get; set; }
        public int ProcessIntervalMilSec { get; set; }
        private readonly IObservable<MethodInfo> _workers;

        protected ViewModel()
        {
            InStreamAsync = new Subject<string>();
            OutStreamAsync = new Subject<string>();
            ExceptionsAsync = new Subject<Exception>();

            OutStreamAsync.Subscribe(SendAsync);
            ExceptionsAsync.Subscribe(HandleException);

            ProcessIntervalMilSec = 1000;

            _workers = this.GetType()
                           .GetMethodsWithAttribute<AsyncWorkerAttribute>()
                           .ToObservable();

            Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                .Subscribe(x =>
                {
                    string name = x.EventArgs.PropertyName;
                    string value = this.GetType().GetProperty(name).GetValue(this, null).ToString();
                    OutStreamAsync.OnNext(value);
                });
        }

        

        public void SendAsync(string data)
        {
            Connection.Send(data);
        }

        public void HandleException(Exception ex)
        {
            throw ex;
        }

        private void RunAsyncWorkers()
        {
            Observable.Interval(TimeSpan.FromMilliseconds(ProcessIntervalMilSec))
                .Subscribe(x => _workers.ForEach(m => m.Invoke(this, null)));

            //OutStreamAsync.OnNext(DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }





        private static readonly Task _emptyTask = EmptyTask();
        protected override Task OnConnectedAsync(SignalR.Hosting.IRequest request, IEnumerable<string> groups, string connectionId)
        {
            RunAsyncWorkers();
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

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
