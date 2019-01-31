using BHJet_Mobile.Infra;
using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.ChamadoAvulso;
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
        public Ocorrencia(long idCorrida)
        {
            InitializeComponent();
            ViewModel = new OcorrenciaViewModel(new CorridaServico(), idCorrida);
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
            try
            {
                // Busca ID Ocorrencia
                var param = ((Button)sender).CommandParameter;
                var idOcorrencia = (int)param;

                // Trata OS
                ViewModel.OcorrenciaSelecionada(idOcorrencia);

                // Troca de página após Login
                await Navigation.PopModalAsync();
            }
            catch (CorridaException)
            {
                // Finaliza Atendimento
                UsuarioAutenticado.Instance.FinalizaAtendimento();
                // Navega
                App.Current.MainPage = new Index();
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }


        private async void EncerrarOS(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.EncerrarOrdemServico();
            }
            catch (CorridaException)
            {
                // Finaliza Atendimento
                UsuarioAutenticado.Instance.FinalizaAtendimento();
                // Navega
                App.Current.MainPage = new Index();
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
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

    }
}