using BHJet_Mobile.DependencyService;
using BHJet_Mobile.Infra.Permissao;
using BHJet_Mobile.View.Util;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View.ChamadoAvulso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detalhe : ContentPage
    {
        public Detalhe()
        {
            InitializeComponent();
        }

        private void AbrirWaze(object sender, EventArgs e)
        {
            Xamarin.Forms.DependencyService.Get<IIntegracaoWaze>().RedirecionaWaze("-19.879046", "-43.933999");
        }

        private async void RegistrarOcorrencia(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Ocorrencia());
        }

        private async void TiraFoto(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            // Verifica Permissao
            PermissaoBase.VerificaPermissao(Plugin.Permissions.Abstractions.Permission.Camera, PermissaoNegada);


            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Demo"
                });

            if (file == null)
                return;

            var TESTE = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;

            });
        }

        public async void PermissaoNegada()
        {
            if (await this.DisplayAlert("Atenção", "Para registrar uma ocorrência do tipo FOTO, voce deve permitir ao app acesso a mesma. Deseje ativar o recurso ?", "Sim", "Não"))
                PermissaoBase.VerificaPermissao(Plugin.Permissions.Abstractions.Permission.Location, PermissaoNegada);
            else
                App.Current.MainPage = new LoginPage();
        }
    }
}