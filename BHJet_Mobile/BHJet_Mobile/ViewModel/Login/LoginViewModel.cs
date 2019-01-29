﻿using BHJet_Mobile.Infra;
using BHJet_Mobile.Servico.Autenticacao;
using BHJet_Mobile.Infra.Variaveis;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using System.Threading.Tasks;

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
