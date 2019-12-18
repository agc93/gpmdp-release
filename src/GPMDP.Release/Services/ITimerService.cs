using System;
using System.Timers;
using Microsoft.Extensions.Configuration;

namespace GPMDP.Release.Services
{
    public interface ITimerService
    {
        void Reset();
        event EventHandler TimerFinished;
        Action OnTimerFinished { set; }
    }
    public class TimerService : ITimerService
    {
        private Timer _timer;

//        public TimerService() : this(1000)
//        {
//            
//        }

        public TimerService(IConfiguration config)
        {
            var interval = config.GetValue<int>("DebounceInterval", 0);
            Initialize((interval == 0) ? 1000 : interval);
        }

        public TimerService(int interval)
        {
            Initialize(interval);
        }

        private void Initialize(int interval)
        {
            _timer = new Timer(interval);
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = false;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimerFinished?.Invoke(this, EventArgs.Empty);
            OnTimerFinished?.Invoke();
        }

        public void Reset()
        {
            _timer.Stop();
            _timer.Start();
        }

        public event EventHandler TimerFinished;
        public Action OnTimerFinished { get; set; }
    }
}