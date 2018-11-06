using BHJet_Admin.Infra;
using BHJet_Admin.Models.Motorista;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class MotoristaController : Controller
    {
        [ValidacaoUsuarioAttribute()]
        public ActionResult Index()
        {
            return View();
        }

        #region Novo/Edicao Motorista
        [ValidacaoUsuarioAttribute()]
        public ActionResult Novo(bool? Edicao, long? ID)
        {
            if (ID != null)
            {
                ViewBag.TipoAlteracao = "Editar";
                return View(new NovoMotoristaModel()
                {
                    NomeCompleto = "Leonardo",
                    EdicaoCadastro = true
                });
            }
            else
            {
                ViewBag.TipoAlteracao = "Novo";
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
            return View(new NovoMotoristaModel() { CelularWhatsapp = false });
        }
        #endregion

        #region Listar Motoristas
        [ValidacaoUsuarioAttribute()]
        public ActionResult Listar(string palavraChave)
        {
            var nListaMotorista = new MotoristaSimplesModel[]
                  {
                      new MotoristaSimplesModel()
                      {
                           ID = 1,
                           NomeCompleto = string.IsNullOrEmpty(palavraChave) ? "LEONARDO" : "MEU OOVO",
                           TipoRegimeContratacao = BHJet_Core.Enum.RegimeContratacao.CLT
                      }
                  };

            return View(new EditarMotoristaModel()
            {
                MotoristaSelecionado = null,
                ListaMotorista = nListaMotorista
            });
        }
        #endregion

    }
}