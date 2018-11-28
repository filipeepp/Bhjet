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
                          Situacao = "Ativo",
                          TipoUser = BHJet_Core.Enum.TipoUsuario.Administrador
                     }
                 }

            });
        }
    }
}