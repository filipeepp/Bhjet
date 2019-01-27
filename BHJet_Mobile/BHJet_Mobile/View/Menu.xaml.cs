using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.ChamadoAvulso;
using BHJet_Mobile.View.Diaria;
using BHJet_Mobile.View.Motorista;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentView
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<string, int>("ObservableChamada", "ObservableChamada", 1);

            if (UsuarioAutenticado.Instance.StatusAplicatico)
                this.btnStatus.BackgroundColor = Color.Red;
            else
                this.btnStatus.BackgroundColor = Color.FromRgb(25, 54, 81);
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            if (UsuarioAutenticado.Instance.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                App.Current.MainPage = new DiariaDeBordo();
            else
                App.Current.MainPage = new Index();
        }

        private void DadosMotorista_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new DadosBasicos();
        }
    }
}