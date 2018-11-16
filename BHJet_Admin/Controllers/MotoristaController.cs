using BHJet_Admin.Infra;
using BHJet_Admin.Models.Motorista;
using BHJet_DTO.Profissional;
using BHJet_Servico.Profissional;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class MotoristaController : Controller
    {
        private readonly IProfissionalServico profissionalServico;

        public MotoristaController(IProfissionalServico _profissional)
        {
            _profissional = profissionalServico;
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
            if (ID != null)
            {
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
                    Endereco = profissional.EnderecoCompleto,
                    Observacao = profissional.Observacao,
                    TelefoneCelular = profissional.TelefoneCelular,
                    TelefoneResidencial = profissional.TelefoneResidencial,
                    TipoCarteiraMotorista = profissional.TipoCNH,
                    EdicaoCadastro = true
                });
            }
            else
            {
                // Tipo de Execução
                ViewBag.TipoAlteracao = "Novo";

                ModelState.Clear();

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
            // Atualiza dados do profissional
            profissionalServico.AtualizaDadosProfissional(new ProfissionalCompletoModel()
            {
                NomeCompleto = model.NomeCompleto,
                Email = model.Email,
                CelularWpp = model.CelularWhatsapp,
                CPF = model.CpfCnpj,
                TelefoneResidencial = model.TelefoneResidencial,
                TelefoneCelular = model.TelefoneCelular,
                CNH = model.CNH,
                ContratoCLT = model.TipoRegimeContratacao == BHJet_Core.Enum.RegimeContratacao.CLT ? true : false,
                Observacao = model.Observacao,
                EnderecoCompleto = model.Endereco,
                ID = model.ID,
                TipoCNH = model.TipoCarteiraMotorista,
                TipoRegime = model.TipoRegimeContratacao
            });

            // Return
            return View(model);
        }
        #endregion

        #region Listar Motoristas
        [ValidacaoUsuarioAttribute()]
        public ActionResult Listar(string palavraChave)
        {
            // Busca Motoristas
            var entidade = profissionalServico.BuscaProfissionais();

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
        #endregion

    }
}