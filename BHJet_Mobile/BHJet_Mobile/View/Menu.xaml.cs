using BHJet_Mobile.View.ChamadoAvulso;
using BHJet_Mobile.View.Motorista;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Menu : ContentView
	{
		public Menu ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<ContentView, int>(this, "ObservableChamada", 1);
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new Index();
        }

        private void DadosMotorista_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new DadosBasicos();
        }       
    }
}