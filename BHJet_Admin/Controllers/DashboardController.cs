using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Dashboard;
using BHJet_Enumeradores;
using BHJet_Core.Extension;
using BHJet_DTO.Diaria;
using BHJet_Servico.Cliente;
using BHJet_Servico.Corrida;
using BHJet_Servico.Dashboard;
using BHJet_Servico.Diaria;
using BHJet_Servico.Profissional;
using BHJet_Servico.Tarifa;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IResumoServico resumoServico;
        private readonly ICorridaServico corridaServico;
        private readonly IProfissionalServico profissionalServico;
        private readonly IDiariaServico diariaServico;
        private readonly IClienteServico clienteServico;
        private readonly ITarifaServico tarifaServico;

        public DashboardController(IResumoServico _resumoServico, ICorridaServico _corridaServico,
            IProfissionalServico _profServico, IDiariaServico _DiariaServico, IClienteServico _clienteServico, ITarifaServico _tarifaServico)
        {
            resumoServico = _resumoServico;
            corridaServico = _corridaServico;
            profissionalServico = _profServico;
            diariaServico = _DiariaServico;
            clienteServico = _clienteServico;
            tarifaServico = _tarifaServico;
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
            // Modelo
            ExibeLocalizacaoModel[] localizacao = new ExibeLocalizacaoModel[] { };

            try
            {
                // Consiste Entrada
                if (TempData["TipoSolicitacao"] == null)
                    return null;

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
            catch (Exception e)
            {
                return Json(localizacao, JsonRequestBehavior.AllowGet);
            }
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
            try
            {
                // Consiste Entrada
                if (model == null)
                    return null;

                // chamado pesquisado
                if (!long.TryParse(model.PesquisaOSCliente, out long osCliente))
                    return RedirectToAction("Index", "Home");

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
                        Foto = entidade.Origem.CaminhoProtocolo
                    },
                    Desinos = entidade.Destinos.Select(dest => new OSClienteEnderecoModel()
                    {
                        EnderecoOrigem = dest.EnderecoCompleto,
                        Espera = dest.TempoEspera?.ToString(),
                        Observacao = dest.Observacao,
                        ProcurarPessoa = dest.ProcurarPor,
                        Realizar = dest.Realizar,
                        Status = dest.StatusCorrida.RetornaDescricaoEnum(typeof(StatusCorrida)),
                        Foto = dest.CaminhoProtocolo
                    }).ToArray()
                });
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                return RedirectToAction("Index", "Home");
            }
        }

        #region Diaria Avulsa
        [ValidacaoUsuarioAttribute()]
        public ActionResult CadastroDiariaAvulsa()
        {
            return View(new DiariaModel()
            {
                Observacao = null,
            });
        }

        [HttpPost]
        [ValidacaoUsuarioAttribute()]
        public ActionResult CadastroDiariaAvulsa(DiariaModel modelo)
        {
            try
            {
                if (modelo.HorarioInicial == null)
                {
                    ModelState.AddModelError("", "Hora inicio do expediente.");
                    return View(modelo);
                }
                else if (modelo.HorarioFim == null)
                {
                    ModelState.AddModelError("", "Hora Fim do expediente.");
                    return View(modelo);
                }
                else if (modelo.HorarioFim < modelo.HorarioFim)
                {
                    ModelState.AddModelError("", "Hora Fim do expediente não pode ser menor que o horario inicial.");
                    return View(modelo);
                }

                if (modelo.PeriodoFinal.ToString().ToDate() < modelo.PeriodoInicial.ToString().ToDate())
                {
                    ModelState.AddModelError("", "A data de expediente final deve ser maior que a inicial.");
                    return View(modelo);
                }
                else if (modelo.ClienteSelecionado == null)
                {
                    ModelState.AddModelError("", "Favor selecionar um cliente na lista.");
                    return View(modelo);
                }

                if (ModelState.IsValid)
                {
                    // Incluir diaria
                    diariaServico.IncluirDiaria(new DiariaAvulsaFiltroDTO()
                    {
                        IDCliente = modelo.ClienteSelecionado ?? 0,
                        IDColaboradorEmpresa = modelo.ProfissionalSelecionado ?? 0,
                        DataInicioExpediente = modelo.PeriodoInicial.ToDate() ?? new DateTime(),
                        DataFimExpediente = modelo.PeriodoFinal.ToDate() ?? new DateTime(),
                        HoraInicioExpediente = modelo.HorarioInicial ?? new TimeSpan(),
                        HoraFimExpediente = modelo.HorarioFim ?? new TimeSpan(),
                        ValorDiariaNegociado = modelo.ValorDiaria.ToDecimalCurrency(),
                        ValorDiariaComissaoNegociado = modelo.ValorComissao.ToDecimalCurrency(),
                        FranquiaKMDiaria = modelo.FranquiaKMDiaria.ToDecimalCurrency(),
                        ValorKMAdicionalNegociado = modelo.ValorKMAdicional.ToDecimalCurrency()
                    });

                    this.MensagemSucesso("Diaria avulsa cadastrada com sucesso.");

                    ModelState.Clear();
                    return View();
                }

                return RedirectToAction("CadastroDiariaAvulsa");
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult BuscaProfissionais(string trechoPesquisa)
        {
            // Recupera dados
            var entidade = profissionalServico.BuscaProfissionais(trechoPesquisa);

            // Return
            return Json(entidade.Select(x => new AutoCompleteModel()
            {
                label = x.ID + " - " + x.NomeCompleto,
                value = x.ID
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
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

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult BuscaTarifas(long idCliente)
        {
            // Recupera dados
            var entidade = tarifaServico.BuscaTaritaCliente(idCliente);

            // Return
            return Json(new
            {
                DecValorDiaria = entidade.decValorDiaria?.ToString("C", new CultureInfo("pt-BR")) ?? string.Empty,
                decValorKMAdicionalDiaria = entidade.decValorKMAdicionalDiaria?.ToString("C", new CultureInfo("pt-BR")) ?? string.Empty,
                decFranquiaKMDiaria = entidade.decFranquiaKMDiaria?.ToString("C", new CultureInfo("pt-BR")) ?? string.Empty
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult BuscaComissao(long idProfissional)
        {
            try
            {
                // Recupera dados
                var entidade = profissionalServico.BuscaComissaoProfissional(idProfissional);

                // Return
                return Json(new
                {
                    decPercentualComissao = entidade.decPercentualComissao.ToString("C", new CultureInfo("pt-BR")).Replace("R$ ", "") + "%" ?? string.Empty,
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    string.Empty,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private string MontaDescricaoProfissional(int id, string nomeMotorista, TipoProfissional tipo)
        {
            return $"<b>ID:</b> {id} <br/><b>Nome:</b> {nomeMotorista}</br><b>Tipo:</b> {tipo.RetornaDescricaoEnum(typeof(TipoProfissional))}";
        }
    }
}
