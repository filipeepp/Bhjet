using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BHJet_Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override  void OnAppearing()
        {
           
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (findIcon.AnimationIsRunning("RotateTo"))
            {
                ViewExtensions.CancelAnimations(findIcon);
            }
            else
            {
                await findIcon.RotateTo(360, 2000);
                findIcon.Rotation = 0;
            }
        }
    }
}
