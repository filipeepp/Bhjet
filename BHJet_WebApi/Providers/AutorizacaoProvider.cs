using BHJet_Core.Variaveis;
using BHJet_Repositorio.Admin;
using BHJet_Repositorio.Entidade;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BHJet_WebApi.Providers
{
    public class AutorizacaoProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                // Busca e valida usuario
                var user = ValidaUsuario(ref context);

                if (context.HasError)
                    return;

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("sub", context.UserName));
                identity.AddClaim(new Claim("id", user.idUsuario.ToString()));
                identity.AddClaim(new Claim("role", "user"));

                context.Validated(identity);
            }
            catch (Exception e)
            {
                context.SetError("invalid_grant", e.Message);
            }
        }

        private static UsuarioEntidade ValidaUsuario(ref OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Válidar usuario aqui
            var user = new AutenticacaoRepositorio().BuscaUsuario(new BHJet_Repositorio.Filtro.ValidaUsuarioFiltro()
            {
                usuarioEmail = context.UserName,
                usuarioSenha = context.Password,
                usuarioTipo = BHJet_Core.Enum.TipoUsuario.Administrador
            });

            // Validacao
            if (user == null)
                throw new Exception(Mensagem.Validacao.UsuarioNaoEncontrato);

            // Usuario OK
            return user;
        }
    }
}