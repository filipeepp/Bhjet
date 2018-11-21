using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Dashboard;
using BHJet_Core.Enum;
using BHJet_Core.Extension;
using BHJet_Servico.Corrida;
using BHJet_Servico.Dashboard;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IResumoServico resumoServico;
        private readonly ICorridaServico corridaServico;

        public DashboardController(IResumoServico _resumoServico, ICorridaServico _corridaServico)
        {
            resumoServico = _resumoServico;
            corridaServico = _corridaServico;
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public ActionResult ExibeLocalizacao(DashboardTipoDisponivelEnum? tipoSolicitacao)
        {
            // Consiste Entrada
            if (tipoSolicitacao == null)
                return null;

            // Titulo
            TempData["TipoSolicitacao"] = tipoSolicitacao;
            ViewBag.TituloSolicitacao = tipoSolicitacao.Value.ToString().UpperCaseSeparete();

            return View();
        }

        [ValidacaoUsuarioAttribute()]
        public ActionResult ExibeLocalizacao(DashboardTipoDisponivelEnum? tipoSolicitacao, ResumoModel model)
        {
            // Consiste Entrada
            if (tipoSolicitacao == null)
                return null;

            // Titulo
            TempData["TipoSolicitacao"] = tipoSolicitacao;
            TempData["idProfissional"] = model.PesquisaMotociclista;
            ViewBag.TituloSolicitacao = tipoSolicitacao.Value.ToString().UpperCaseSeparete();

            return View();
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult BuscaLocalizacao()
        {
            // Consiste Entrada
            if (TempData["TipoSolicitacao"] == null)
                return null;

            // Modelo
            ExibeLocalizacaoModel[] localizacao = new ExibeLocalizacaoModel[] { };

            // Busca Localizacao
            switch (TempData["TipoSolicitacao"])
            {
                case DashboardTipoDisponivelEnum.MotociclistaDisponivel:
                    localizacao = resumoServico.BuscaLocalizacaoProfissionais(TipoProfissional.Motociclista)?.Select(x => new ExibeLocalizacaoModel()
                    {
                        id = x.idColaboradorEmpresaSistema,
                        geoPosicao = x.geoPosicao,
                        psCorrida = false,
                        desc = MontaDescricaoProfissional(x.idColaboradorEmpresaSistema, x.NomeColaborador, TipoProfissional.Motociclista)
                    }).ToArray();
                    break;
                case DashboardTipoDisponivelEnum.MotoristaDisponivel:
                    localizacao = resumoServico.BuscaLocalizacaoProfissionais(TipoProfissional.Motorista)?.Select(x => new ExibeLocalizacaoModel()
                    {
                        id = x.idColaboradorEmpresaSistema,
                        geoPosicao = x.geoPosicao,
                        psCorrida = false,
                        desc = MontaDescricaoProfissional(x.idColaboradorEmpresaSistema, x.NomeColaborador, TipoProfissional.Motorista)
                    }).ToArray();
                    break;
                case DashboardTipoDisponivelEnum.FuncionarioDisponivel:
                    if (int.TryParse(TempData["idProfissional"]?.ToString(), out int idProfissional))
                    {
                        var model = resumoServico.BuscaLocalizacaoProfissional(idProfissional);
                        localizacao = new ExibeLocalizacaoModel[]{
                            new ExibeLocalizacaoModel()
                            {
                                id = model.idColaboradorEmpresaSistema,
                                geoPosicao = model.geoPosicao,
                                psCorrida = false,
                                desc = MontaDescricaoProfissional(model.idColaboradorEmpresaSistema, model.NomeColaborador, model.TipoColaborador)
                            }
                        };
                    }
                    break;
                case DashboardTipoDisponivelEnum.ChamadoAguardandoCarros:
                    localizacao = resumoServico.BuscaLocalizacaoCorridas(StatusCorrida.AguardandoAtendimento, TipoProfissional.Motorista)?.Select(x => new ExibeLocalizacaoModel()
                    {
                        id = x.idCorrida,
                        geoPosicao = x.geoPosicao,
                        psCorrida = true
                    }).ToArray();
                    break;
                case DashboardTipoDisponivelEnum.ChamadoAguardandoMotociclista:
                    localizacao = resumoServico.BuscaLocalizacaoCorridas(StatusCorrida.AguardandoAtendimento, TipoProfissional.Motociclista)?.Select(x => new ExibeLocalizacaoModel()
                    {
                        id = x.idCorrida,
                        geoPosicao = x.geoPosicao,
                        psCorrida = true
                    }).ToArray();
                    break;
            }

            // Return
            return Json(localizacao, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult BuscaResumoSituacaoChamados()
        {
            // Recupera dados
            var entidade = resumoServico.BuscaResumoChamadosSituacao();

            // Return
            return Json(entidade, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult BuscaResumoAtendimentos()
        {
            // Recupera dados
            var entidade = resumoServico.BuscaResumoAtendimentosSituacao();

            // Return
            return Json(entidade, JsonRequestBehavior.AllowGet);
        }

        [ValidacaoUsuarioAttribute()]
        public ActionResult OSCliente(ResumoModel model)
        {
            // Consiste Entrada
            if (model == null)
                return null;

            // chamado pesquisado
            if (!long.TryParse(model.PesquisaOSCliente, out long osCliente))
                RedirectToAction("Index", "Home");

            // Busca Dados da OS
            var entidade = corridaServico.BuscaDetalheCorrida(osCliente);

            // Return
            return View(new OSClienteModel()
            {
                Cliente = entidade.IDCliente,
                Motorista = entidade.IDProfissional,
                NumeroOS = entidade.NumeroOS,
                Origem = new OSClienteEnderecoModel()
                {
                    EnderecoOrigem = entidade.Origem.EnderecoCompleto,
                    Espera = entidade.Origem.TempoEspera?.ToString(),
                    Observacao = entidade.Origem.Observacao,
                    ProcurarPessoa = entidade.Origem.ProcurarPor,
                    Realizar = entidade.Origem.Realizar,
                    Status = entidade.Origem.StatusCorrida.RetornaDescricaoEnum(typeof(StatusCorrida)),
                    Foto = System.IO.File.ReadAllBytes(@"C:\Users\LEOZI\Pictures\ctr.jpg")
                },
                Desinos = entidade.Destinos.Select(dest => new OSClienteEnderecoModel()
                {
                    EnderecoOrigem = dest.EnderecoCompleto,
                    Espera = dest.TempoEspera?.ToString(),
                    Observacao = dest.Observacao,
                    ProcurarPessoa = dest.ProcurarPor,
                    Realizar = dest.Realizar,
                    Status = dest.StatusCorrida.RetornaDescricaoEnum(typeof(StatusCorrida)),
                    Foto = System.IO.File.ReadAllBytes(@"C:\Users\LEOZI\Pictures\ctr.jpg")
                }).ToArray()
            });
        }

        [ValidacaoUsuarioAttribute()]
        public ActionResult CadastroDiariaAvulsa()
        {
            var ListaClientes = new System.Collections.Generic.Dictionary<long, string>();
            ListaClientes.Add(1, "teste");

            return View(new DiariaModel()
            {
                ListaClientes = new SelectListItem[]
                 {
                   new  SelectListItem
                   {
                       Value = "1",
                       Text = "Cliente XPTO"
                   }
                 },
                ListaProfissionais = new SelectListItem[]
                 {
                   new  SelectListItem
                   {
                       Value = "1",
                       Text = "Profissional XPTO"
                   }
                 },
                Observacao = null
            });
        }

        [HttpPost]
        [ValidacaoUsuarioAttribute()]
        public ActionResult CadastroDiariaAvulsa(DiariaModel modelo)
        {
            if (ModelState.IsValid)
            {
                ViewBag.MsgCustomAlerta = "Sucesso";
                return View(new DiariaModel()
                {
                    ListaClientes = new SelectListItem[]
                 {
                   new  SelectListItem
                   {
                       Value = "1",
                       Text = "Cliente XPTO"
                   }
                 },
                    ListaProfissionais = new SelectListItem[]
                 {
                   new  SelectListItem
                   {
                       Value = "1",
                       Text = "Profissional XPTO"
                   }
                 },
                    Observacao = null
                });
            }

            return RedirectToAction("CadastroDiariaAvulsa");
        }

        private string MontaDescricaoProfissional(int id, string nomeMotorista, TipoProfissional tipo)
        {
            return $"<b>ID:</b> {id} <br/><b>Nome:</b> {nomeMotorista}</br><b>Tipo:</b> {tipo.RetornaDescricaoEnum(typeof(TipoProfissional))}";
        }

    }
}
