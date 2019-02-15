
using BHJet_Mobile.Servico.Autenticacao;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View;
using BHJet_Mobile.View.ChamadoAvulso;
using BHJet_Mobile.View.Diaria;
using BHJet_Mobile.View.Util;
using BHJet_Mobile.ViewModel.Login;
using System;
using Xamarin.Forms;
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
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
