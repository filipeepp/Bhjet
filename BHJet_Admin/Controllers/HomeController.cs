using BHJet_Admin.Models;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View(new ResumoModel()
            {
                CarrosDisponiveis = 999,
                ChamadosAvulsosAguardandoCarro = 888,
                ChamadosAvulsosAguardandoMoto = 777,
                MotociclistasDisponiveis = 666
            });
        }

        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
    }
}
