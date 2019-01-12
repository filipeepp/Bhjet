using BHJet_Mobile.View.ChamadoAvulso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View.Util
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ocorrencia : ContentPage
    {
        public Ocorrencia()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // Troca de página após Login
            await Navigation.PopModalAsync();
        }

        private void Ligar_Clicked(object sender, EventArgs e)
        {
            PlacePhoneCall("031983363474");
        }

        private void PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
    }
}