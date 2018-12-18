using BHJet_Mobile.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ViewModel = new LoginViewModel();
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
                await ViewModel.ExecutarLogin();
                // Troca de página após Login
                App.Current.MainPage = new MainPage();
            }
            catch (Exception error)
            {
                //this.TrataExceptionMobile(error);
            }
        }
    }
}