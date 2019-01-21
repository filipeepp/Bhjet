﻿using BHJet_Mobile.Infra;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.Diaria;
using BHJet_Mobile.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View.ChamadoAvulso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Index : ContentPage
    {
        public Index()
        {
            InitializeComponent();
            ViewModel = new IndexViewModel(UsuarioAutenticado.Instance);

            MessagingCenter.Subscribe<string, int>("ObservableChamada", "ObservableChamada", async (s, a) =>
            {
                // If Status Motoristas
                if (UsuarioAutenticado.Instance.StatusAplicatico)
                {
                    // Desativa Efeito
                    EfeitoPesquisaDesativada();
                }
                else
                {
                    // Ativa Efeito Pesquisa
                    EfeitoPesquisaAtivada();

                    // Ativa Infinite
                    DoWorkAsyncInfiniteLoop();
                }
            });
        }

        /// <summary>
        /// ViewModel da Pagina
        /// </summary>
        public IndexViewModel ViewModel
        {
            get; set;
        }

        protected async override void OnAppearing()
        {

            try
            {
                // Carrega Inicio
                ViewModel.Carrega(await this.DisplayAlert("teste", "Ver diaria iniciada ?", "nao", "sim"));

                // Inicia Pesquisa
                MessagingCenter.Send<string, int>("ObservableChamada", "ObservableChamada", 1);
            }
            catch (DiariaException e)
            {
                // Alerta
                await this.DisplayAlert("Atenção", e.Message, "OK");
                // Redirect
                App.Current.MainPage = new DiariaDeBordo();
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
        }

        private async void DoWorkAsyncInfiniteLoop()
        {

            Device.StartTimer(TimeSpan.FromSeconds(5),  () => {

                // Verifica se pesquisa esta ativa
                if (UsuarioAutenticado.Instance.StatusAplicatico)
                {
                    // Busca Chamado Corrida
                    if (true)
                    {
                        Device.BeginInvokeOnMainThread(async () => {

                            // Busca Corrida
                            ViewModel.BuscaCorrida();

                            // Atualiza tela
                            await ChamadoEncontradoPainel();

                        });

                        // Encerra busca
                        UsuarioAutenticado.Instance.StatusAplicatico = false;
                        return false;
                    }

                    // Repete
                    return true;
                }
                else
                    return false;

            });
        }

        private async void EfeitoPesquisaAtivada()
        {
            // Pesquisa Ativada
            UsuarioAutenticado.Instance.StatusAplicatico = true;

            // Animação pesquisa ativa
            if (findIcon.AnimationIsRunning("RotateTo"))
                ViewExtensions.CancelAnimations(findIcon);
            else
            {
                await findIcon.RotateTo(360, 2000);
                findIcon.Rotation = 0;
            }

            // Altera Label
            lblStatus.Text = "ONLINE";
            lblStatus.TextColor = Color.DarkGreen;

            // Desativa Painel
            await ProcurandoChamadoPainel();
        }

        private async void EfeitoPesquisaDesativada()
        {
            // Pesquisa Ativada
            UsuarioAutenticado.Instance.StatusAplicatico = false;

            // Cancela animação
            if (findIcon.AnimationIsRunning("RotateTo"))
                ViewExtensions.CancelAnimations(findIcon);

            // Altera Label
            lblStatus.Text = "OFFLINE";
            lblStatus.TextColor = Color.Red;

            // Ativa Painel
            await ProcurandoChamadoPainel();
        }

        public void AceitarCorrida(object sender, EventArgs args)
        {
            // Troca de página após Login
            App.Current.MainPage = new Detalhe();
        }

        public async void RecusarCorrida(object sender, EventArgs args)
        {
            await ProcurandoChamadoPainel();
        }

        private async System.Threading.Tasks.Task ChamadoEncontradoPainel()
        {
            await this.ctnProcurando.TranslateTo(-500, -20, 500);
            this.ctnProcurando.IsVisible = false;

            await this.ctnEncontrado.TranslateTo(-500, -20, 400);
            this.ctnEncontrado.IsVisible = true;
            await this.ctnEncontrado.TranslateTo(0, 0, 400);
        }

        private async System.Threading.Tasks.Task ProcurandoChamadoPainel()
        {
            await this.ctnProcurando.TranslateTo(0, 0, 300);
            this.ctnProcurando.IsVisible = true;

            await this.ctnEncontrado.TranslateTo(-500, -20, 300);
            this.ctnEncontrado.IsVisible = false;
        }

        private void RegistrarBordo_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new DiariaDeBordo();
        }
    }
}