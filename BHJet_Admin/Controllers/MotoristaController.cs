using BHJet_Admin.Infra;
using BHJet_Admin.Models.Motorista;
using BHJet_DTO.Profissional;
using BHJet_Servico.Profissional;
using System;
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

        [ValidacaoUsuarioAttribute()]
        public ActionResult Index()
        {
            return View();
        }

        #region Novo/Edicao Motorista
        [ValidacaoUsuarioAttribute()]
        public ActionResult Novo(bool? Edicao, long? ID, NovoMotoristaModel model)
        {
            try
            {
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
                        EdicaoCadastro = true
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
                };

                // Alteração
                if (model.EdicaoCadastro)
                    profissionalServico.AtualizaDadosProfissional(entidade); // Atualiza dados do profissional
                else
                    profissionalServico.IncluirProfissional(entidade); // Atualiza dados do profissional

                this.TrataSucesso("Profissional atualizado com sucesso.");

                // Return
                return View(model);
            }
            catch(Exception e)
            {
                this.TrataErro(e);
                return View(model);
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