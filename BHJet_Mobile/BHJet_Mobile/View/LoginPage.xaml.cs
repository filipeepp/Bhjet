using BHJet_Mobile.Servico.Autenticacao;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.ChamadoAvulso;
using BHJet_Mobile.View.Diaria;
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
        /// Evento do botão de Logar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogar_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Executa o Login
                ViewModel.ExecutarLogin();

                //if (ViewModel.Login.Username == "123")
                //{
                //    UsuarioAutenticado.Instance.Contrato = BHJet_Enumeradores.TipoContrato.ContratoLocacao;
                //    UsuarioAutenticado.Instance.Nome = "Filipe ALOCADO teste";
                //    UsuarioAutenticado.Instance.Tipo = BHJet_Enumeradores.TipoProfissional.Motociclista;
                //    UsuarioAutenticado.Instance.StatusAplicatico = false;
                //}
                //else
                //{
                //    UsuarioAutenticado.Instance.Contrato = BHJet_Enumeradores.TipoContrato.ChamadosAvulsos;
                //    UsuarioAutenticado.Instance.Nome = "Leonardo AVULSO teste";
                //    UsuarioAutenticado.Instance.Tipo = BHJet_Enumeradores.TipoProfissional.Motorista;
                //    UsuarioAutenticado.Instance.StatusAplicatico = false;
                //}

                // Troca de página após Login
                if (UsuarioAutenticado.Instance.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                    App.Current.MainPage = new DiariaDeBordo();
                else
                    App.Current.MainPage = new Index();
            }
            catch (Exception error)
            {
                this.TrataExceptionMobile(error);
            }
        }
    }
}