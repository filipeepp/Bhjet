using BHJet_Servico.Autorizacao;
using BHJet_Servico.Dashboard;
using BHJet_Usuario.Models.Entregas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHJet_Usuario.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var origem = new EntregaModel
            {
                Enderecos = new List<EnderecoModel>()
                 {
                     new EnderecoModel()
                     {
                     }
                 }
            };

            if (TempData["origemSolicitacao"] != null)
                origem = (EntregaModel)TempData["origemSolicitacao"];

            return View(origem);
        }

        [HttpPost]
        public ActionResult Index(EntregaModel model)
        {
            this.TempData["origemSolicitacao"] = model;

            return RedirectToAction("Index", "Entregas");
        }
    }
}