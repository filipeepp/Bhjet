using BHJet_Usuario.Models.Entregas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHJet_Usuario.Controllers
{
    public class ResumoController : Controller
    {
        // GET: Resumo
        public ActionResult Index()
        {
            var origem = (EntregaModel)TempData["origemSolicitacao"];
            this.TempData["origemSolicitacao"] = origem;

            return View(origem);
        }

        [HttpGet]
        public ActionResult CallCB()
        {

            throw new Exception("gfgd");

        }

    }
}