﻿using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_DTO.Corrida;
using BHJet_Enumeradores;
using BHJet_Servico.Area;
using BHJet_Servico.Cliente;
using BHJet_Servico.Corrida;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class EntregasController : Controller
    {
        private readonly ICorridaServico corridaServico;

        private readonly IClienteServico clienteServico;

        private readonly IAreaAtuacaoServico areaServico;

        public EntregasController(ICorridaServico _corridaServico, IClienteServico _clienteServico, IAreaAtuacaoServico _area)
        {
            corridaServico = _corridaServico;
            clienteServico = _clienteServico;
            areaServico = _area;
        }

        // Passo 1 - Destinos
        [ValidacaoCorridaAttribute(OSAvulsoPassos.Destinos)]
        public ActionResult Index()
        {
            // Variaveis
            var osAvulsa = this.RetornaOSAvulsa();

            if(osAvulsa == null)
                return RedirectToAction("~/HomeExterno/Index");

            // Return View
            return View(osAvulsa);
        }

        // Passo 1 - Destinos Conclusao
        [HttpPost]
        [ValidacaoCorridaAttribute(OSAvulsoPassos.Destinos)]
        public ActionResult Index(EntregaModel model)
        {
            // Atualiza OS
            model.PassoOS = OSAvulsoPassos.Destinos;
            this.AtualizaOSAvulsa(model);

            // Redirect
            return RedirectToAction("Resumo", "Entregas");
        }

        // Passo 2 - Resumo
        [ValidacaoCorridaAttribute(OSAvulsoPassos.Conclusao)]
        public ActionResult Resumo()
        {
            // Busca Solicitação
            var osAvulsa = this.RetornaOSAvulsa();

            // Busca Precificação
            var resumo = corridaServico.CalculoPrecoCorrida(new BHJet_DTO.Corrida.CalculoCorridaDTO()
            {
                IDCliente = osAvulsa.IDCliente ?? 0,
                TipoVeiculo = (int)osAvulsa.TipoProfissional,
                Localizacao = osAvulsa.Enderecos.Select(l => new CalculoCorridaLocalidadeDTO()
                {
                    Longitude = Double.Parse(l.Longitude.Replace(".", ",")),
                    Latitude = Double.Parse(l.Latitude.Replace(".", ","))
                }).ToArray()
            });
            osAvulsa.ValorCorrida = resumo.Preco;
            //osAvulsa.QtdKM = 0;

            // Busca Dados de Pagamento
            try
            {
                // Busca
                var dadosBd = clienteServico.BuscaDadosBancariosCliente(osAvulsa.IDCliente ?? 0);

                // Dados
                osAvulsa.DadosPagamento = new PagamentoModel()
                {
                    NomeCartaoCredito = dadosBd.NomeCartaoCredito,
                    NumeroCartaoCredito = dadosBd.NumeroCartaoCredito,
                    Validade = dadosBd.Validade
                };
            }
            catch
            {
                osAvulsa.DadosPagamento = new PagamentoModel()
                {
                    Validade = "/"
                };
            }

            // Atualiza OS
            this.AtualizaOSAvulsa(osAvulsa);

            // Return
            return View(osAvulsa);
        }

        // Passo 2 - Resumo Conclusao
        [HttpPost]
        [ValidacaoCorridaAttribute(OSAvulsoPassos.Conclusao)]
        public ActionResult Resumo(EntregaModel model)
        {
            // Busca Solicitação
            var osAvulsa = this.RetornaOSAvulsa();

            // Se Logado redireciona para pagamento
            if (string.IsNullOrWhiteSpace(UsuarioLogado.Instance.bhTkUsu) || osAvulsa.IDCliente == null)
            {
                osAvulsa.SimulandoCorridaSemUsuario = true;
                TempData["Error"] = "Favor realizar o login antes de continuar a solicitação do serviço.";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                // Incluir Corrida
                osAvulsa.OSNumero = corridaServico.IncluirCorrida(new IncluirCorridaDTO()
                {
                    IDCliente = osAvulsa.IDCliente,
                    TipoProfissional = (int)osAvulsa.TipoProfissional,
                    IDProfissional = osAvulsa.ProfissionalSelecionado,
                    Enderecos = osAvulsa.Enderecos.Select(c => new EnderecoCorridaDTO()
                    {
                        Descricao = c.Descricao,
                        Latitude = c.Latitude,
                        Longitude = c.Longitude,
                        Observacao = c.Observacao,
                        ProcurarPessoa = c.ProcurarPessoa,
                        TipoOcorrencia = c.TipoOcorrencia
                    }).ToList()
                });

                // Atualiza OS
                this.AtualizaOSAvulsa(osAvulsa);

                // Redirect
                return RedirectToAction("Resumo", "Entregas");
            }
        }

        #region Ajax Metodos
        // Adicionar Endereco
        [HttpPost]
        public JsonResult Finaliza(EntregaModel model)
        {
            model.Enderecos.Add(new EnderecoModel()
            {

            });

            this.AtualizaOSAvulsa(model);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        // Busca Tipo de Veiculo
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

        [HttpGet]
        public JsonResult BuscaAreas()
        {
            try
            {
                // Busca area atuação
                var areas = areaServico.BuscaAreaAtuacao();

                // Serializa
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(areas);

                // Return
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}