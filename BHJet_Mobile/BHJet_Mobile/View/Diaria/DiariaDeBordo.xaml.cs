using BHJet_Mobile.Servico.Diaria;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.ViewModel.DiariaDeBordo;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View.Diaria
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiariaDeBordo : ContentPage
    {
        public DiariaDeBordo()
        {
            InitializeComponent();
            ViewModel = new DiariaDeBordoViewModel(UsuarioAutenticado.Instance, new DiariaServico());
            BindingContext = ViewModel;
        }

        /// <summary>
        /// ViewModel da Pagina
        /// </summary>
        public DiariaDeBordoViewModel ViewModel
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

        private async void IniciarDiaria_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Atualiza Turno
                await ViewModel.AtualizaTurno();

                // Mensagem
                await this.DisplayAlert("Atenção", "Dados do turno atualizados com sucesso.", "OK");
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }
    }
}