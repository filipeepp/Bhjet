using BHJet_Admin.Models;
using BHJet_DTO.Corrida;
using BHJet_Servico.Corrida;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class EntregasController : Controller
    {
        private readonly ICorridaServico corridaServico;

        public EntregasController(ICorridaServico _corridaServico)
        {
            corridaServico = _corridaServico;
        }

        // GET: Entregas
        public ActionResult Index(object model)
        {
            var origem = (EntregaModel)TempData["origemSolicitacao"];
            this.TempData["origemSolicitacao"] = origem;

            return View(origem);
        }

        [HttpPost]
        public ActionResult Index(EntregaModel model)
        {
            this.TempData["origemSolicitacao"] = model;

            return RedirectToAction("Resumo", "Entregas");
        }

        [HttpPost]
        public JsonResult Finaliza(EntregaModel model)
        {
            model.Enderecos.Add(new EnderecoModel()
            {

            });

            this.TempData["origemSolicitacao"] = model;

            return Json("", JsonRequestBehavior.AllowGet);
        }

        // GET: Resumo
        public ActionResult Resumo()
        {
            // Busca Solicitação
            var origem = (EntregaModel)TempData["origemSolicitacao"];
            this.TempData["origemSolicitacao"] = origem;

            // Busca Precificação
            var resumo = corridaServico.CalculoPrecoCorrida(new BHJet_DTO.Corrida.CalculoCorridaDTO()
            {
                IDCliente = origem.IDCliente ?? 0,
                TipoVeiculo = (int)origem.TipoProfissional,
                Localizacao = origem.Enderecos.Select(l => new CalculoCorridaLocalidadeDTO()
                {
                    Longitude = Double.Parse(l.Longitude),
                    Latitude = Double.Parse(l.Latitude)
                }).ToArray()
            });
            origem.ValorCorrida = resumo;

            return View(origem);
        }

        // POST: Resumo
        [HttpPost]
        public ActionResult Resumo(EntregaModel model)
        {
            var origem = (EntregaModel)TempData["origemSolicitacao"];
            this.TempData["origemSolicitacao"] = origem;

            // Se Logado redireciona para pagamento
            if (origem.IDCliente == null)
                return RedirectToAction("Pagamento", "Pagamento");
            else
            {
                // Incluir Corrida
                origem.OSNumero = corridaServico.IncluirCorrida(new IncluirCorridaDTO()
                {
                    IDCliente = origem.IDCliente,
                    TipoProfissional = (int)origem.TipoProfissional,
                    Enderecos = origem.Enderecos.Select(c => new EnderecoCorridaDTO()
                    {
                        Descricao = c.Descricao,
                        Latitude = c.Latitude,
                        Longitude = c.Longitude,
                        Observacao = c.Observacao,
                        ProcurarPessoa = c.ProcurarPessoa,
                        TipoOcorrencia = c.TipoOcorrencia
                    }).ToList()
                });
                
                // Redirect
                return RedirectToAction("Resumo", "Entregas");
            }
        }

        [HttpGet]
        public JsonResult BcTpOc()
        {
            // Recupera dados
            var entidade = corridaServico.BuscaOcorrencias();

            // Return
            return Json(entidade.Select(x => new AutoCompleteModel()
            {
                label = x.ID + " - " + x.Descricao,
                value = x.ID
            }), JsonRequestBehavior.AllowGet);
        }
    }
}