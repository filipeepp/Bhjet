using BHJet_Mobile.Servico.Autenticacao;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.ChamadoAvulso;
using BHJet_Mobile.View.Util;
using BHJet_Mobile.ViewModel.Login;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel(new AutenticacaoServico(), new MotoristaServico(), UsuarioAutenticado.Instance);
            BindingContext = ViewModel;
        }

        /// <summary>
        /// Destrutor
        /// </summary>
        ~LoginPage() { }

        /// <summary>
        /// ViewModel da Pagina de Login
        /// </summary>
        public LoginViewModel ViewModel
        {
            get; set;
        }

        /// <summary>
        /// Abrindo
        /// </summary>
        protected override void OnAppearing()
        {

        }

        /// <summary>
        /// Evento do botão de Logar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnLogar_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Logar();
            }
            catch (Exception error)
            {
                this.TrataExceptionMobile(error);
            }
        }

        private async System.Threading.Tasks.Task Logar()
        {
            ViewModel.Loading = true;

#if DEBUG
            ViewModel.Login = new LoginModel()
            {
                Username = "fulanociclano@teste.com",
                Password = "12345678"
            };
#endif

            // Executa o Login
            await ViewModel.ExecutarLogin();

            // Redirect Seleção de Tipo de veiculo
            if (UsuarioAutenticado.Instance.IDCorridaAtendimento != null)
                App.Current.MainPage = new Detalhe();
            else
                App.Current.MainPage = new TipoVeiculo();
        }
    }
}