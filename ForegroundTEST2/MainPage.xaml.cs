#if ANDROID
using Android.Widget;
using ForegroundTEST2.Platforms.Android;
#endif
using ForegroundTEST2.Interface;
using ForegroundTEST2.Models;

namespace ForegroundTEST2
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        IServicesTest services;
        internal static TimerModel TimerModel { get; private set; } = new();
        public MainPage(IServicesTest services)
        {
            InitializeComponent();
            this.services = services;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
#if ANDROID 
            services.Start();
            Toast.MakeText(Platform.CurrentActivity, "Servicio iniciado", ToastLength.Long).Show();
#endif        
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
#if ANDROID
            services.Stop();
            Toast.MakeText(Platform.CurrentActivity, "Servicio detenido", ToastLength.Long).Show();
#endif        
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
#if ANDROID
            Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(ForegroundServiceDemo));
            Android.App.Application.Context.StartForegroundService(intent);
#endif
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            TimerModel.StartNewTimer();
        }
    }

}
