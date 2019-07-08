using BHJet_Enumeradores;
using BHJet_Mobile.Infra;
using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.Servico.Corrida.Model;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Servico.Motorista.Model;
using BHJet_Mobile.Sessao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHJet_Mobile.ViewModel
{
    public class IndexViewModel : PropertyChangedClass
    {
        public IndexViewModel(IUsuarioAutenticado _usuarioAutenticado, IMotoristaServico _motoristaServico, ICorridaServico _corrida)
        {
            usuarioAutenticado = _usuarioAutenticado;
            motoristaServico = _motoristaServico;
            corridaServico = _corrida;
        }

        private readonly IUsuarioAutenticado usuarioAutenticado;

        private readonly IMotoristaServico motoristaServico;

        private readonly ICorridaServico corridaServico;

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

        private bool _ChamadoEncontrado;
        public bool ChamadoEncontrado
        {
            get
            {
                return _ChamadoEncontrado;
            }
            set
            {
                _ChamadoEncontrado = value;
                OnPropertyChanged();
            }
        }

        private bool _ExibeLogomarca;
        public bool ExibeLogomarca
        {
            get
            {
                return _ExibeLogomarca;
            }
            set
            {
                _ExibeLogomarca = value;
                OnPropertyChanged();
            }
        }

        private long? _IDCorridaEncontrada;
        public long? IDCorridaEncontrada
        {
            get
            {
                return _IDCorridaEncontrada;
            }
            set
            {
                _IDCorridaEncontrada = value;
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

        public void Carrega()
        {
            try
            {
                // Load
                Loading = true;

                // Permite pesquisar corrida
                if (usuarioAutenticado.StatusAplicatico)
                    IDCorridaEncontrada = null;       

                // Busca Dados Corrida pesquisada
                var dadosCorridaPesquisa = GlobalVariablesManager.GetApplicationCurrentProperty(GlobalVariablesManager.VariaveisGlobais.DadosCorridaPesquisada) as ChamadoEncontradoItemViewModel;
                if (dadosCorridaPesquisa != null)
                    chamadoItem = dadosCorridaPesquisa;
            }
            finally
            {
                // Load
                Loading = false;
            }
        }

        public KeyValuePair<bool, TipoContrato> BuscaCorrida()
        {
            try
            {
                // Busca perfil do usuario
                var perfil = new PerfilMotoristaModel();
                Task.Run(async () =>
                {
                    perfil = await BuscaPerfilMotorista();
                }).Wait();

                // Se existe diaria
                if (perfil != null && perfil.idRegistroDiaria != null)
                {
                    usuarioAutenticado.SetPerfil(perfil);
                    return new KeyValuePair<bool, TipoContrato>(true, TipoContrato.ContratoLocacao);
                }
                else // Se nao - busca Corrida
                {
                    // Busca Corrida
                    var corrida = new CorridaAbertaModel();
                    IDCorridaEncontrada = null;
                    Task.Run(async () =>
                    {
                        corrida = await BuscaCorridaAberta();
                    }).Wait();

                    // Corrida
                    if (corrida != null)
                    {
                        // ID Corrida
                        IDCorridaEncontrada = corrida.ID;
                        usuarioAutenticado.IDCorridaPesquisada = corrida.ID;

                        // Binding
                        chamadoItem = new ChamadoEncontradoItemViewModel()
                        {
                            NomeCliente = corrida.NomeCliente,
                            Comissao = corrida.Comissao.ToString("C"),
                            DestinoInicial = corrida.EnderecoCompleto
                        };
                        GlobalVariablesManager.SetApplicationCurrentProperty(GlobalVariablesManager.VariaveisGlobais.DadosCorridaPesquisada, chamadoItem);

                        // Return
                        return new KeyValuePair<bool, TipoContrato>(true, TipoContrato.ChamadosAvulsos);
                    }
                    else
                    {
                        IDCorridaEncontrada = null;
                        usuarioAutenticado.IDCorridaAtendimento = null;
                        return new KeyValuePair<bool, TipoContrato>(false, TipoContrato.ChamadosAvulsos);
                    }
                }
            }
            finally
            {

            }
        }

        public void AceitarCorrida()
        {
            usuarioAutenticado.IDCorridaPesquisada = null;
            usuarioAutenticado.IDCorridaAtendimento = IDCorridaEncontrada;
        }

        private async Task<PerfilMotoristaModel> BuscaPerfilMotorista()
        {
            try
            {
                return await motoristaServico.BuscaPerfilMotorista();
            }
            catch
            {
                return null;
            }
        }

        private async Task<CorridaAbertaModel> BuscaCorridaAberta()
        {
            try
            {
                return await corridaServico.BuscaCorridaAberta(usuarioAutenticado.IDProfissional ?? 0, usuarioAutenticado.Tipo);
            }
            catch
            {
                return null;
            }
        }

        public async Task RecusarCorrida()
        {
            if (IDCorridaEncontrada != null)
            {
                usuarioAutenticado.IDCorridaPesquisada = null;
                await corridaServico.RecusarOrdemServico(IDCorridaEncontrada ?? 0);
                IDCorridaEncontrada = null;
            }
        }

        public async Task LiberarCorrida()
        {
            if (IDCorridaEncontrada != null)
            {
                usuarioAutenticado.IDCorridaPesquisada = null;
                await corridaServico.LiberarOrdemServico(IDCorridaEncontrada ?? 0);
                IDCorridaEncontrada = null;
            }
        }
    }
}
