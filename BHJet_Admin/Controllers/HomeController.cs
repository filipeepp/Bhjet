using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BHJet_Admin.Controllers
{
    public class HomeController : Controller
    {
        [ValidacaoUsuarioAttribute()]
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
            if (TempData.ContainsKey("Error"))
                ViewBag.ErroLogin = TempData["Error"].ToString();

            return View(new LoginModel());
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }


        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // esta action trata o post (login)
                var usuario = new UsuarioModel()
                {
                    USUNOME = "LeonardoSilva",
                    USULOGIN = "11",
                    USUSENHA = "1",
                    USUTOKEN = "dd"
                };

                if (usuario == null || model.Login != "123@bhjet" || model.Senha != "321@bhjet")
                {
                    ViewBag.ErroLogin = "USUÁRIO e/ou SENHA inválido(s)!";
                    return View(model);
                }

                // Tickets
                var userData = JsonConvert.SerializeObject(usuario);
                var ticket = new FormsAuthenticationTicket(1, usuario.USUNOME, DateTime.Now, DateTime.Now.AddMinutes(120), false, userData, FormsAuthentication.FormsCookiePath);
                var encryptedCookie = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedCookie) { Expires = DateTime.Now.AddHours(2) };

                // Cookies
                Response.Cookies.Add(cookie);

                // Session
                Session["IDTKUsuarioJet"] = usuario.USUTOKEN.ToString();

                // Return
                return RedirectToAction("Index", "Home");
            }
            else
                return View(model);
        }
    }
}
