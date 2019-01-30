using BHJet_Mobile.DependencyService;
using BHJet_Mobile.Infra;
using BHJet_Mobile.Infra.Extensao;
using BHJet_Mobile.Infra.Permissao;
using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.Util;
using BHJet_Mobile.ViewModel.Corrida;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;
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
            ViewModel = new DetalheViewModel(UsuarioAutenticado.Instance, new CorridaServico());
            BindingContext = ViewModel;
        }

        /// <summary>
        /// ViewModel da Pagina de Login
        /// </summary>
        public DetalheViewModel ViewModel
        {
            get; set;
        }

        /// <summary>
        /// Abertura da Tela
        /// </summary>
        protected async override void OnAppearing()
        {
            try
            {
                await ViewModel.Carrega();
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
        }

        private void AbrirWaze(object sender, EventArgs e)
        {
            try
            {
                // Busca ID Corrida
                var idCorridaParam = ((Button)sender).CommandParameter;
                var id = (long)idCorridaParam;

                // Busca Latitude e Longitude
                var local = ViewModel.BuscaLocalizacaoLog(id);

                // Redirect Waze
                Xamarin.Forms.DependencyService.Get<IIntegracaoWaze>().RedirecionaWaze(local.Item1.ToString(), local.Item2.ToString());
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }

        /// <summary>
        /// Registra Ocorrencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RegistrarOcorrencia(object sender, EventArgs e)
        {
            try
            {
                // Busca ID Corrida
                var idEnderecoCorridaParam = ((Button)sender).CommandParameter;
                var id = (long)idEnderecoCorridaParam;

                // Limpa cache
                GlobalVariablesManager.SetApplicationCurrentProperty(GlobalVariablesManager.VariaveisGlobais.OcorrenciaID, null);

                // Registra Ocorrencia
                await Navigation.PushModalAsync(new Ocorrencia());

                //
                var teste = 1;
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }

        /// <summary>
        /// Registra FOTO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TiraFoto(object sender, EventArgs e)
        {
            try
            {
                // Busca ID Corrida
                var idEnderecoCorridaParam = ((Button)sender).CommandParameter;
                var id = (long)idEnderecoCorridaParam;

                // Tira Foto
                var registroFoto = await TiraFotoOcorrencia();

                // Carrega Foto
                await ViewModel.RegistroFotoDocumento(id, registroFoto);
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }

        private async Task<byte[]> TiraFotoOcorrencia()
        {
            // Inicia
            await CrossMedia.Current.Initialize();

            // Verifica Permissao
            await PermissaoBase.VerificaPermissao(Plugin.Permissions.Abstractions.Permission.Camera, PermissaoNegada);

            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
                throw new Exception("Nenhuma câmera detectada.");

            // Tira Foto
            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Demo"
                });

            // Valida arquivo foto
            if (file == null)
                return null;

            // Return arquivo
            return file.GetStream().ReadFully();
        }

        public async void PermissaoNegada()
        {
            if (await this.DisplayAlert("Atenção", "Para registrar uma ocorrência do tipo FOTO, voce deve permitir ao app acesso a mesma. Deseje ativar o recurso ?", "Sim", "Não"))
                await PermissaoBase.VerificaPermissao(Plugin.Permissions.Abstractions.Permission.Location, PermissaoNegada);
            else
                App.Current.MainPage = new LoginPage();
        }

        private async void RegistrarChegada(object sender, EventArgs e)
        {
            try
            {
                // Busca ID Corrida
                var idEnderecoCorridaParam = ((MenuItem)sender).CommandParameter;
                var id = (long)idEnderecoCorridaParam;

                // Registra Chegada
                await ViewModel.RegistrarChegada(id);
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }
    }
}