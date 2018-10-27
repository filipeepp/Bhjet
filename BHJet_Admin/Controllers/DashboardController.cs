using BHJet_Core.Enum;
using BHJet_Core.Extension;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult ChamadoAvulsoEspera(DashboardTipoDisponivelEnum? tipoSolicitacao)
        {
            // Consiste Entrada
            if (tipoSolicitacao == null)
                return null;

            // Titulo
            ViewBag.TituloSolicitacao = tipoSolicitacao.Value.ToString().UpperCaseSeparete();

            return View();
        }


      
    }
}
