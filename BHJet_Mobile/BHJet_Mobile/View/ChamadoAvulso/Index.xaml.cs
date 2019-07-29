using BHJet_Mobile.DependencyService;
using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using BHJet_Mobile.View.Diaria;
using BHJet_Mobile.ViewModel;
using System;
using System.Threading;
using System.Threading.Tasks;
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
                switch (UsuarioAutenticado.Instance.StatusAplicatico)
                {
                    case BHJet_Enumeradores.StatusAplicativoEnum.ChamadoEncontrado:
                        await ChamadoEncontradoPainel();
                        break;
                    case BHJet_Enumeradores.StatusAplicativoEnum.Pausado:
                        // Altera situacao
                        UsuarioAutenticado.Instance.StatusAplicatico = BHJet_Enumeradores.StatusAplicativoEnum.Pesquisando;
                        // Ativa Efeito Pesquisa
                        EfeitoPesquisaAtivada();
                        // Ativa Infinite
                        DoWorkAsyncInfiniteLoop();
                        break;
                    case BHJet_Enumeradores.StatusAplicativoEnum.Pesquisando:
                        // Altera situacao
                        UsuarioAutenticado.Instance.StatusAplicatico = BHJet_Enumeradores.StatusAplicativoEnum.Pausado;
                        // Cancela pesquisa
                        UsuarioAutenticado.Instance.CancelaPesquisaChamado();
                        // Desativa Efeito
                        EfeitoPesquisaDesativada();
                        break;
                }
            });
        }

        private bool Logando { get; set; }

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

                // Configura Pagina
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // If Status Motoristas
                    switch (UsuarioAutenticado.Instance.StatusAplicatico)
                    {
                        case BHJet_Enumeradores.StatusAplicativoEnum.Atendimento:
                            App.Current.MainPage = new Detalhe();
                            break;

                        case BHJet_Enumeradores.StatusAplicativoEnum.ChamadoEncontrado:
                            await ChamadoEncontradoPainel();
                            break;

                        case BHJet_Enumeradores.StatusAplicativoEnum.Pausado:
                            // Cancela pesquisa
                            UsuarioAutenticado.Instance.CancelaPesquisaChamado();
                            // Desativa Efeito
                            EfeitoPesquisaDesativada();
                            break;

                        case BHJet_Enumeradores.StatusAplicativoEnum.Pesquisando:
                            // Ativa Efeito Pesquisa
                            EfeitoPesquisaAtivada();
                            // Ativa Infinite
                            DoWorkAsyncInfiniteLoop();
                            break;

                        default:
                            // Ativa Efeito Pesquisa
                            EfeitoPesquisaAtivada();
                            // Ativa Infinite
                            DoWorkAsyncInfiniteLoop();
                            break;
                    }
                });
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
            finally
            {
                ViewModel.Loading = false;
            }
        }

        private async void DoWorkAsyncInfiniteLoop()
        {
            try
            {
                if (UsuarioAutenticado.Instance.CancelaPesquisa != null)
                    UsuarioAutenticado.Instance.CancelaPesquisa.Cancel();
                UsuarioAutenticado.Instance.CancelaPesquisa = new CancellationTokenSource();

                await System.Threading.Tasks.Task.Run(() =>
                {
                    Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                   {
                       // Verifica se pesquisa esta ativa
                       if (UsuarioAutenticado.Instance.StatusAplicatico == BHJet_Enumeradores.StatusAplicativoEnum.Pesquisando)
                       {
                           // Cancela
                           if (UsuarioAutenticado.Instance.CancelaPesquisa.IsCancellationRequested) return false;

                           // Envia localizacao e disponibilidade
                           UsuarioAutenticado.Instance.AlteraDisponibilidade(true, false).Wait();

                           // Busca Corrida - Diaria
                           var resultado = ViewModel.BuscaCorrida();
                           if (resultado.Key)
                           {
                               Device.BeginInvokeOnMainThread(async () =>
                               {
                                   // Redireciona para o tipo de chamado
                                   if (resultado.Value == BHJet_Enumeradores.TipoContrato.ChamadosAvulsos)
                                   {
                                       // Aviso Vibracao
                                       Xamarin.Essentials.Vibration.Vibrate(1000);
                                       // Aviso Sonoro
                                       TextSpeechUtil.ExecutarVoz($"Corrida encontrada. Endereço inícial, {ViewModel.chamadoItem.DestinoInicial}.");
                                       // Atualiza tela
                                       await ChamadoEncontradoPainel(true);
                                       Xamarin.Essentials.Vibration.Vibrate(1000);
                                   }
                               });

                               // Redireciona para o tipo de chamado
                               if (resultado.Value == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                               {
                                   UsuarioAutenticado.Instance.StatusAplicatico = BHJet_Enumeradores.StatusAplicativoEnum.Diarista;
                                   UsuarioAutenticado.Instance.CancelaPesquisa.Cancel();
                                   App.Current.MainPage = new DiariaDeBordo();
                                   return false;
                               }
                               else
                               {
                                   // Encerra busca
                                   UsuarioAutenticado.Instance.StatusAplicatico = BHJet_Enumeradores.StatusAplicativoEnum.ChamadoEncontrado;
                                   UsuarioAutenticado.Instance.CancelaPesquisa.Cancel();
                                   App.Current.MainPage = new Index();
                                   return false;
                               }
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
            // Cancela animação
            this.AbortAnimation("PesquisaIcon");
            // Altera Label
            lblStatus.Text = "OFFLINE";
            lblStatus.TextColor = Color.Red;

            // Ativa Painel
            await ProcurandoChamadoPainel();
        }

        public async void AceitarCorrida(object sender, EventArgs args)
        {
            try
            {
                // Loading on
                ViewModel.Loading = true;

                // Cancela por inatividade
                if (UsuarioAutenticado.Instance.CancelaPorInatividade != null)
                    UsuarioAutenticado.Instance.CancelaPorInatividade.Cancel();

                // Controle
                await ViewModel.AceitarCorrida();

                // Troca de página após Login
                App.Current.MainPage = new Detalhe();
            }
            catch (Exception e)
            {
                // Libera OS
                LiberarCorrida();

                // Trata
                this.TrataExceptionMobile(e);
            }
            finally
            {
                // Loading off
                ViewModel.Loading = false;
            }
        }

        public async void RecusarCorrida(object sender, EventArgs args)
        {
            try
            {
                // Recusa
                await ViewModel.RecusarCorrida();

                // Cancela por inatividade
                if (UsuarioAutenticado.Instance.CancelaPorInatividade != null)
                    UsuarioAutenticado.Instance.CancelaPorInatividade.Cancel();
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
            finally
            {
                await FinalizaAtendimento();
            }
        }

        public async void LiberarCorrida()
        {
            try
            {
                await ViewModel.LiberarCorrida();
            }
            catch (Exception e)
            {
                this.TrataExceptionMobile(e);
            }
            finally
            {
                await FinalizaAtendimento();
                if (UsuarioAutenticado.Instance.CancelaPorInatividade != null)
                    UsuarioAutenticado.Instance.CancelaPorInatividade.Cancel();
            }
        }

        private async System.Threading.Tasks.Task FinalizaAtendimento()
        {
            UsuarioAutenticado.Instance.FinalizaAtendimento();
            await ProcurandoChamadoPainel();
            EfeitoPesquisaAtivada();
            //DoWorkAsyncInfiniteLoop();
            App.Current.MainPage = new Index();
        }

        private async System.Threading.Tasks.Task ChamadoEncontradoPainel(bool cancelaTempo = false)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.ViewModel.ChamadoEncontrado = true;
            });
            this.ViewModel.ChamadoEncontrado = true;
            this.ViewModel.OnPropertyChanged("ChamadoEncontrado");
            this.ApplyBindings();

            if (cancelaTempo)
            {
                UsuarioAutenticado.Instance.CancelaPorInatividade = new CancellationTokenSource();
                Task.Delay(40000).ContinueWith(t =>
                {
                    if (UsuarioAutenticado.Instance.StatusAplicatico == BHJet_Enumeradores.StatusAplicativoEnum.ChamadoEncontrado)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            // Recusa Corrida
                            // RecusarCorrida(null, new EventArgs());
                            LiberarCorrida();
                            // Aviso Sonoro
                            TextSpeechUtil.ExecutarVoz($"Corrida rejeitada por inativídade.");
                        });
                    }
                }, UsuarioAutenticado.Instance.CancelaPorInatividade.Token);
            }
        }

        private async System.Threading.Tasks.Task ProcurandoChamadoPainel()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.ViewModel.ChamadoEncontrado = false;
            });
            this.ViewModel.ChamadoEncontrado = false;
            this.ViewModel.OnPropertyChanged("ChamadoEncontrado");
            this.ApplyBindings();
        }

        private void RegistrarBordo_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new DiariaDeBordo();
        }
    }
}