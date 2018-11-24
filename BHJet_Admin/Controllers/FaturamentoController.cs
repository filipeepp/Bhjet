using BHJet_Admin.Infra;
using BHJet_Admin.Models.Faturamento;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class FaturamentoController : Controller
    {
        [ValidacaoUsuarioAttribute()]
        public ActionResult Index()
        {
            return View();
        }

        #region Gerar Faturamento
        [ValidacaoUsuarioAttribute()]
        public ActionResult GerarFaturamento()
        {
            return View(new GerarFaturamantoModel()
            {
                ListaClientes = new System.Collections.Generic.Dictionary<int, string>()
                {

                }
            });
        }

        [HttpPost]
        [ValidacaoUsuarioAttribute()]
        public ActionResult GerarFaturamento(GerarFaturamantoModel model)
        {
            return View(new GerarFaturamantoModel()
            {
                ListaClientes = new System.Collections.Generic.Dictionary<int, string>()
                {

                },
                ClienteSelecionado = 1,
                ListaFaturamento = new System.Collections.Generic.List<FaturamentoModel>()
                 {
                    new FaturamentoModel()
                     {
                      Cliente = "Cliente A",
                      Apuração = DateTime.Now,
                       DescContrato = "Avulso",
                       Valor = 1541m
                    }
                 }
            });
        }
        #endregion

        #region Faturamento Avulso
        [ValidacaoUsuarioAttribute()]
        public ActionResult FaturamentoNormal()
        {

            var cli = new System.Collections.Generic.Dictionary<int, string>();
            cli.Add(1, "Mercado Teste");
            cli.Add(2, "Loja Teste");

            return View(new FaturamentoNormal()
            {
                ListaClientes = cli,
                ListaTipoContrato = new System.Collections.Generic.Dictionary<int, string>()
                {

                }
            });
        }

        [HttpPost]
        [ValidacaoUsuarioAttribute()]
        public ActionResult FaturamentoNormal(FaturamentoNormal model)
        {
            var cli = new System.Collections.Generic.Dictionary<int, string>();
            cli.Add(1, "Mercado Teste");
            cli.Add(2, "Loja Teste");

            return View(new FaturamentoNormal()
            {
                ListaClientes = cli,
                ListaTipoContrato = new System.Collections.Generic.Dictionary<int, string>()
                {

                },
                ListaFaturamento = new System.Collections.Generic.List<FaturamentoModel>()
                 {
                    new FaturamentoModel()
                     {
                      Cliente = "Cliente A",
                      Apuração = DateTime.Now,
                       DescContrato = "Avulso",
                       Valor = 1541m
                    }
                 }
            });
        }
        #endregion

        #region Faturamento Avulso
        [ValidacaoUsuarioAttribute()]
        public ActionResult FaturamentoAvulso()
        {

            var cli = new System.Collections.Generic.Dictionary<int, string>();
            cli.Add(1, "Mercado Teste");
            cli.Add(2, "Loja Teste");

            return View(new FaturamentoAvulsoModel()
            {
                ListaClientes = cli,
                ListaTipoContrato = new System.Collections.Generic.Dictionary<int, string>()
                {

                }
            });
        }

        [HttpPost]
        [ValidacaoUsuarioAttribute()]
        public ActionResult FaturamentoAvulso(FaturamentoAvulsoModel model)
        {
            var cli = new System.Collections.Generic.Dictionary<int, string>();
            cli.Add(1, "Mercado Teste");
            cli.Add(2, "Loja Teste");

            return View(new FaturamentoAvulsoModel()
            {
                ListaClientes = cli,
                ListaTipoContrato = new System.Collections.Generic.Dictionary<int, string>()
                {

                },
                ListaFaturamento = new System.Collections.Generic.List<FaturamentoModel>()
                 {
                    new FaturamentoModel()
                     {
                      Cliente = "Cliente A",
                      Apuração = DateTime.Now,
                       DescContrato = "Avulso",
                       Valor = 1541m
                    }
                 }
            });
        }
        #endregion

        #region Detalhe Faturamento
        [ValidacaoUsuarioAttribute()]
        public ActionResult DetalheFaturamento()
        {
            return View(new DetalheFaturamentoModel()
            {
                Cliente = "Cliente Fulano",
                Contrato = "Avulso",
                DataRelatorio = DateTime.Now,
                PeriodoIntervalo = "01/11/2018 até 30/11/2018",
                Detalhes = new DetalheCorridaFaturamentoModel[]
                {
                    new DetalheCorridaFaturamentoModel()
                    {
                         Mensageiro = "Jose da silva",
                         LogCorrida = new DetalheLogCorridaFaturamentoModel[]
                         {
                              new DetalheLogCorridaFaturamentoModel()
                              {
                                   Data = DateTime.Now.AddDays(3),
                                    InicioDiaria = new TimeSpan(8,0,0),
                                     InicioAlmoco = new TimeSpan(12,0,0),
                                     FinalAlmoco = new TimeSpan(13,0,0),
                                      FinalDiaria = new TimeSpan(17,0,0),
                                       KMRodado = 170,
                                         Tipo = BHJet_Core.Enum.TipoProfissional.Motociclista,
                                          ValorTransporte = 80.00
                              }
                         },
                          DetalheTotal = new DetalheTotalFaturamentoModel()
                          {
                               FranquiaContratada = 1,
                                KMExcedente = 2,
                                TotalDiarias = 3,
                            TotalFatura = 4,
                                TotalKm = 5,
                            ValorDiaria = 6,
                            ValorDiarias = 7,
                            ValorExcedente = 8
                          }
                    }
                },
                Total = new DetalheTotalFaturamentoModel()
                {
                    FranquiaContratada = 1,
                    KMExcedente = 2,
                    TotalDiarias = 3,
                    TotalFatura = 4,
                    TotalKm = 5,
                    ValorDiaria = 6,
                    ValorDiarias = 7,
                    ValorExcedente = 8
                }
            });
        }
        #endregion
    }
}