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
            return View(new EntregaModel
            {

                Enderecos = new List<EnderecoModel>()
                 {
                     new EnderecoModel()
                     {

                     }
                 }

            });
        }

        [HttpPost]
        public ActionResult Index(EntregaModel model)
        {
            return RedirectToAction("Index", "Entregas", model);
        }
    }
}