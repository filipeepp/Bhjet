using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Faturamento;
using BHJet_DTO.Faturamento;
using BHJet_Enumeradores;
using BHJet_Servico.Cliente;
using BHJet_Servico.Faturamento;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class FaturamentoController : Controller
    {
        private readonly IClienteServico clienteServico;
        private readonly IFaturamentoServico faturamentoServico;

        public FaturamentoController(IClienteServico _clienteServico, IFaturamentoServico _faturamentoServico)
        {
            clienteServico = _clienteServico;
            faturamentoServico = _faturamentoServico;
        }

        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult Index()
        {
            return View();
        }

        #region Gerar Faturamento
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult GerarFaturamento()
        {
            return View(new GerarFaturamantoModel());
        }

        [HttpPost]
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult GerarFaturamento(GerarFaturamantoModel model)
        {
            try
            {
                // Datas
                DateTime inicio = new DateTime(model.AnoSelecionado, model.MesSelecionado, 1);
                DateTime fim = new DateTime(model.AnoSelecionado, model.MesSelecionado, DateTime.DaysInMonth(model.AnoSelecionado, model.MesSelecionado));

                // Gera Faturamento
                var faturamentos = faturamentoServico.GerarFaturamento(new BHJet_DTO.Faturamento.GerarFaturamentoDTO()
                {
                    IdCliente = model.ClienteSelecionado,
                    DataInicioFaturamento = inicio,
                    DataFimFaturamento = fim
                });

                this.MensagemSucesso("Faturamento gerado com sucesso.");

                // Return View
                return View(new GerarFaturamantoModel()
                {
                    ClienteSelecionado = model.ClienteSelecionado,
                    ListaFaturamento = faturamentos.Select(x => new FaturamentoModel()
                    {
                        ID = x.ID,
                        IDCliente = x.IDCliente,
                        Cliente = x.NomeCliente,
                        Apuracao = x.Periodo,
                        DescContrato = x.TipoContrato,
                        Valor = x.Valor.ToString("C", new CultureInfo("pt-BR"))
                    })
                });
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                return View(model);
            }
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public JsonResult BuscaClientes(string trechoPesquisa)
        {
            // Recupera dados
            var entidade = clienteServico.BuscaListaClientes(trechoPesquisa);

            // Return
            return Json(entidade.Select(x => new AutoCompleteModel()
            {
                label = x.ID + " - " + x.vcNomeFantasia,
                value = x.ID
            }), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Faturamento Normal
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult FaturamentoNormal()
        {
            return View(new FaturamentoNormal()
            {
                ListaFaturamento = null
            });
        }

        [HttpPost]
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult FaturamentoNormal(FaturamentoNormal model)
        {
            try
            {
                // Datas
                DateTime inicio = new DateTime(model.AnoSelecionado, model.MesSelecionado, 1);
                DateTime fim = new DateTime(model.AnoSelecionado, model.MesSelecionado, DateTime.DaysInMonth(model.AnoSelecionado, model.MesSelecionado));

                // Gera Faturamento
                var faturamentos = faturamentoServico.GetFaturamentoNormal(new BHJet_DTO.Faturamento.ConsultarFaturamentoDTO()
                {
                    DataInicioFaturamentoFiltro = inicio,
                    DataFimFaturamentoFiltro = fim,
                    IdClienteFiltro = model.ClienteSelecionado,
                    TipoContratoFiltro = model.TipoContratoSelecionado
                });

                // Popula retorno
                model.ListaFaturamento = faturamentos.Select(fat => new FaturamentoModel()
                {
                    ID = fat.ID,
                    IDCliente = fat.IDCliente,
                    Cliente = fat.NomeCliente,
                    Apuracao = fat.Periodo,
                    DescContrato = fat.TipoContrato,
                    Valor = fat.Valor.ToString("C", new CultureInfo("pt-BR"))
                });

                return View(model);
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                model.ListaFaturamento = null;
                return View(model);
            }
        }
        #endregion

        #region Faturamento Avulso
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
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
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
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
                      Apuracao = "",
                       DescContrato = "Avulso",
                       Valor = ""
                    }
                 }
            });
        }
        #endregion

        #region Detalhe Faturamento
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
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
                                         Tipo = TipoProfissional.Motociclista,
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

        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador, TipoUsuario.FuncionarioCliente)]
        public ActionResult DetalheFaturamentoAvulso(long idCliente, string periodo)
        {
            try
            {
                // Variaveis
                ItemFaturamentoDetalheDTO[] resultado = new ItemFaturamentoDetalheDTO[] { };
                string Periodo = string.Empty;
                // Execute
                if (string.IsNullOrWhiteSpace(periodo))
                {
                    resultado = faturamentoServico.GetFaturamentoDetalhe(idCliente);
                    Periodo = " TODOS";
                }
                else
                {
                    // Datas pesquisa
                    var datIni = DateTime.Parse(periodo.Split(' ')[0].TrimStart().TrimEnd());
                    var datFim = DateTime.Parse(periodo.Split(' ')[2].TrimStart().TrimEnd());
                    periodo = datIni.ToShortDateString() + " a " + datFim.ToShortDateString();
                    // Busca detalhe
                    resultado = faturamentoServico.GetFaturamentoDetalhe(idCliente, datIni, datFim);
                }
                ViewBag.Total = resultado.Sum(x => x.Valor);

                // Return View
                return View(new DetalheFaturamentoAvulso()
                {
                    Cliente = resultado.FirstOrDefault().NomeCliente,
                    DataRelatorio = DateTime.Now.ToLongDateString(),
                    PeriodoIntervalo = periodo,
                    Registros = resultado.Select(c => new DetalheFaturamentoAvulsoRegistros()
                    {
                        DataCorrida = c.Data,
                        NumeroOS = c.OS,
                        QuantidadeKM = c.KM,
                        Profissional = c.Profissional,
                        Valor = c.Valor,
                        TipoContrato = c.Tipo
                    }).ToArray()
                });
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        #endregion
    }
}