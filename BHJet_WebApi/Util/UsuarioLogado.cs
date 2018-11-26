using System.Linq;
using System.Security.Claims;
using System.Web;


namespace BHJet_WebApi.Util
{
    /// <summary>
    /// Usuario Logado no sistema
    /// </summary>
    public class UsuarioLogado
    {
        protected ClaimsIdentity Usuario { get; set; }

        /// <summary>
        /// Construtor usuario autenticado
        /// </summary>
        public UsuarioLogado()
        {
            Usuario = HttpContext.Current.User.Identity as ClaimsIdentity;
        }

        /// <summary>
        /// Login do usuario autenticado
        /// </summary>
        public string Login
        {
            get
            {
                return Usuario.Claims.Where(w => w.Type == "sub").FirstOrDefault().Value;
            }
        }

        /// <summary>
        /// ID do usuario autenticado
        /// </summary>
        public string LoginID
        {
            get
            {
                return Usuario.Claims.Where(w => w.Type == "id").FirstOrDefault().Value;
            }
        }
    }
}