using BHJet_Mobile.Infra;
using BHJet_Mobile.Servico.Diaria;
using BHJet_Mobile.Sessao;
using System.Threading.Tasks;

namespace BHJet_Mobile.ViewModel
{
    public class IndexViewModel : PropertyChangedClass
    {
        public IndexViewModel(IUsuarioAutenticado _usuarioAutenticado, IDiariaServico _diariaServico)
        {
            usuarioAutenticado = _usuarioAutenticado;
            diariaServico = _diariaServico;
        }

        private readonly IUsuarioAutenticado usuarioAutenticado;

        private readonly IDiariaServico diariaServico;

        public bool DiarioBordo
        {
            get
            {
                return usuarioAutenticado.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao ? true : false;
            }
        }

        private ChamadoEncontradoItemViewModel _chamadoItem;
        public ChamadoEncontradoItemViewModel chamadoItem
        {
            get
            {
                return _chamadoItem;
            }
            set
            {
                _chamadoItem = value;
                OnPropertyChanged();
            }
        }

        private bool _PermitePesquisaCorrida;
        public bool PermitePesquisaCorrida
        {
            get
            {
                return _PermitePesquisaCorrida;
            }
            set
            {
                _PermitePesquisaCorrida = value;
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

                // Carrega dados de corrida
                if (usuarioAutenticado.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
                {
                    // Verifica se a diaria foi aberta
                    if (await diariaServico.VerificaDiariaAberta())
                        PermitePesquisaCorrida = true;
                    else
                        PermitePesquisaCorrida = false;

                    // Alerta Pesquisa
                    if (!PermitePesquisaCorrida)
                        throw new DiariaException("Você está alocado para o cliente 'TALS', inicie o registro do turno para iniciar as corridas.");
                }
                else
                {
                    // Avulso sempre permite pesquisa
                    PermitePesquisaCorrida = true;
                }
            }
            finally
            {
                // Load
                Loading = false;
            }
        }

        public async Task BuscaCorrida()
        {
            // Buscando Corrida
            try
            {
                chamadoItem = new ChamadoEncontradoItemViewModel()
                {
                    NomeCliente = "Cliente fulano",
                    Comissao = "R$ 50,00",
                    DestinoInicial = "Rua teste, nº 10 - Bairro Centro"
                };
            }
            catch
            {

            }
        }


    }
}
