using BHJet_Mobile.Infra;
using BHJet_Mobile.Infra.Extensao;
using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.Sessao;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BHJet_Mobile.ViewModel.Corrida
{
    public class DetalheViewModel : PropertyChangedClass
    {
        public DetalheViewModel(IUsuarioAutenticado _usuarioAutenticado, ICorridaServico _corridaServico)
        {
            usuarioAutenticado = _usuarioAutenticado;
            corridaServico = _corridaServico;
        }

        private readonly IUsuarioAutenticado usuarioAutenticado;

        private readonly ICorridaServico corridaServico;

        private ObservableCollection<DetalheItemViewModel> _ListaLogs;
        public ObservableCollection<DetalheItemViewModel> ListaLogs
        {
            get
            {
                return _ListaLogs;
            }
            set
            {
                _ListaLogs = value;
                OnPropertyChanged();
            }
        }

        private long _idCorrida;
        public long idCorrida
        {
            get
            {
                return _idCorrida;
            }
            set
            {
                _idCorrida = value;
                OnPropertyChanged();
            }
        }

        public async Task Carrega()
        {
            try
            {
                // Load
                Loading = true;

                // Busca Log Corrida
                var logCorrida = await corridaServico.BuscaLogCorrida(usuarioAutenticado.IDCorridaAtendimento ?? 0);

                // Binding
                idCorrida = usuarioAutenticado.IDCorridaAtendimento ?? 0;
                ListaLogs = logCorrida.Select(lg => new DetalheItemViewModel()
                {
                    idCorrida = lg.idCorrida,
                    idEnderecoCorrida = lg.idEnderecoCorrida,
                    IDOcorrencia = lg.IDOcorrencia,
                    HoraChegada = lg.dtHoraChegada,
                    EnderecoCompleto = lg.EnderecoCompleto,
                    Observacao = lg.Atividade + Environment.NewLine + lg.Observacao,
                    Latitude = lg.Latitude,
                    Longitude = lg.Longitude,
                    Status = lg.Status,
                    TelefoneContato = lg.TelefoneContato,
                    RegistroFoto = lg.RegistroFoto,
                    PessoaContato = lg.PessoaContato,
                    Atividade = lg.Atividade
                }).ToObservable();
            }
            finally
            {
                Loading = false;
            }
        }

        public Tuple<decimal, decimal> BuscaLocalizacaoLog(long idEnderecoCorrida)
        {
            // Busca LOG
            var log = ListaLogs.Where(lg => lg.idEnderecoCorrida == idEnderecoCorrida).FirstOrDefault();
            // Return Localizacao
            return new Tuple<decimal, decimal>(log.Latitude, log.Longitude);
        }

        public async Task RegistroFotoDocumento(long idEnderecoCorrida, byte[] foto)
        {
            // Registro de documento
            await corridaServico.CadastraProtocolo(foto, idEnderecoCorrida);

            // Busca LOG
            var log = ListaLogs.Where(lg => lg.idCorrida == idEnderecoCorrida).FirstOrDefault();

            // Registro
            log.RegistroFoto = foto;
        }

        public async Task RegistrarChegada(long idEnderecoCorrida)
        {
            // Busca Log
            var log = ListaLogs.Where(lg => lg.idEnderecoCorrida == idEnderecoCorrida).FirstOrDefault();

            // Verifica
            if (log.HoraChegada == null)
                await corridaServico.RegistraChegaLogCorrida(idEnderecoCorrida);
            else
                throw new CorridaException($"A Hora de chegada foi registrada em {log.HoraChegada}.");
        }
    }
}
