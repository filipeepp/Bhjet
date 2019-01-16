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
            try
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
            catch (Exception e)
            {
                this.TrataErro(e);
                return View(new ResumoModel()
                {
                    CarrosDisponiveis = 0,
                    ChamadosAvulsosAguardandoCarro = 0,
                    ChamadosAvulsosAguardandoMoto = 0,
                    MotociclistasDisponiveis = 0
                });
            }
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
                    var modelUsu = autorizacaoServico.Autenticar(new BHJet_Servico.Autorizacao.Filtro.AutenticacaoFiltro()
                    {
                        usuario = model.Login,
                        senha = model.Senha,
                        area = BHJet_Core.Enum.TipoAplicacao.Interna
                    });

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
