using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Motorista;
using BHJet_Core.Extension;
using BHJet_CoreGlobal;
using BHJet_DTO.Profissional;
using BHJet_Enumeradores;
using BHJet_Servico.Profissional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class MotoristaController : Controller
    {
        private readonly IProfissionalServico profissionalServico;

        public MotoristaController(IProfissionalServico _profissional)
        {
            profissionalServico = _profissional;
        }

        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult Index()
        {
            return View();
        }

        #region Novo/Edicao Motorista
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult Novo(bool? Edicao, long? ID, NovoMotoristaModel model, bool? alteraComissao = false)
        {
            try
            {
                if (alteraComissao == true && TempData["ModelAtual"] != null)
                {
                    ModelState.Clear();
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<NovoMotoristaModel>(TempData["ModelAtual"].ToString());

                    return View(results);
                }
                if (ID != null)
                {
                    ModelState.Clear();

                    // Tipo de Execução
                    ViewBag.TipoAlteracao = "Editar";

                    // Busca dados do profissional
                    var profissional = profissionalServico.BuscaProfissional(ID ?? 0);

                    // Return
                    return View(new NovoMotoristaModel()
                    {
                        ID = profissional.ID,
                        TipoVeiculos = string.Join(",", profissional.TipoVeiculos),
                        VeiculoSelecionado = profissional.TipoVeiculos,
                        NomeCompleto = profissional.NomeCompleto,
                        TipoRegimeContratacao = profissional.TipoRegime,
                        CelularWhatsapp = profissional.CelularWpp,
                        CNH = profissional.CNH,
                        CpfCnpj = profissional.CPF,
                        Email = profissional.Email,
                        Observacao = profissional.Observacao,
                        TelefoneCelular = profissional.TelefoneCelular,
                        TelefoneResidencial = profissional.TelefoneResidencial,
                        TipoCarteiraMotorista = profissional.TipoCNH,
                        Cep = profissional.Cep,
                        Rua = profissional.Rua,
                        Bairro = profissional.Bairro,
                        Cidade = profissional.Cidade,
                        Complemento = profissional.Complemento,
                        EnderecoPrincipal = profissional.EnderecoPrincipal,
                        PontoReferencia = profissional.PontoReferencia,
                        RuaNumero = profissional.RuaNumero,
                        UF = profissional.UF,
                        EdicaoCadastro = true,
                        DocumentoRG = profissional.DocumentoRG,
                        Senha = "",
                        Situacao = profissional.Status,
                        ValorComissao = profissional.Comissoes.OrderBy(c => c.dtDataInicioVigencia).FirstOrDefault().decPercentualComissao.ToString("C"),
                        Comissao = profissional.Comissoes != null ? profissional.Comissoes.Select(c => new NovoMotoristaComissaoModel()
                        {
                            ID = c.ID,
                            ValorComissao = c.decPercentualComissao.ToString(),
                            VigenciaInicio = c.dtDataInicioVigencia.ToShortDateString(),
                            VigenciaFim = c.dtDataFimVigencia.ToShortDateString(),
                            Observacao = c.Observacao

                        }).ToArray() : new NovoMotoristaComissaoModel[] { }
                    });
                }
                else
                {

                    ModelState.Clear();

                    // Tipo de Execução
                    ViewBag.TipoAlteracao = "Novo";

                    // Return
                    return View(new NovoMotoristaModel()
                    {
                        EdicaoCadastro = false,
                        ID = 0,
                        Bairro = "",
                        Situacao = true,
                        Senha = "",
                        Email = "",
                        Comissao = new NovoMotoristaComissaoModel[]
                          {
                               MontaComissaoPadrao()
                          }
                    });
                }
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                // Return
                return View(new NovoMotoristaModel()
                {
                    EdicaoCadastro = false
                });
            }
        }

        [HttpPost]
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult Novo(NovoMotoristaModel model)
        {
            try
            {
                // Validacoes
                //if (model.Comissao != null && model.Comissao.Any())
                //{
                //    model.Comissao.All(x =>
                //    {
                //        if (!DateTime.TryParse(x.VigenciaInicio, out DateTime ini) || !DateTime.TryParse(x.VigenciaFim, out DateTime fim))
                //            throw new Exception($"Preencha datá válida para vigência da comissão.");
                //        else if (DateTime.Parse(x.VigenciaFim) <= DateTime.Now.Date || DateTime.Parse(x.VigenciaFim) < DateTime.Parse(x.VigenciaInicio))
                //            throw new Exception($"A comissão {x.ID} está com a data de vigência final menor que a data atual ou que a vigência inicial, favor atualizar.");
                //        else if (x.ValorComissao == null || x.VigenciaInicio == null || x.VigenciaFim == null)
                //            throw new Exception($"Preencha ao menos uma comissão para o profissional.");
                //        return true;
                //    });
                //}
                //else
                //{
                //    model.Comissao = new NovoMotoristaComissaoModel[] { };
                //    throw new Exception($"Preencha ao menos uma comissão para o profissional.");
                //}
                if (model.VeiculoSelecionado == null || !model.VeiculoSelecionado.Any() || (model.VeiculoSelecionado.Count() == 1 && model.VeiculoSelecionado.FirstOrDefault() == 0))
                    throw new Exception($"Preencha ao menos um tipo de veiculo que o motorista possui ou irá utilizar.");

                // Modelo entidade
                var entidade = new ProfissionalCompletoModel()
                {
                    ID = model.ID,
                    NomeCompleto = model.NomeCompleto,
                    Email = model.Email,
                    CelularWpp = model.CelularWhatsapp,
                    CPF = model.CpfCnpj,
                    TelefoneResidencial = model.TelefoneResidencial,
                    TipoVeiculos = model.VeiculoSelecionado.ToArray(),
                    TelefoneCelular = model.TelefoneCelular,
                    CNH = model.CNH,
                    ContratoCLT = model.TipoRegimeContratacao == RegimeContratacao.CLT ? true : false,
                    Observacao = model.Observacao,
                    TipoCNH = model.TipoCarteiraMotorista,
                    TipoRegime = model.TipoRegimeContratacao,
                    Cep = model.Cep,
                    Rua = model.Rua,
                    Bairro = model.Bairro,
                    Cidade = model.Cidade,
                    Complemento = model.Complemento,
                    EnderecoPrincipal = model.EnderecoPrincipal,
                    PontoReferencia = model.PontoReferencia,
                    RuaNumero = model.RuaNumero,
                    UF = model.UF,
                    DocumentoRG = model.DocumentoRG,
                    Senha = model.Senha != null ? CriptografiaUtil.Criptografa(model.Senha, "ch4v3S3m2nt3BHJ0e1tA9u4t4hu1s33r") : null,
                    Status = model.Situacao,
                    Comissoes = new ProfissionalComissaoModel[] {
                    new ProfissionalComissaoModel()
                    {
                        ID = 1,
                        Observacao = "",
                        decPercentualComissao = model.ValorComissao.ToDecimalCurrency(),
                        dtDataFimVigencia = DateTime.Now,
                        dtDataInicioVigencia = DateTime.Now.AddYears(3000)
                    }}
                };

                // Alteração
                if (model.EdicaoCadastro)
                    profissionalServico.AtualizaDadosProfissional(entidade); // Atualiza dados do profissional
                else
                {
                    if (string.IsNullOrWhiteSpace(model.Senha))
                        throw new Exception($"Na inclusão de um motorista, o campo SENHA é de preenchimento obrigatório.");
                    profissionalServico.IncluirProfissional(entidade); // Atualiza dados do profissional
                }

                this.MensagemSucesso("Profissional atualizado com sucesso.");

                // Return
                return View(new NovoMotoristaModel()
                {
                    EdicaoCadastro = false,
                    ID = 0,
                    Bairro = "",
                    Senha = "",
                    Email = "",
                    Comissao = new NovoMotoristaComissaoModel[]
                          {
                               MontaComissaoPadrao()
                          }
                });
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                return View(model);
            }
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        [HttpPost]
        public JsonResult AddComissao(NovoMotoristaModel data)
        {
            try
            {
                // Validação
                if (data.Comissao != null && data.Comissao.Where(c => c.ValorComissao == null || c.VigenciaFim == null || c.VigenciaInicio == null).Any())
                    throw new Exception("Favor preencher todos os campos da comissão anterior antes de incluir uma nova.");

                var listaNovaComissao = data.Comissao?.ToList() ?? new List<NovoMotoristaComissaoModel>();
                listaNovaComissao.Add(MontaComissaoPadrao());
                data.Comissao = listaNovaComissao.ToArray();

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                TempData["ModelAtual"] = json;

                return Json(json);
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                TempData["ModelAtual"] = json;
                return Json(json);

            }
        }

        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        [HttpPost]
        public JsonResult ExcluirComissao(NovoMotoristaModel data, int numeroComissao)
        {
            try
            {
                if (data.Comissao.Count() == 1)
                {
                    data.Comissao[0] = new NovoMotoristaComissaoModel()
                    {

                    };
                }
                else
                {
                    var teste = data.Comissao.ToList();
                    teste.RemoveAt(numeroComissao);
                    data.Comissao = teste.ToArray();
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                TempData["ModelAtual"] = json;

                return Json(json);
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                TempData["ModelAtual"] = json;
                return Json(json);

            }
        }
        #endregion

        #region Listar Motoristas
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult Listar(string palavraChave)
        {
            try
            {
                // Busca Motoristas
                var entidade = profissionalServico.BuscaProfissionais(palavraChave, null);

                // Return
                return View(new EditarMotoristaModel()
                {
                    MotoristaSelecionado = null,
                    ListaMotorista = entidade.Select(pf => new MotoristaSimplesModel()
                    {
                        ID = pf.ID,
                        NomeCompleto = pf.NomeCompleto,
                        TipoRegimeContratacao = pf.TipoRegime
                    }).ToArray()
                });
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                return RedirectToAction("Index", "Motorista");
            }
        }
        #endregion

        #region Apoio
        private NovoMotoristaComissaoModel MontaComissaoPadrao()
        {
            return new NovoMotoristaComissaoModel()
            {
                VigenciaInicio = DateTime.Now.ToShortDateString(),
                VigenciaFim = DateTime.Now.AddYears(50).ToShortDateString()
            };
        }
        #endregion

        [HttpGet]
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public JsonResult BcTpVec()
        {
            // Recupera dados
            var entidade = profissionalServico.BuscaTipoVeiculos();

            // Return
            return Json(entidade.Select(x => new AutoCompleteModel()
            {
                label = x.ID + " - " + x.Descricao,
                value = x.ID
            }), JsonRequestBehavior.AllowGet);
        }
    }
}