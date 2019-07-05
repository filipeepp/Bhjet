using BHJet_Mobile.DependencyService;
using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.Diaria;
using BHJet_Mobile.ViewModel;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BHJet_Mobile.View.ChamadoAvulso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Index : ContentPage
    {
        public Index(bool logando = false)
        {
            InitializeComponent();
            ViewModel = new IndexViewModel(UsuarioAutenticado.Instance, new MotoristaServico(), new CorridaServico());
            BindingContext = ViewModel;
            Logando = logando;
            MessagingCenter.Unsubscribe<string, int>("ObservableChamada", "ObservableChamada");
            MessagingCenter.Subscribe<string, int>("ObservableChamada", "ObservableChamada", async (s, a) =>
            {
                // If Status Motoristas
                if (UsuarioAutenticado.Instance.StatusAplicatico)
                {
                    // Cancela pesquisa
                    UsuarioAutenticado.Instance.StatusAplicatico = false;
                    UsuarioAutenticado.Instance.CancelaPesquisaChamado();
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

        private bool Logando { get; set; }

        public CancellationTokenSource CancelaEspera { get; set; }

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
                ViewModel.Carrega();

                // Tratamento de Logon
                if (Logando)
                {
                    // Logado
                    Logando = false;
                    // Inicia Pesquisa
                    if (UsuarioAutenticado.Instance.StatusAplicatico)
                    {
                        EfeitoPesquisaAtivada();
                        DoWorkAsyncInfiniteLoop();
                    }
                    else
                        EfeitoPesquisaDesativada();
                }
                // Permite Pesquisa Corrida
                else if (ViewModel.PermitePesquisaCorrida)
                {
                    EfeitoPesquisaAtivada();
                    DoWorkAsyncInfiniteLoop();
                }
                // Corrida encontrada
                else if (!UsuarioAutenticado.Instance.StatusAplicatico && UsuarioAutenticado.Instance.IDCorridaPesquisada != null)
                {
                    await ChamadoEncontradoPainel();
                }
                else if (!UsuarioAutenticado.Instance.StatusAplicatico)
                {
                    EfeitoPesquisaDesativada();
                }
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
        }

        private async void DoWorkAsyncInfiniteLoop()
        {
            try
            {
                UsuarioAutenticado.Instance.CancelaPesquisaChamado();
                UsuarioAutenticado.Instance.CancelaPesquisa = new CancellationTokenSource();

                await System.Threading.Tasks.Task.Run(() =>
                {
                    Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                   {
                       // Verifica se pesquisa esta ativa
                       if (UsuarioAutenticado.Instance.StatusAplicatico)
                       {
                           // Cancela
                           if (UsuarioAutenticado.Instance.CancelaPesquisa.IsCancellationRequested) return false;

                           // Busca Corrida - Diaria
                           var resultado = ViewModel.BuscaCorrida();
                           if (resultado.Key)
                           {
                               Device.BeginInvokeOnMainThread(async () =>
                               {
                                   // Aviso Vibracao
                                   Xamarin.Essentials.Vibration.Vibrate(1000);
                                   // Aviso Sonoro
                                   TextSpeechUtil.ExecutarVoz($"Corrida encontrada. Endereço inícial, {ViewModel.chamadoItem.DestinoInicial}.");

                                   // Redireciona para o tipo de chamado
                                   if (resultado.Value == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                                       App.Current.MainPage = new DiariaDeBordo();
                                   else
                                   {
                                       // Atualiza tela
                                       await ChamadoEncontradoPainel(true);
                                       Xamarin.Essentials.Vibration.Vibrate(1000);
                                   }
                               });

                               // Encerra busca
                               UsuarioAutenticado.Instance.StatusAplicatico = false;
                               return false;
                           }
                           return true;
                       }
                       else
                           return false;
                   });
                }).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
        }

        private async void EfeitoPesquisaAtivada()
        {
            // Pesquisa Ativada
            UsuarioAutenticado.Instance.StatusAplicatico = true;
            this.AbortAnimation("PesquisaIcon");
            new Animation {
                                 { 0, 0.5, new Animation (v => findIcon.Scale = v, 1, 2) },
                                 { 0, 1, new Animation (v => findIcon.Rotation = v, 0, 360) },
                                 { 0.5, 1, new Animation (v => findIcon.Scale = v, 2, 1) }
                              }.Commit(this, "PesquisaIcon", 16, 4000, null, (v, c) => findIcon.Rotation = 0, () => true);

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
            this.AbortAnimation("PesquisaIcon");
            // Altera Label
            lblStatus.Text = "OFFLINE";
            lblStatus.TextColor = Color.Red;

            // Ativa Painel
            await ProcurandoChamadoPainel();
        }

        public void AceitarCorrida(object sender, EventArgs args)
        {
            // Controle
            ViewModel.AceitarCorrida();
            // Troca de página após Login
            App.Current.MainPage = new Detalhe();
            if (CancelaEspera != null)
                CancelaEspera.Cancel();
        }

        public async void RecusarCorrida(object sender, EventArgs args)
        {
            try
            {
                await ViewModel.RecusarCorrida();
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
            finally
            {
                await FinalizaAtendimento();
                if (CancelaEspera != null)
                    CancelaEspera.Cancel();
            }
        }

        private async System.Threading.Tasks.Task FinalizaAtendimento()
        {
            UsuarioAutenticado.Instance.FinalizaAtendimento();
            await ProcurandoChamadoPainel();
            EfeitoPesquisaAtivada();
            DoWorkAsyncInfiniteLoop();
        }

        protected async override void OnDisappearing()
        {
            //bool finalizaAt = false;
            //try
            //{
            //    ViewModel.Loading = true;
            //    if (UsuarioAutenticado.Instance.IDCorridaAtendimento == null &&
            //    UsuarioAutenticado.Instance.IDCorridaPesquisada != null &&
            //    !UsuarioAutenticado.Instance.StatusAplicatico)
            //    {
            //        finalizaAt = true;
            //        await ViewModel.RecusarCorrida();
            //    }
            //}
            //catch (Exception e)
            //{
            //    this.TrataExceptionMobile(e);
            //}
            //finally
            //{
            //    if (finalizaAt)
            //        await FinalizaAtendimento();
            //}
        }

        private async System.Threading.Tasks.Task ChamadoEncontradoPainel(bool cancelaTempo = false)
        {
            //await this.ctnProcurando.TranslateTo(-500, -20, 500);
            this.ctnProcurando.IsVisible = false;
            this.imgLogo.IsVisible = false;
           // await this.ctnEncontrado.TranslateTo(-500, -20, 400);
            this.ctnEncontrado.IsVisible = true;
            this.ctnEncontrado.FadeTo(0, 3000);
            // await this.ctnEncontrado.TranslateTo(0, 0, 400);

            if (cancelaTempo)
            {
                CancelaEspera = new CancellationTokenSource();
                Task.Delay(40000).ContinueWith(t =>
                {
                    if (UsuarioAutenticado.Instance.IDCorridaAtendimento == null && !UsuarioAutenticado.Instance.StatusAplicatico)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                        // Recusa Corrida
                        RecusarCorrida(null, new EventArgs());
                        // Aviso Sonoro
                        TextSpeechUtil.ExecutarVoz($"Corrida rejeitada por inativídade.");
                        });
                    }
                }, CancelaEspera.Token);
            }
        }

        private async System.Threading.Tasks.Task ProcurandoChamadoPainel()
        {
           // await this.ctnProcurando.TranslateTo(0, 0, 300);
            this.ctnProcurando.IsVisible = true;
            this.imgLogo.IsVisible = true;
            //await this.ctnEncontrado.TranslateTo(-500, -20, 300);
            this.ctnEncontrado.IsVisible = false;
        }

        private void RegistrarBordo_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new DiariaDeBordo();
        }
    }
}