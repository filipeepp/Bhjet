using System.Web;
using System.Web.Mvc;

namespace BHJet_Admin.Infra
{
    public class ValidacaoUsuarioAttribute : AuthorizeAttribute
    {
        public ValidacaoUsuarioAttribute()
        {
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            CheckIfUserIsAuthenticated(filterContext);
        }

        private void CheckIfUserIsAuthenticated(AuthorizationContext filterContext)
        {
            // Tkn Usuario Logado
            var tknBrUs = HttpContext.Current.Session["IDTKUsuarioJet"]?.ToString();

            // Validacao Usuario
            if (!HttpContext.Current.User.Identity.IsAuthenticated || string.IsNullOrWhiteSpace(tknBrUs))
            {
                filterContext.Controller.TempData["Error"] = "Favor realizar o login antes de prosseguir.";
                filterContext.Result = new RedirectResult("~/Home/Login");
            }
        }
    }
}