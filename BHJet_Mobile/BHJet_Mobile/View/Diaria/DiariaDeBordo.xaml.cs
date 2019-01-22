using BHJet_Mobile.View.ChamadoAvulso;
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
        }

        /// <summary>
        /// ViewModel da Pagina
        /// </summary>
        public DiariaDeBordoViewModel ViewModel
        {
            get; set;
        }

        protected override void OnAppearing()
        {
            try
            {
                ViewModel.Carrega();
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

                // Navega
                App.Current.MainPage = new Index();
            }
            catch (Exception ex)
            {
                this.TrataExceptionMobile(ex);
            }
        }
    }
}