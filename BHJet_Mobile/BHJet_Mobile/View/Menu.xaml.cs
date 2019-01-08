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

    }
}