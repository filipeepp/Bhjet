using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Core.Variaveis;
using BHJet_DTO.Autenticacao;
using BHJet_Servico.Autorizacao;
using BHJet_Servico.Dashboard;
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

        private readonly IResumoServico resumoServico;

        public HomeController(IAutorizacaoServico _autorizacaoServico, IResumoServico _resumoServico)
        {
            autorizacaoServico = _autorizacaoServico;
            resumoServico = _resumoServico;
        }

        [ValidacaoUsuarioAttribute()]
        public ActionResult Index()
        {
            // Busca dados Resumo
            var modelResumo = resumoServico.BuscaResumo();

            // Return
            return View(new ResumoModel()
            {
                CarrosDisponiveis = modelResumo.MotoristasDisponiveis,
                ChamadosAvulsosAguardandoCarro = modelResumo.ChamadosAguardandoMotorista,
                ChamadosAvulsosAguardandoMoto = modelResumo.ChamadosAguardandoMotociclista,
                MotociclistasDisponiveis = modelResumo.MotociclistaDisponiveis
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
                    //var modelUsu = autorizacaoServico.Autenticar(model.Login, model.Senha);
                    if (model.Login != "admin@bhjet.com.br" || model.Senha != "123456")
                        throw new Exception(Mensagem.Validacao.UsuarioNaoEncontrato);

                    var modelUsu = new TokenModel()
                    {
                        access_token = "_bqlkRnVgSsPSqT1-GOW2rtnzmE7TD9xpqmL4UM2yibyN-qJH839aT9JLalftP4b1pk0k_A76o3c5YWzWC8EjRUM2DTaO-FqcLDmSAYFdpD5mT7AgxTU163y8AyXyovSnJr5Pufmpv5WRUCdNzcwV5TwBOG9uULZbW_Mzrl9YfuMior-SjcIvMhyOfEN9d1m7XctHggGRNghoD2MtKP0OpdTA8I-m57bLhs11avq8ZyGSvKSP9fXSrTQ5qqdrFuF",
                    };

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
