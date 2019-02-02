using BHJet_CoreGlobal;
using BHJet_Mobile.DependencyService;
using BHJet_Mobile.Infra;
using BHJet_Mobile.Infra.Database;
using BHJet_Mobile.Infra.Database.Tabelas;
using BHJet_Mobile.Infra.Extensao;
using BHJet_Mobile.Infra.Permissao;
using BHJet_Mobile.Infra.Variaveis;
using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.Util;
using BHJet_Mobile.ViewModel.Corrida;
using Plugin.Geolocator;
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
                // Carrega dados da corrida
                await ViewModel.Carrega();

                // Inicia compartilhamento de localizacao
                if (!CrossGeolocator.Current.IsListening)
                    StartListening();
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
        }

        async Task StartListening()
        {
            // Verifica listening
            if (CrossGeolocator.Current.IsListening)
                return;

            // Database
            using (var db = new Database())
            {
                if (await db.ExisteTabela("BHJetLocalizacaoCorrida") == false)
                    await db.CriaTabela<LocalizacaoCorrida>();
                else
                    await db.LimpaTabela("BHJetLocalizacaoCorrida");
            }

            ///This logic will run on the background automatically on iOS, however for Android and UWP you must put logic in background services. Else if your app is killed the location updates will be killed.
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true, new Plugin.Geolocator.Abstractions.ListenerSettings
            {
                ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = true,
                DeferralDistanceMeters = 1,
                DeferralTime = TimeSpan.FromSeconds(1),
                ListenForSignificantChanges = true,
                PauseLocationUpdatesAutomatically = false
            });

            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                // Database
                using (var db = new Database())
                {
                    // Insere localizacao
                    await db.InsereItem<LocalizacaoCorrida>(new LocalizacaoCorrida()
                    {
                        IDCorrida = ViewModel.idCorrida,
                        Latitude = e.Position.Latitude,
                        Longitude = e.Position.Longitude
                    });
                }
            });
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

                // Registra Ocorrencia
                await Navigation.PushModalAsync(new Ocorrencia(ViewModel.idCorrida));
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

                // Ocorrencia
                await this.DisplayAlert("Atenção", Mensagem.Sucesso.ProtocoloCadastrado, "OK");
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
                var idEnderecoCorridaParam = ((Button)sender).CommandParameter;
                var id = (long)idEnderecoCorridaParam;

                // Registra Chegada
                await ViewModel.RegistrarChegada(id);

                // Mensagem
                await this.DisplayAlert("Atenção", Mensagem.Sucesso.RegistrarChegada, "OK");
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }
    }
}