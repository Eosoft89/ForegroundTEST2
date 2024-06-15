using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForegroundTEST2.Interface;
#if ANDROID
using Android.Widget;
#endif
namespace ForegroundTEST2.Models
{
    internal class TimerModel : ISubject
    {
        private readonly IDispatcherTimer timer;
        private readonly Stopwatch stopwatch;
        private List<IObserver> observers = new();

        private int remainingPercentage;
        private double maxTime = 100;
        private double remainingTime;
        private int remainingIntPerc;

        public int RemainingPercentage
        {
            get { return remainingPercentage; }
            set { remainingPercentage = value; Notify(); }
        }
        public string ContentText { get; set; }

        public TimerModel()
        {
            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            stopwatch = new();
        }

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Notify()
        {
            foreach (IObserver o in observers)
            {
                o.Update(this);
            }
        }

        public void StartNewTimer()
        {
            timer.Tick += TimerTick;
            remainingTime = maxTime;
            RemainingPercentage = 100;
            timer.Start();
            stopwatch.Start();
#if ANDROID
            Toast.MakeText(Platform.CurrentActivity, "Timer iniciado", ToastLength.Long).Show();
#endif      
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if(stopwatch.Elapsed <= TimeSpan.FromSeconds(100))
            {
                remainingTime = maxTime - stopwatch.Elapsed.TotalSeconds;
                remainingIntPerc = Convert.ToInt32((remainingTime * 100) / maxTime);

                if (remainingIntPerc < RemainingPercentage)
                {
                    ContentText = TimeSpan.FromSeconds(remainingTime).ToString("hh' : 'mm' : 'ss");
                    RemainingPercentage = remainingIntPerc;
                }
            }
            else
            {
                stopwatch.Stop();
                timer.Stop();
                stopwatch.Reset();
            }
        }
    }
}
