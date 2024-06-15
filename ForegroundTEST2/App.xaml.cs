using ForegroundTEST2.Interface;

namespace ForegroundTEST2
{
    public partial class App : Application
    {
        private readonly IServicesTest services;
        public App(IServicesTest services)
        {
            InitializeComponent();
            MainPage = new AppShell();
            this.services = services;   
        }

        protected override Window CreateWindow(IActivationState activationState)
        {

            Window window = base.CreateWindow(activationState);

            window.Deactivated += Window_Deactivated;
            window.Resumed += Window_Resumed;

            return window;
        }

        private void Window_Resumed(object sender, EventArgs e)
        {
            services.Stop();     
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            services.Start();
        }
    }
}
