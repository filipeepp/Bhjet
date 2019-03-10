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


            return View(new EntregaModel
            {

                Enderecos = new List<EnderecoModel>()
                 {
                     new EnderecoModel()
                     {

                     },
                     new EnderecoModel()
                     {

                     }
                 }

            });
        }

        [HttpPost]
        public ActionResult Index(EntregaModel model)
        {

            model.Enderecos.Add(new EnderecoModel()
            {

            });

            return View(model);
        }
    }
}