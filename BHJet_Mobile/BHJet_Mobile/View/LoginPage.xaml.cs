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
            //ViewModel = new LoginViewModel(new AutorizacaoServico());
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
        private async void btnLogar_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Executa o Login
                var tsrue = await ViewModel.ExecutarLogin();

                // Troca de página após Login
                if (tsrue)
                    App.Current.MainPage = new DiariaDeBordo();
                else
                    App.Current.MainPage = new Index();
            }
            catch (Exception error)
            {
                //this.TrataExceptionMobile(error);
            }
        }
    }
}