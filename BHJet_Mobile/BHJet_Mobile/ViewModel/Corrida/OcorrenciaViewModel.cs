using BHJet_CoreGlobal;
using BHJet_Mobile.Infra;
using BHJet_Mobile.Infra.Database;
using BHJet_Mobile.Infra.Database.Tabelas;
using BHJet_Mobile.Infra.Extensao;
using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.Servico.Corrida.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHJet_Mobile.ViewModel.Corrida
{
    public class OcorrenciaViewModel : PropertyChangedClass
    {
        public OcorrenciaViewModel(ICorridaServico _corridaServico, long idCorrida)
        {
            corridaServico = _corridaServico;
            IDCorrida = idCorrida;
        }

        private readonly ICorridaServico corridaServico;

        private long _IDCorrida;
        public long IDCorrida
        {
            get
            {
                return _IDCorrida;
            }
            set
            {
                _IDCorrida = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<OcorrenciaItemViewModel> _ListaOcorrencias;
        public ObservableCollection<OcorrenciaItemViewModel> ListaOcorrencias
        {
            get
            {
                return _ListaOcorrencias;
            }
            set
            {
                _ListaOcorrencias = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Carregamento
        /// </summary>
        /// <returns></returns>
        public async Task CarregaTela()
        {
            try
            {
                // On
                Loading = false;

                // Carrega Dados
                var entidade = await corridaServico.BuscaOcorrencias();

                // Binding
                ListaOcorrencias = entidade.Select(oc => new OcorrenciaItemViewModel()
                {
                    DescricaoStatus = oc.DescricaoStatus,
                    StatusCorrida = oc.StatusCorrida,
                    Inicia = oc.Inicia,
                    Finaliza = oc.Finaliza,
                    Cancela = oc.Cancela
                }).ToObservable();
            }
            finally
            {
                // Off
                Loading = false;
            }
        }

        /// <summary>
        /// Tratamento Ocorrencia selecionada
        /// </summary>
        /// <param name="idOcorrencia"></param>
        public async void OcorrenciaSelecionada(int idOcorrencia)
        {
            try
            {
                // On
                Loading = false;

                // Busca ocorrencia
                var ocorrencia = ListaOcorrencias.Where(oc => oc.StatusCorrida == idOcorrencia).FirstOrDefault();

                // Grava ocorrencia
                await corridaServico.AtualizaOcorrenciaCorrida(idOcorrencia, IDCorrida);

                // Verificacoes
                if (ocorrencia.Finaliza || ocorrencia.Cancela)
                {
                    // Encerrar OS
                    var kmPercorrido = await BuscaDistanciaPercorrida();
                    await corridaServico.EncerrarOrdemServico(idOcorrencia, IDCorrida, new EncerrarCorridaFiltro()
                    {
                        KilometragemRodada = kmPercorrido
                    });
                    throw new CorridaException("Corrida Encerrada"); // Finaliza
                }
            }
            finally
            {
                // Off
                Loading = false;
            }
        }

        /// <summary>
        /// Encerrar OS
        /// </summary>
        /// <returns></returns>
        public async Task EncerrarOrdemServico()
        {
            try
            {
                // On
                Loading = false;

                // Encerrar OS
                var kmPercorrido = await BuscaDistanciaPercorrida();
                await corridaServico.EncerrarOrdemServico(null, IDCorrida, new EncerrarCorridaFiltro()
                {
                    KilometragemRodada = kmPercorrido
                });

                // Finaliza
                throw new CorridaException("Corrida Encerrada");
            }
            finally
            {
                // Off
                Loading = false;
            }
        }


        private async Task<double> BuscaDistanciaPercorrida()
        {
            using (var db = new Database())
            {
                // Insere localizacao
                var trajetoPercorrido = await db.BuscaItems<LocalizacaoCorrida>();

                // Calcula distancia
                return DistanciaUtil.CalculaDistancia(trajetoPercorrido.Select(dist => new Localidade()
                {
                    Latitude = dist.Latitude,
                    Longitude = dist.Longitude
                }).ToArray());
            }
        }
    }

    public class OcorrenciaItemViewModel : PropertyChangedClass
    {
        private long _StatusCorrida;
        public long StatusCorrida
        {
            get
            {
                return _StatusCorrida;
            }
            set
            {
                _StatusCorrida = value;
                OnPropertyChanged();
            }
        }

        private string _DescricaoStatus;
        public string DescricaoStatus
        {
            get
            {
                return _DescricaoStatus;
            }
            set
            {
                _DescricaoStatus = value;
                OnPropertyChanged();
            }
        }

        private bool _Inicia;
        public bool Inicia
        {
            get
            {
                return _Inicia;
            }
            set
            {
                _Inicia = value;
                OnPropertyChanged();
            }
        }

        private bool _Finaliza;
        public bool Finaliza
        {
            get
            {
                return _Finaliza;
            }
            set
            {
                _Finaliza = value;
                OnPropertyChanged();
            }
        }

        private bool _Cancela;
        public bool Cancela
        {
            get
            {
                return _Cancela;
            }
            set
            {
                _Cancela = value;
                OnPropertyChanged();
            }
        }
    }
}
