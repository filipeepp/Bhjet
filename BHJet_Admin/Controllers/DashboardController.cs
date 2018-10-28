using BHJet_Admin.Models.Dashboard;
using BHJet_Core.Enum;
using BHJet_Core.Extension;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class DashboardController : Controller
    {

        [HttpGet]
        public ActionResult ChamadoAvulsoEspera(DashboardTipoDisponivelEnum? tipoSolicitacao)
        {
            // Consiste Entrada
            if (tipoSolicitacao == null)
                return null;

            // Titulo
            ViewBag.TituloSolicitacao = tipoSolicitacao.Value.ToString().UpperCaseSeparete();

            return View();
        }

        public ActionResult ChamadoAvulsoEspera(DashboardTipoDisponivelEnum? tipoSolicitacao, long? idMotociclista)
        {
            // Consiste Entrada
            if (tipoSolicitacao == null)
                return null;

            // Titulo
            ViewBag.TituloSolicitacao = tipoSolicitacao.Value.ToString().UpperCaseSeparete();

            return View();
        }

        public ActionResult OSCliente(string NumeroOS)
        {
            // Consiste Entrada
            if (NumeroOS == null)
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

        public ActionResult CadastroDiariaAvulsa()
        {
            return View();
        }


    }
}
