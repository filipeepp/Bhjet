using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Core.Variaveis;
using BHJet_Servico.Autorizacao;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BHJet_Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAutorizacaoServico autorizacaoServico;

        public HomeController(IAutorizacaoServico _autorizacaoServico)
        {
            autorizacaoServico = _autorizacaoServico;
        }

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
                try
                {
                    // Autentica Usuario
                    var modelUsu = autorizacaoServico.Autenticar(model.Login, model.Senha);

                    // Tickets
                    var userData = JsonConvert.SerializeObject(model.Login);
                    var ticket = new FormsAuthenticationTicket(1, model.Login, DateTime.Now, DateTime.Now.AddMinutes(120), false, userData, FormsAuthentication.FormsCookiePath);
                    var encryptedCookie = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedCookie) { Expires = DateTime.Now.AddHours(2) };

                    // Cookies
                    Response.Cookies.Add(cookie);

                    // Session
                    Session["IDTKUsuarioJet"] = modelUsu.access_token.ToString();

                }
                catch (Exception e)
                {
                    ViewBag.ErroLogin = e.Message ?? Mensagem.Validacao.UsuarioNaoEncontrato;
                    return View(new LoginModel());
                }

                // Return
                return RedirectToAction("Index", "Home");
            }
            else
                return View(model);
        }
    }
}
