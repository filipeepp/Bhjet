using BHJet_Mobile.View.ChamadoAvulso;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View.Diaria
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DiariaDeBordo : ContentPage
	{
		public DiariaDeBordo ()
		{
			InitializeComponent ();
		}

        private void IniciarDiaria_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new Index();
        }
    }
}