using System.ComponentModel.Composition;
using System.Globalization;
using Duplex.MVVM;
using System;

namespace Duplex.Samples.ViewModels
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export]
    public class TestViewModel : ViewModel
    {
        [ObservableProperty]
        public virtual string CurrentDateTime { get; set; }

        [AsyncWorker]
        public virtual void UpdateDateTime()
        {
            CurrentDateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }
    }
}