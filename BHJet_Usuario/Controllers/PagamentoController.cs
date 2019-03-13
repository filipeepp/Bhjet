using BHJet_Usuario.Models.Entregas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHJet_Usuario.Controllers
{
    public class PagamentoController : Controller
    {
        // GET: Pagamento
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

            return View(new PagamentoModel()
            {
                 NumeroOS = 1555
            });
        }
    }
}