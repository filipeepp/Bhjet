using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Dashboard;
using BHJet_Core.Enum;
using BHJet_Core.Extension;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public ActionResult ChamadoAvulsoEspera(DashboardTipoDisponivelEnum? tipoSolicitacao)
        {
            // Consiste Entrada
            if (tipoSolicitacao == null)
                return null;

            // Titulo
            ViewBag.TituloSolicitacao = tipoSolicitacao.Value.ToString().UpperCaseSeparete();

            return View();
        }

        [ValidacaoUsuarioAttribute()]
        public ActionResult ChamadoAvulsoEspera(DashboardTipoDisponivelEnum? tipoSolicitacao, ResumoModel model)
        {
            // Consiste Entrada
            if (tipoSolicitacao == null)
                return null;

            // Titulo
            ViewBag.TituloSolicitacao = tipoSolicitacao.Value.ToString().UpperCaseSeparete();

            return View();
        }

        [ValidacaoUsuarioAttribute()]
        public ActionResult OSCliente(ResumoModel model)
        {
            // Consiste Entrada
            if (model == null)
                return null;

            return View(new OSClienteModel()
            {
                Cliente = 1123,
                Motorista = 444,
                NumeroOS = 999,
                Origem = new OSClienteEnderecoModel()
                {
                    EnderecoOrigem = "teste des",
                    Espera = "12",
                    Observacao = "observacao",
                    ProcurarPessoa = "Thiago",
                    Realizar = "Pegar o diogo na porrada",
                    Status = "Status desc"
                },
                Desinos = new OSClienteEnderecoModel[]
                 {
                     new OSClienteEnderecoModel()
                     {
                          EnderecoOrigem = "teste des",
                    Espera = "12",
                    Observacao = "observacao",
                    ProcurarPessoa = "Thiago",
                    Realizar = "Pegar o diogo na porrada",
                    Status = "Status desc"
                     },
                       new OSClienteEnderecoModel()
                     {
                          EnderecoOrigem = "teste des 2",
                    Espera = "12",
                    Observacao = "observacao 2",
                    ProcurarPessoa = "Pedro 2",
                    Realizar = "Pegar o diogo na porrada",
                    Status = "Status desc"
                     }
                 }

            });
        }

        [ValidacaoUsuarioAttribute()]
        public ActionResult CadastroDiariaAvulsa()
        {
            var ListaClientes = new System.Collections.Generic.Dictionary<long, string>();
            ListaClientes.Add(1, "teste");

            return View(new DiariaModel()
            {
                ListaClientes = new SelectListItem[]
                 {
                   new  SelectListItem
                   {
                       Value = "1",
                       Text = "Cliente XPTO"
                   }
                 },
                ListaProfissionais = new SelectListItem[]
                 {
                   new  SelectListItem
                   {
                       Value = "1",
                       Text = "Profissional XPTO"
                   }
                 },
                Observacao = null
            });
        }

        [HttpPost]
        [ValidacaoUsuarioAttribute()]
        public ActionResult CadastroDiariaAvulsa(DiariaModel modelo)
        {
            if (ModelState.IsValid)
            {
                ViewBag.MsgCustomAlerta = "Sucesso";
                return View(new DiariaModel()
                {
                    ListaClientes = new SelectListItem[]
                 {
                   new  SelectListItem
                   {
                       Value = "1",
                       Text = "Cliente XPTO"
                   }
                 },
                    ListaProfissionais = new SelectListItem[]
                 {
                   new  SelectListItem
                   {
                       Value = "1",
                       Text = "Profissional XPTO"
                   }
                 },
                    Observacao = null
                });
            }

            return RedirectToAction("CadastroDiariaAvulsa");
        }

    }
}
