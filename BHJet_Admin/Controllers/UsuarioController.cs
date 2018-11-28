using BHJet_Admin.Models.Usuario;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View(new UsuariosModel()
            {
                usuarios = new UsuarioModel[]
                 {
                     new UsuarioModel()
                     {
                          ID = 1,
                          Email ="teste@teste.com",
                          Situacao = true,
                           SituacaoDesc = "Ativo",
                          TipoUser = BHJet_Core.Enum.TipoUsuario.Administrador
                     }
                 }

            });
        }

        // POST: Usuario
        [HttpPost]
        public ActionResult Index(UsuariosModel model)
        {
            return View(new UsuariosModel()
            {
                usuarios = new UsuarioModel[]
                 {
                     new UsuarioModel()
                     {
                          ID = 1,
                          Email ="teste@teste.com",
                          Situacao = true,
                           SituacaoDesc = "Ativo",
                          TipoUser = BHJet_Core.Enum.TipoUsuario.Administrador
                     }
                 }

            });
        }





    }

}
