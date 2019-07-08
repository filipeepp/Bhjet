using BHJet_Mobile.Infra.Variaveis;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.Util;
using BHJet_Mobile.ViewModel.Motorista;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View.Motorista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DadosBasicos : ContentPage
    {
        public DadosBasicos()
        {
            InitializeComponent();
            ViewModel = new DadosBasicosViewModel(UsuarioAutenticado.Instance, new MotoristaServico());
            BindingContext = ViewModel;
        }

        /// <summary>
        /// ViewModel da Pagina
        /// </summary>
        public DadosBasicosViewModel ViewModel
        {
            get; set;
        }

        protected async override void OnAppearing()
        {
            try
            {
                await ViewModel.Carrega();
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
        } 

        private async void AtualizaDados_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Atualizado dados
                await  ViewModel.AtualizarDados();

                // Mensagem
                await this.DisplayAlert("Atenção", Mensagem.Sucesso.OK, "OK");
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }

        private void TrocarVeiculo_Clicked(object sender, EventArgs e)
        {
            try
            {
                UsuarioAutenticado.Instance.CancelaPesquisaChamado();
                App.Current.MainPage = new TipoVeiculo();
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }

        private async void Sair_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Atualizado dados
                await UsuarioAutenticado.Instance.Sair();
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
            finally
            {
                App.Current.MainPage = new LoginPage();
            }
        }
    }
}