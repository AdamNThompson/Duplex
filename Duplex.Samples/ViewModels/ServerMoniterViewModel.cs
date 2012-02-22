using Duplex.MVVM;

namespace Duplex.Samples.ViewModels
{
    public class ServerMoniterViewModel : ViewModel
    {
        [ObservableProperty]
        public int CpuUsage { get; set; }

        [AsyncWorker(IntervalMilSec=1000)]
        public void MoniterCpuUtilization()
        {
            
        }
    }
}