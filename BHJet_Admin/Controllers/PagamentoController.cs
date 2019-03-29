using BHJet_Admin.Models;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class PagamentoController : Controller
    {
        public ActionResult Pagamento()
        {
            var origem = (EntregaModel)TempData["origemSolicitacao"];
            this.TempData["origemSolicitacao"] = origem;

            return View(new PagamentoModel()
            {

            });
        }

        [HttpPost]
        public ActionResult Pagamento(PagamentoModel model)
        {
            var origem = (EntregaModel)TempData["origemSolicitacao"];
            this.TempData["origemSolicitacao"] = origem;

            return View(new PagamentoModel());
        }
    }
}