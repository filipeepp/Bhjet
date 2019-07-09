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
            if (UsuarioAutenticado.Instance.StatusAplicatico == BHJet_Enumeradores.StatusAplicativoEnum.Atendimento || UsuarioAutenticado.Instance.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                return;

            // Envia ordem
            if (ViewExtension.VerificaMainPage(typeof(Index)))
                MessagingCenter.Send<string, int>("ObservableChamada", "ObservableChamada", 1);

            // Estiliza o menu
            EstiloMenu();

            // Abre novamente a tela inicial
            App.Current.MainPage = new Index();
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            switch (UsuarioAutenticado.Instance.StatusAplicatico)
            {
                case BHJet_Enumeradores.StatusAplicativoEnum.Atendimento:
                    App.Current.MainPage = new Detalhe();
                    break;

                case BHJet_Enumeradores.StatusAplicativoEnum.ChamadoEncontrado:
                    App.Current.MainPage = new Index();
                    break;

                case BHJet_Enumeradores.StatusAplicativoEnum.Diarista:
                    App.Current.MainPage = new DiariaDeBordo();
                    break;

                default:
                    App.Current.MainPage = new Index();
                    break;
            }
        }

        private void DadosMotorista_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new DadosBasicos();
        }

        private void EstiloMenu()
        {
            switch (UsuarioAutenticado.Instance.StatusAplicatico)
            {
                case BHJet_Enumeradores.StatusAplicativoEnum.Atendimento:
                    this.btnStatus.BackgroundColor = Color.FromRgb(0, 128, 0);
                    break;

                case BHJet_Enumeradores.StatusAplicativoEnum.Pausado:
                    this.btnStatus.BackgroundColor = Color.Red;
                    break;

                default:
                    this.btnStatus.BackgroundColor = Color.FromRgb(25, 54, 81);
                    break;
            }
        }
    }
}