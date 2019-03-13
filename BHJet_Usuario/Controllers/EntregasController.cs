using BHJet_Usuario.Models.Entregas;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Usuario.Controllers
{
    public class EntregasController : Controller
    {
        public EntregasController()
        {
        }

        // GET: Entregas
        public ActionResult Index(object model)
        {
            var origem = (EntregaModel)TempData["origemSolicitacao"];
            this.TempData["origemSolicitacao"] = origem;

            return View(origem);
        }

        [HttpPost]
        public ActionResult Index(EntregaModel model)
        {
            this.TempData["origemSolicitacao"] = model;

            return RedirectToAction("Resumo", "Entregas");
        }

        [HttpPost]
        public ActionResult Finaliza()
        {
            var model = (EntregaModel)TempData["origemSolicitacao"];

            model.Enderecos.Add(new EnderecoModel()
            {

            });

            this.TempData["origemSolicitacao"] = model;

            return View(model);
        }

        // GET: Resumo
        public ActionResult Resumo()
        {
            var origem = (EntregaModel)TempData["origemSolicitacao"];
            this.TempData["origemSolicitacao"] = origem;

            return View(origem);
        }

        // POST: Resumo
        [HttpPost]
        public ActionResult Resumo(EntregaModel model)
        {
            var origem = (EntregaModel)TempData["origemSolicitacao"];
            this.TempData["origemSolicitacao"] = origem;

            // Se Logado redireciona para pagamento
            return RedirectToAction("Pagamento", "Pagamento");

            //return View(origem);
        }
    }
}