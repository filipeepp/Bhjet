
using BHJet_Mobile.Servico;
using BHJet_Mobile.Servico.Autenticacao;
using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View;
using BHJet_Mobile.View.ChamadoAvulso;
using BHJet_Mobile.View.Diaria;
using BHJet_Mobile.View.Util;
using BHJet_Mobile.ViewModel.Login;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Background;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BHJet_Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try
            {
                var VM = new LoginViewModel(new AutenticacaoServico(), new MotoristaServico(), UsuarioAutenticado.Instance);

                System.Threading.Tasks.Task.Run(async () =>
                {
                    var resultado = await VM.BuscaUsuario();
                    if (resultado)
                    {
                        await VM.ExecutarLogin();
                        if (UsuarioAutenticado.Instance.IDCorridaAtendimento != null)
                            App.Current.MainPage = new Detalhe();
                        else if (UsuarioAutenticado.Instance.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                            App.Current.MainPage = new DiariaDeBordo();
                        else
                            MainPage = new TipoVeiculo();
                    }
                    else
                        MainPage = new LoginPage();
                }).Wait();
            }
            catch
            {
                MainPage = new LoginPage();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            if (UsuarioAutenticado.Instance.IDCorridaAtendimento == null &&
                  UsuarioAutenticado.Instance.IDCorridaPesquisada != null &&
                  !UsuarioAutenticado.Instance.StatusAplicatico)
            {
                BackgroundAggregatorService.Add(() => new SomeBackgroundWork());
                BackgroundAggregatorService.StartBackgroundService();
                UsuarioAutenticado.Instance.FinalizaAtendimento();
                try
                {
                    new CorridaServico().LiberarOrdemServico(UsuarioAutenticado.Instance.IDCorridaPesquisada ?? 0);
                }
                finally
                {
                    // finalizada
                    MainPage = new Index();
                }
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
