using BHJet_Core.Utilitario;
using BHJet_Core.Variaveis;
using BHJet_Mobile.Infra;
using BHJet_Servico.Autorizacao;
using System.Threading.Tasks;

namespace BHJet_Mobile.ViewModel.Login
{
    public class LoginViewModel : PropertyChangedClass
    {
        /// <summary>
        /// Construtor LoginViewModel
        /// </summary>
        /// <param name="_BeneficiarioNegocio"></param>
        public LoginViewModel(IAutorizacaoServico _autorizacaoServico)
        {
            Login = new LoginModel();
            autorizacaoServico = _autorizacaoServico;
        }

        private readonly IAutorizacaoServico autorizacaoServico;

        /// <summary>
        /// Usuario Logado
        /// </summary>
        public LoginModel Login { get; set; }

        /// <summary>
        /// Metodo de Login
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ExecutarLogin()
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
                var modelUsu = autorizacaoServico.Autenticar(new BHJet_Servico.Autorizacao.Filtro.AutenticacaoFiltro()
                {
                    usuario = Login.Username,
                    senha = Login.Password,
                    area = BHJet_Core.Enum.TipoAplicacao.Colaborador
                });

                // Busca perfil do usuario

                return true;
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
