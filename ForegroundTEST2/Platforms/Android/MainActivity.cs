using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace ForegroundTEST2
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if(Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                if (!CheckPermissionGranted(Manifest.Permission.PostNotifications))
                {
                    RequestPostNotificationsPermission();
                }         
            }
        }

        public bool CheckPermissionGranted(string permission)
        {
            if(ActivityCompat.CheckSelfPermission(this, permission) != Permission.Granted)
                return false;
            else
                return true;
        }

        private static readonly int requestPostNotifications = 10023;
        private readonly string[] requestPermission = { "android.permission.POST_NOTIFICATIONS" };

        private void RequestPostNotificationsPermission()
        {
            if(ActivityCompat.ShouldShowRequestPermissionRationale(this, requestPermission.ToString()))
            {
                //Poner una razón para que el usuario de permiso, si es que lo ha denegado antes
                ActivityCompat.RequestPermissions(this, requestPermission, requestPostNotifications);
            }
            else
            {   //Solicitar el permiso directamente
                ActivityCompat.RequestPermissions(this, requestPermission, requestPostNotifications);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (grantResults.Length <= 0)
            {
                // If user interaction was interrupted, the permission request is cancelled and you 
                // receive empty arrays.
                Log.Info("Error", "User interaction was cancelled");
            }
            else if (grantResults[0] == PermissionChecker.PermissionGranted)
            {
                Toast.MakeText(this, "Permission allowed", ToastLength.Long);
            }
            else
            {
                Toast.MakeText(this, "Permission denied", ToastLength.Long);
            }

        }
    }
}
