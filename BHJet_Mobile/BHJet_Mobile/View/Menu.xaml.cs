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
            // Inicializa Componente
            InitializeComponent();

            // Estiliza o menu
            EstiloMenu();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (UsuarioAutenticado.Instance.IDCorridaPesquisada != null || UsuarioAutenticado.Instance.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                return;
            if (UsuarioAutenticado.Instance.IDCorridaAtendimento == null)
            {
                // Envia ordem
                MessagingCenter.Send<string, int>("ObservableChamada", "ObservableChamada", 1);

                // Estiliza o menu
                EstiloMenu();

                // Abre novamente a tela inicial
                App.Current.MainPage = new Index();
            }
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            if (UsuarioAutenticado.Instance.IDCorridaAtendimento != null)
                App.Current.MainPage = new Detalhe();
            else
            {
                if (UsuarioAutenticado.Instance.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                    App.Current.MainPage = new DiariaDeBordo();
                else
                    App.Current.MainPage = new Index();
            }
        }

        private void DadosMotorista_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new DadosBasicos();
        }

        private void EstiloMenu()
        {
            if (UsuarioAutenticado.Instance.IDCorridaAtendimento != null ||
                UsuarioAutenticado.Instance.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                this.btnStatus.BackgroundColor = Color.FromRgb(25, 54, 81);
            else if (!UsuarioAutenticado.Instance.StatusAplicatico && UsuarioAutenticado.Instance.IDCorridaPesquisada == null)
                this.btnStatus.BackgroundColor = Color.Red;
            else
                this.btnStatus.BackgroundColor = Color.FromRgb(25, 54, 81);
        }
    }
}