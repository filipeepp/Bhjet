using BHJet_Admin.Infra;
using BHJet_Admin.Models.Motorista;
using BHJet_Core.Extension;
using BHJet_DTO.Profissional;
using BHJet_Servico.Profissional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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

        [ValidacaoUsuarioAttribute()]
        public ActionResult Index()
        {
            return View();
        }

        #region Novo/Edicao Motorista
        [ValidacaoUsuarioAttribute()]
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
                        Comissao = profissional.Comissoes != null ? profissional.Comissoes.Select(c => new NovoMotoristaComissaoModel()
                        {
                            ID = c.ID,
                            ValorComissao = c.decPercentualComissao.ToString(),
                            VigenciaInicio = c.dtDataInicioVigencia,
                            VigenciaFim = c.dtDataFimVigencia,
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
                        Comissao = new NovoMotoristaComissaoModel[]
                          {
                               new NovoMotoristaComissaoModel()
                               {

                               }
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
        [ValidacaoUsuarioAttribute()]
        public ActionResult Novo(NovoMotoristaModel model)
        {
            try
            {
                // Validacoes
                if (model.Comissao.Any())
                {
                    model.Comissao.All(x =>
                    {
                        if (x.VigenciaInicio < DateTime.Now.Date)
                            throw new Exception($"A comissão {x.ID} está com a data de vigência inicial menor que a data atual, favor atualizar.");
                        else if (x.VigenciaFim <= DateTime.Now.Date || x.VigenciaFim < x.VigenciaInicio)
                            throw new Exception($"A comissão {x.ID} está com a data de vigência final menor que a data atual ou que a vigência inicial, favor atualizar.");
                        return true;
                    });
                }

                // Modelo entidade
                var entidade = new ProfissionalCompletoModel()
                {
                    ID = model.ID,
                    NomeCompleto = model.NomeCompleto,
                    Email = model.Email,
                    CelularWpp = model.CelularWhatsapp,
                    CPF = model.CpfCnpj,
                    TelefoneResidencial = model.TelefoneResidencial,
                    TelefoneCelular = model.TelefoneCelular,
                    CNH = model.CNH,
                    ContratoCLT = model.TipoRegimeContratacao == BHJet_Core.Enum.RegimeContratacao.CLT ? true : false,
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
                    Comissoes = model.Comissao.Any() ? model.Comissao.Select(x => new ProfissionalComissaoModel()
                    {
                        ID = x.ID,
                        decPercentualComissao = x.ValorComissao.ToDecimalCurrency(),
                        dtDataFimVigencia = x.VigenciaFim ?? new DateTime(),
                        dtDataInicioVigencia = x.VigenciaInicio ?? new DateTime()
                    }).ToArray() : new ProfissionalComissaoModel[] { }
                };

                // Alteração
                if (model.EdicaoCadastro)
                    profissionalServico.AtualizaDadosProfissional(entidade); // Atualiza dados do profissional
                else
                    profissionalServico.IncluirProfissional(entidade); // Atualiza dados do profissional

                this.MensagemSucesso("Profissional atualizado com sucesso.");

                // Return
                return View(model);
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


        [ValidacaoUsuarioAttribute()]
        [HttpPost]
        public JsonResult AddComissao(NovoMotoristaModel data)
        {
            try
            {
                // Validação
                if (data.Comissao != null && data.Comissao.Where(c => c.ValorComissao == null || c.VigenciaFim == null || c.VigenciaInicio == null).Any())
                    throw new Exception("Favor preencher todos os campos da comissão anterior antes de incluir uma nova.");

                var listaNovaComissao = data.Comissao?.ToList() ?? new List<NovoMotoristaComissaoModel>();
                listaNovaComissao.Add(new NovoMotoristaComissaoModel());
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

        [ValidacaoUsuarioAttribute()]
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
        [ValidacaoUsuarioAttribute()]
        public ActionResult Listar(string palavraChave)
        {
            try
            {
                // Busca Motoristas
                var entidade = profissionalServico.BuscaProfissionais(palavraChave);

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

    }
}