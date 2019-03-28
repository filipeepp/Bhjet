﻿using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Core.Variaveis;
using BHJet_CoreGlobal;
using BHJet_DTO.Autenticacao;
using BHJet_DTO.Cliente;
using BHJet_Enumeradores;
using BHJet_Servico.Autorizacao;
using BHJet_Servico.Cliente;
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

        private readonly IClienteServico clienteServico;

        public HomeController(IAutorizacaoServico _autorizacaoServico, IResumoServico _resumoServico, IClienteServico _cliente)
        {
            autorizacaoServico = _autorizacaoServico;
            resumoServico = _resumoServico;
            clienteServico = _cliente;
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
            var origem = TempData["origemSolicitacao"] != null ? (EntregaModel)TempData["origemSolicitacao"] : null;
            this.TempData["origemSolicitacao"] = origem;
            var simulacao = TempData["simulandoCorrida"] != null ? (bool)TempData["simulandoCorrida"] : false;
            this.TempData["simulandoCorrida"] = simulacao;

            if (TempData.ContainsKey("Error"))
                ViewBag.ErroLogin = TempData["Error"].ToString();

            return View(new LoginModel()
            {
            });
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            UsuarioLogado.Logoff();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Registrar()
        {
            // Erro
            if (TempData.ContainsKey("Error"))
                ViewBag.ErroLogin = TempData["Error"].ToString();

            // Return
            return View(new RegistrarUsuarioModel());
        }

        [HttpPost]
        public ActionResult Registrar(RegistrarUsuarioModel model)
        {
            // Cadastra Cliente Externo
            if (ModelState.IsValid)
            {
                try
                {
                    ViewBag.ErroLogin = "";
                    // Verifica Senha
                    if (model.Senha != model.SenhaConfirm)
                    {
                        ViewBag.ErroLogin = "As senhas informadas não batem, favor informar corretamente.";
                        model.Senha = "";
                        model.SenhaConfirm = "";
                        return View(model);
                    }

                    // Incluir Cliente
                    clienteServico.IncluirClienteAvulso(new ClienteAvulsoDTO()
                    {
                        Nome = model.Nome,
                        Email = model.Email,
                        DataNascimento = model.DataNascimento ?? default(DateTime),
                        Celular = model.Celular,
                        Comercial = model.Comercial,
                        CPF = model.CPF,
                        CEP = model.CEP,
                        Rua = model.Rua,
                        Bairro = model.Bairro,
                        Cidade = model.Cidade,
                        Estado = model.Estado.ToString(),
                        NomeCartaoCredito = model.NomeCartaoCredito,
                        Numero = model.Numero ?? 0,
                        NumeroCartaoCredito = model.NumeroCartaoCredito.Replace(".",""),
                        Pais = model.Pais,
                        Senha = CriptografiaUtil.Criptografa(model.Senha, "ch4v3S3m2nt3BHJ0e1tA9u4t4hu1s33r"),
                        Sexo = model.Sexo.ToString(),
                        ValidadeCartaoCredito = model.ValidadeCartaoCredito
                    });

                    ViewBag.ErroLogin = string.Empty;
                    ViewBag.SucessoLogin = $"Bem vindo, {model.Nome}, cadastro realizado com sucesso";
                }
                catch (Exception e)
                {
                    ViewBag.ErroLogin = e.Message;
                    return View(model);
                }
                return RedirectToAction("Login", "Home");
            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var origem = TempData["origemSolicitacao"] != null ? (EntregaModel)TempData["origemSolicitacao"] : null;
            var simulacao = TempData["simulandoCorrida"] != null ? (bool)TempData["simulandoCorrida"] : false;
            this.TempData["simulandoCorrida"] = simulacao;
            long? cliente = null;
            if (ModelState.IsValid)
            {
                try
                {
                    // Autentica Usuario
                    var modelUsu = autorizacaoServico.Autenticar(new BHJet_Servico.Autorizacao.Filtro.AutenticacaoFiltro()
                    {
                        usuario = model.Login,
                        senha = model.Senha,
                        area = TipoAplicacao.Interna
                    });

                    // Tickets
                    var userData = JsonConvert.SerializeObject(model.Login);
                    var ticket = new FormsAuthenticationTicket(1, model.Login, DateTime.Now, DateTime.Now.AddMinutes(120), false, userData, FormsAuthentication.FormsCookiePath);
                    var encryptedCookie = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedCookie) { Expires = DateTime.Now.AddHours(2) };

                    // Cookies
                    Response.Cookies.Add(cookie);

                    // Buscar perfil
                    var perfil = autorizacaoServico.BuscaPerfil(modelUsu.access_token.ToString());
                    cliente = perfil.ClienteSelecionado;

                    // Session - Session["IDTKUsuarioJet"] = 
                    UsuarioLogado.Logar(perfil.ID.ToString(), perfil.ClienteSelecionado, modelUsu.access_token.ToString(), model.Login, perfil.TipoUsuario);
                }
                catch (Exception e)
                {
                    ViewBag.ErroLogin = e.Message ?? Mensagem.Validacao.UsuarioNaoEncontrato;
                    return View(new LoginModel());
                }

                if (simulacao && UsuarioLogado.Instance.BhjTpUsu == TipoUsuario.ClienteAvulsoSite)
                {
                    origem.IDCliente = cliente;
                    this.TempData["origemSolicitacao"] = origem;
                    return RedirectToAction("Resumo", "Entregas");
                }
                else if (UsuarioLogado.Instance.BhjTpUsu == TipoUsuario.Administrador)
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Index", "HomeExterno");

            }
            else
                return View(model);
        }
    }
}
