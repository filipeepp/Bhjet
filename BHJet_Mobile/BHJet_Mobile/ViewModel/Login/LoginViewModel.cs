using BHJet_Mobile.Infra;
using BHJet_Mobile.Servico.Autenticacao;
using BHJet_Mobile.Infra.Variaveis;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using System.Threading.Tasks;
using BHJet_Mobile.Infra.Database;
using BHJet_Mobile.Infra.Database.Tabelas;
using BHJet_CoreGlobal;
using System.Linq;

namespace BHJet_Mobile.ViewModel.Login
{
    public class LoginViewModel : PropertyChangedClass
    {
        /// <summary>
        /// Construtor LoginViewModel
        /// </summary>
        /// <param name="_BeneficiarioNegocio"></param>
        public LoginViewModel(IAutenticacaoServico _autorizacaoServico, IMotoristaServico _motoristaServico, IUsuarioAutenticado _usuarioAutenticado)
        {
            Login = new LoginModel();
            autorizacaoServico = _autorizacaoServico;
            motoristaServico = _motoristaServico;
            usuarioAutenticado = _usuarioAutenticado;
        }

        private readonly IAutenticacaoServico autorizacaoServico;

        private readonly IMotoristaServico motoristaServico;

        private readonly IUsuarioAutenticado usuarioAutenticado;

        private string protSenha = "lg4v5S3m2nt3BAHJ0e1tA@u4t4hu1533r";

        /// <summary>
        /// Usuario Logado
        /// </summary>
        public LoginModel Login { get; set; }

        /// <summary>
        /// Metodo de Login
        /// </summary>
        /// <returns></returns>
        public async Task ExecutarLogin()
        {
            try
            {
                // Carregando
                Loading = true;
                OffLoading = false;

                // Validação
                if (string.IsNullOrWhiteSpace(Login.Username) || string.IsNullOrWhiteSpace(Login.Password))
                    throw new ErrorException(Mensagem.Validacao.UsuarioNaoEncontrato);

                // Autenticacao
                var modelUsu = autorizacaoServico.Autenticar(Login.Username, Login.Password);

                // Busca perfil do usuario
                var perfil = await motoristaServico.BuscaPerfilMotorista();

                // Usuario autenticado
                usuarioAutenticado.SetPerfil(perfil);

                // Salva login
                await GravaUsuario();
            }
            finally
            {
                // Finaliza loading
                Loading = false;
                OffLoading = true;
            }

        }

        private async Task GravaUsuario()
        {
            // Database
            using (var db = new Database())
            {
                await db.LimpaTabela("BHJetMotorista");
                await db.InsereItem(new UsuarioLogado()
                {
                    IDMotorista = usuarioAutenticado.IDProfissional,
                    usuario = Login.Username,
                    senha = CriptografiaUtil.Criptografa(Login.Password, protSenha)
                });
            }
        }

        public async Task<bool> BuscaUsuario()
        {
            try
            {
                // Carregando
                Loading = true;
                OffLoading = false;

                // Database
                using (var db = new Database())
                {
                    if (await db.ExisteTabela<UsuarioLogado>() == false)
                    {
                        await db.CriaTabela<UsuarioLogado>();
                        return false;
                    }
                    else
                    {
                        var item = await db.BuscaItems<UsuarioLogado>();

                        if (item == null || !item.Any())
                            return false;

                        var usuario = item.LastOrDefault();

                        if (usuario != null)
                        {
                            Login = new LoginModel()
                            {
                                Username = usuario.usuario,
                                Password = CriptografiaUtil.Descriptografa(usuario.senha, protSenha)
                            };

                            return true;
                        }
                        else
                            return false;
                    }
                }
            }
            finally
            {
                // Finaliza loading
                Loading = false;
                OffLoading = true;
            }
        }

    }
}
