using BHJet_Mobile.Infra;
using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.ViewModel.Corrida;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View.Util
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ocorrencia : ContentPage
    {
        public Ocorrencia()
        {
            InitializeComponent();
            ViewModel = new OcorrenciaViewModel(new CorridaServico());
            BindingContext = ViewModel;
        }

        /// <summary>
        /// ViewModel da Pagina de Login
        /// </summary>
        public OcorrenciaViewModel ViewModel
        {
            get; set;
        }

        protected async override void OnAppearing()
        {
            try
            {
                await ViewModel.CarregaTela();
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
        }

        private async void SelecionaOcorrencia(object sender, EventArgs e)
        {
            // Busca ID Ocorrencia
            var param = ((Button)sender).CommandParameter;
            var idOcorrencia = (long)param;

            // Seta Ocorrencia
            GlobalVariablesManager.SetApplicationCurrentProperty(GlobalVariablesManager.VariaveisGlobais.OcorrenciaID, idOcorrencia);

            // Troca de página após Login
            await Navigation.PopModalAsync();
        }

        private void Ligar_Clicked(object sender, EventArgs e)
        {
            try
            {
                PhoneDialer.Open("031983363474");
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }

        private void EncerrarOS(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }
    }
}