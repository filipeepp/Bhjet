using BHJet_Admin.Models.Motorista;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class MotoristaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        #region Novo Motorista
        public ActionResult Novo()
        {
            return View(new NovoMotoristaModel());
        }

        [HttpPost]
        public ActionResult Novo(NovoMotoristaModel model)
        {
            return View(new NovoMotoristaModel());
        }
        #endregion


    }
}