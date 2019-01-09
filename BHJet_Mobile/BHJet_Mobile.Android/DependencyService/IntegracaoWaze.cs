﻿using Android.Content;
using BHJet_Mobile.DependencyService;
using BHJet_Mobile.Droid.DependencyService;

[assembly: Xamarin.Forms.Dependency(typeof(IntegracaoWaze))]
namespace BHJet_Mobile.Droid.DependencyService
{
    public class IntegracaoWaze : IIntegracaoWaze
    {
        public void RedirecionaWaze(string latitude, string longitude)
        {
            try
            {
                // Abre Waze
                string url = $"https://waze.com/ul?ll={latitude},{longitude}&navigate=yes";

                Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
                MainActivity.Instance.StartActivity(intent);
            }
            catch (ActivityNotFoundException ex)
            {
                // Redireciona google play
                Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=com.waze"));
                MainActivity.Instance.StartActivity(intent);
            }
        }
    }
}