using BHJet_Mobile.Infra;
using BHJet_Mobile.Servico.Diaria;
using BHJet_Mobile.Sessao;
using System.Threading.Tasks;

namespace BHJet_Mobile.ViewModel.DiariaDeBordo
{
    public class DiariaDeBordoViewModel : PropertyChangedClass
    {
        public DiariaDeBordoViewModel(IUsuarioAutenticado _usuarioAutenticado, IDiariaServico _diariaServico)
        {
            usuarioAutenticado = _usuarioAutenticado;
            diariaServico = _diariaServico;
        }

        private readonly IUsuarioAutenticado usuarioAutenticado;

        private readonly IDiariaServico diariaServico;

        private TurnoItemViewModel _turnoItem;
        public TurnoItemViewModel TurnoItem
        {
            get
            {
                return _turnoItem;
            }
            set
            {
                _turnoItem = value;
                OnPropertyChanged();
            }
        }

        public string NomeCompleto
        {
            get
            {
                return usuarioAutenticado.Nome;
            }
        }


        public async Task Carrega()
        {
            try
            {
                // Load
                Loading = true;

                // Busca dados diaria de bordo
                var model = await diariaServico.BuscaTurno();

                // Binding
                TurnoItem = new TurnoItemViewModel()
                {
                    InicioJornada = model.DataInicio,
                    KMInicio = model.KMInicio,
                    InicioAlmoco = model.DataInicioIntervalo,
                    KMAlmoco = model.KMInicioIntervalo,
                    FimAlmoco = model.DataFimIntervalo,
                    KMFimAlmoco = model.KMFimInvervalo,
                    FimJornada = model.DataFim,
                    KMFim = model.KMFim
                };
            }
            finally
            {
                Loading = false;
            }
        }

        public async Task AtualizaTurno()
        {
            try
            {
                // Load
                Loading = true;

                // Validacao
                if (string.IsNullOrWhiteSpace(TurnoItem.InicioJornada) && string.IsNullOrWhiteSpace(TurnoItem.KMInicio) &&
                    string.IsNullOrWhiteSpace(TurnoItem.InicioAlmoco) && string.IsNullOrWhiteSpace(TurnoItem.KMAlmoco) &&
                    string.IsNullOrWhiteSpace(TurnoItem.FimAlmoco) && string.IsNullOrWhiteSpace(TurnoItem.KMFimAlmoco) &&
                    string.IsNullOrWhiteSpace(TurnoItem.FimJornada) && string.IsNullOrWhiteSpace(TurnoItem.KMFim))
                    throw new ErrorException("Favor preencher os dados de bordo antes de prosseguir.");

                // Atualiza dados diaria de bordo
                await diariaServico.AtualizaTurno(new Servico.Diaria.Model.TurnoModel()
                {
                    KMFim = TurnoItem.KMFim,
                    DataFim = TurnoItem.FimJornada,
                    DataFimIntervalo = TurnoItem.FimAlmoco,
                    KMFimInvervalo = TurnoItem.KMFimAlmoco,
                    DataInicio = TurnoItem.InicioJornada,
                    DataInicioIntervalo = TurnoItem.InicioAlmoco,
                    KMInicio = TurnoItem.KMInicio,
                    KMInicioIntervalo = TurnoItem.KMAlmoco
                });
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
