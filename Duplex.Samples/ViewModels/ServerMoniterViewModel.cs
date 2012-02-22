namespace Duplex.Samples.ViewModels
{
    public class ServerMoniterViewModel : ViewModel
    {
        [NotifyAsync]
        public int CpuUsage { get; set; }

        [ProcessAsync(IntervalMilSec=1000)]
        public void MoniterCpuUtilization()
        {
            
        }
    }
}