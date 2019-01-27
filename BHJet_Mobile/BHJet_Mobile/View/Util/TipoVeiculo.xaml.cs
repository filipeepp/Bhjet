using BHJet_Mobile.Infra.Permissao;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.ChamadoAvulso;
using BHJet_Mobile.View.Diaria;
using BHJet_Mobile.ViewModel.Util;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View.Util
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TipoVeiculo : ContentPage
    {
        public TipoVeiculo()
        {
            InitializeComponent();
            ViewModel = new TipoVeiculoViewModel(UsuarioAutenticado.Instance, new MotoristaServico());
            BindingContext = ViewModel;
        }

        /// <summary>
        /// ViewModel da Pagina
        /// </summary>
        public TipoVeiculoViewModel ViewModel
        {
            get; set;
        }

        private async void MotoSelecionada_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica Permissao
                PermissaoBase.VerificaPermissao(Plugin.Permissions.Abstractions.Permission.Location, PermissaoNegada);

                // Grava registro profissional ativo
                await ViewModel.SelecionaVeiculo(BHJet_Enumeradores.TipoProfissional.Motociclista);

                // Troca de página após seleção
                RedirecionaProfissional();
            }
            catch (Exception error)
            {
                this.TrataExceptionMobile(error);
            }
        }

        private async void CarroSelecionado_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica Permissao
                PermissaoBase.VerificaPermissao(Plugin.Permissions.Abstractions.Permission.Location, PermissaoNegada);

                // Grava registro profissional ativo
                await ViewModel.SelecionaVeiculo(BHJet_Enumeradores.TipoProfissional.Motociclista);

                // Troca de página após seleção
                RedirecionaProfissional();
            }
            catch (Exception error)
            {
                this.TrataExceptionMobile(error);
            }
        }

        public async void PermissaoNegada()
        {
            if (await this.DisplayAlert("Atenção", "Para o funcionamento do app, você deve permitir acesso a sua localização. Deseje ativar o recurso ?", "Sim", "Não"))
                PermissaoBase.VerificaPermissao(Plugin.Permissions.Abstractions.Permission.Location, PermissaoNegada);
            else
                App.Current.MainPage = new LoginPage();
        }

        private void RedirecionaProfissional()
        {
            // Troca de página após Login
            if (UsuarioAutenticado.Instance.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                App.Current.MainPage = new DiariaDeBordo();
            else
                App.Current.MainPage = new Index();
        }
    }
}