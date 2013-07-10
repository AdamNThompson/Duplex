using Duplex.MVVM;
using System;
using System.Globalization;

namespace Duplex.Samples.ViewModels
{
    public class TestViewModel : ViewModel
    {
        [ObservableProperty]
        public virtual string CurrentDateTime { get; set; }

        [AsyncWorker]
        public virtual void UpdateDateTime()
        {
            CurrentDateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            OutStreamAsync.OnNext(CurrentDateTime);
        }
    }
}