using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View.ChamadoAvulso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Index : ContentPage
	{
		public Index ()
		{
			InitializeComponent ();

            MessagingCenter.Subscribe<ContentView, int>(this, "ObservableChamada", async (s, a) =>
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

            });
        }
       

        public void AceitarCorrida(object sender, EventArgs args)
        {
            // Troca de página após Login
            App.Current.MainPage = new Detalhe();
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