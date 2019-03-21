using BHJet_Admin.Models;
using BHJet_Servico.Corrida;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class EntregasController : Controller
    {
        private readonly ICorridaServico corridaServico;

        public EntregasController(ICorridaServico _corridaServico)
        {
            corridaServico = _corridaServico;
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
        public JsonResult Finaliza(EntregaModel model)
        {
            model.Enderecos.Add(new EnderecoModel()
            {

            });

            this.TempData["origemSolicitacao"] = model;

            return Json("", JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public JsonResult BcTpOc()
        {
            // Recupera dados
            var entidade = corridaServico.BuscaOcorrencias();

            // Return
            return Json(entidade.Select(x => new AutoCompleteModel()
            {
                label = x.IDSolicitacao + " - " + x.DescricaoSolicitacao,
                value = x.IDSolicitacao
            }), JsonRequestBehavior.AllowGet);
        }
    }
}