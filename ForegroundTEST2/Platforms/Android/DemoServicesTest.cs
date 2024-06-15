using Android.App;
using Android.Content;
using Android.OS;
using ForegroundTEST2.Interface;
using Android.Content.PM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Runtime;
using AndroidX.Core.App;
using ForegroundTEST2.Models;
using Java.Lang;

namespace ForegroundTEST2.Platforms.Android
{
    [Service(ForegroundServiceType = ForegroundService.TypeMediaPlayback)]
    internal class DemoServicesTest : Service, IServicesTest, IObserver
    {
        bool isStarted = false;
        private const int notificationId = 890501;

        public DemoServicesTest()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                CreateNotificationChannel();
            }
        }
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        private void CreateNotificationChannel()
        {
            NotificationChannel channel = new("ServiceChannel", "Demo de servicio", NotificationImportance.Max);
            NotificationManager manager = Platform.AppContext.GetSystemService(NotificationService) as NotificationManager;
            manager.CreateNotificationChannel(channel);
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (intent.Action == "START_SERVICE")
            {
                System.Diagnostics.Debug.WriteLine("Se ha iniciado el servicio");
                StartMyForeground();
            }
            else if (intent.Action == "STOP_SERVICE")
            {
                System.Diagnostics.Debug.WriteLine("Se ha detenido el servicio");
                StopSelfResult(startId);
            }
            return StartCommandResult.Sticky;
        }
        public void Start()
        {
            Intent startIntent = new(Platform.AppContext, typeof(DemoServicesTest));
            startIntent.SetAction("START_SERVICE");
            Platform.AppContext.StartService(startIntent);
            MainPage.TimerModel.Attach(this);
            isStarted = true;
        }

        public void Stop()
        {
            Intent stopIntent = new(Platform.AppContext, this.Class);
            stopIntent.SetAction("STOP_SERVICE");
            Platform.AppContext.StopService(stopIntent);
            isStarted = false;
            //Disatach acáaaasdasdasd (disatach del IObserver)

        }

        private void StartMyForeground()
        {
            StartForeground(notificationId, GetMyNotification("", 100));
        }

        private Notification GetMyNotification(string contentText, int progressValue)
        {
           // PendingIntent contentIntent = PendingIntent.GetActivity(this, 0, new Intent(Platform.AppContext, this.Class), 0);
            return new NotificationCompat.Builder(Platform.AppContext, "ServiceChannel")
                .SetContentTitle("Servicio trabajando")
                .SetContentText(contentText)
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetAutoCancel(false)
                .SetProgress(100, progressValue, false)
                .SetOngoing(true)
                .SetOnlyAlertOnce(true)
                .Build();
        }

        private void UpdateMyNotification(string contentText, int progressValue)
        {
            Notification notification = GetMyNotification(contentText, progressValue);
            NotificationManager notificationManager = Platform.AppContext.GetSystemService(NotificationService) as NotificationManager;
            notificationManager.Notify(notificationId, notification);

        }
        public void Update(ISubject subject)
        { 
            if (isStarted)
            {
                if (subject is TimerModel timer)
                {
                    Console.WriteLine("------ Entró a update con isStarted = " + isStarted + " y Remaining = " + timer.RemainingPercentage + "--------");
                    UpdateMyNotification(timer.ContentText, timer.RemainingPercentage);
                }
            }
        }
    }
}
