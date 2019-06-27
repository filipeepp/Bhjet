using BHJet_Mobile.DependencyService;
using BHJet_Mobile.iOS.DependencyService;
using Foundation;
using System;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(IntegracaoWaze))]
namespace BHJet_Mobile.iOS.DependencyService
{
    public class IntegracaoWaze : IIntegracaoWaze
    {
        public void RedirecionaWaze(string latitude, string longitude)
        {
            // Variavel
            latitude = latitude.Replace(",", ".");
            longitude = longitude.Replace(",", ".");

            // URL
            //string url = $"https://waze.com/ul?ll={latitude},{longitude}&navigate=yes";
            string url = $"http://maps.google.com/maps?daddr={latitude},{longitude}";

            // Requisição
            NSUrl request = new NSUrl(url);

            try
            {
                bool isOpened = UIApplication.SharedApplication.OpenUrl(request);
               
                if (isOpened == false)
                    UIApplication.SharedApplication.OpenUrl(new NSUrl("http://itunes.apple.com/us/app/id585027354"));
            }
            catch (Exception ex)
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl("http://itunes.apple.com/us/app/id585027354"));
            }
        }
    }
}