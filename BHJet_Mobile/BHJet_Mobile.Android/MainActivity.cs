
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using BHJet_Mobile.Droid.Service;
using BHJet_Mobile.Infra.Service;
using Plugin.Media;
using Plugin.Permissions;
using Xamarin.Forms;

namespace BHJet_Mobile.Droid
{
    [Activity(Label = "BHJet", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Instance = this;

            base.OnCreate(savedInstanceState);

            await CrossMedia.Current.Initialize();
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this.Application);

            //MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", message => {
            //    var intent = new Intent(this, typeof(LongRunningTaskService));
            //    StartService(intent);
            //});

            //MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message => {
            //    var intent = new Intent(this, typeof(LongRunningTaskService));
            //    StopService(intent);
            //});

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public static Activity Instance { get; private set; }
    }
}