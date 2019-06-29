using BHJet_Mobile.Infra;
using BHJet_Mobile.Infra.Variaveis;
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
        public Ocorrencia(long idCorrida, long idLog)
        {
            InitializeComponent();
            ViewModel = new OcorrenciaViewModel(new CorridaServico(), idCorrida, idLog, UsuarioAutenticado.Instance);
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
                var idOcorrencia = (long)param;

                // Trata OS
                await ViewModel.OcorrenciaSelecionada(idOcorrencia);

                // Mensagem
                await this.DisplayAlert("Atenção", Mensagem.Sucesso.OcorrenciaOS, "OK");

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
                // Encerra OS
                await ViewModel.EncerrarOrdemServico();

                // Menszagem
                await this.DisplayAlert("Atenção", Mensagem.Sucesso.EncerraOS, "OK");
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

        private async void Ligar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var telefone = await ViewModel.BuscaTelefoneContato();
                PhoneDialer.Open(telefone);
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }

    }
}