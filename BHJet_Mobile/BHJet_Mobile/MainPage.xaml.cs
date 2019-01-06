using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        protected override void OnAppearing()
        {

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (findIcon.AnimationIsRunning("RotateTo"))
                ViewExtensions.CancelAnimations(findIcon);
            else
            {
                await findIcon.RotateTo(360, 2000);
                findIcon.Rotation = 0;
            }

            Thread.Sleep(1500);

            await this.ctnProcurando.TranslateTo(-500, -20, 500);
            this.ctnProcurando.IsVisible = false;

            await this.ctnEncontrado.TranslateTo(-500, -20, 400);
            this.ctnEncontrado.IsVisible = true;
            await this.ctnEncontrado.TranslateTo(0, 0, 400);
        }

        public async void AceitarCorrida(object sender, EventArgs args)
        {
            var X = this.ctnEncontrado.X;
            await this.ctnEncontrado.TranslateTo(-500, -20, 300);

            await this.ctnEncontrado.TranslateTo(0, 0, 300);
        }

        public async void RecusarCorrida(object sender, EventArgs args)
        {
            await this.ctnProcurando.TranslateTo(0, 0, 300);
            this.ctnProcurando.IsVisible = true;

            await this.ctnEncontrado.TranslateTo(-500, -20, 300);
            this.ctnEncontrado.IsVisible = false;
        }
    }
}
