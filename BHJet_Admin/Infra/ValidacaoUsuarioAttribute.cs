using BHJet_Enumeradores;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHJet_Admin.Infra
{
    public class ValidacaoUsuarioAttribute : AuthorizeAttribute
    {
        TipoUsuario[] TipoUsuario { get; set; }

        public ValidacaoUsuarioAttribute(params TipoUsuario[] tipoUsuario)
        {
            TipoUsuario = tipoUsuario;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            ValidacaoUsuario(filterContext);
        }

        private void ValidacaoUsuario(AuthorizationContext filterContext)
        {
            // Tkn Usuario Logado
            var tknBrUs = UsuarioLogado.Instance.bhTkUsu?.ToString();
            var tknTpUs = UsuarioLogado.Instance.BhjTpUsu;

            // Validacao Usuario
            if (!HttpContext.Current.User.Identity.IsAuthenticated || string.IsNullOrWhiteSpace(tknBrUs))
            {
                filterContext.Controller.TempData["Error"] = "Favor realizar o login antes de prosseguir.";
                filterContext.Result = new RedirectResult("~/Home/Login");
            }

            // Validacao Tipo Usuario
            if (TipoUsuario != null && tknTpUs != null && !TipoUsuario.Contains(tknTpUs ?? BHJet_Enumeradores.TipoUsuario.Visitante))
            {
                filterContext.Controller.TempData["Error"] = "Usuário não tem permissão de acesso a esta funcionalidade.";
                filterContext.Result = new RedirectResult("~/HomeExterno/Index");
            }
        }
    }
}