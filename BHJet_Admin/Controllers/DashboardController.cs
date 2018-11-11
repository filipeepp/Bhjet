using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Dashboard;
using BHJet_Core.Enum;
using BHJet_Core.Extension;
using BHJet_Servico.Dashboard;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IResumoServico resumoServico;

        public DashboardController(IResumoServico _resumoServico)
        {
            resumoServico = _resumoServico;
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public ActionResult ExibeLocalizacao(DashboardTipoDisponivelEnum? tipoSolicitacao)
        {
            // Consiste Entrada
            if (tipoSolicitacao == null)
                return null;

            // Titulo
            ViewBag.TituloSolicitacao = tipoSolicitacao.Value.ToString().UpperCaseSeparete();

            return View();
        }

        [ValidacaoUsuarioAttribute()]
        public ActionResult ExibeLocalizacao(DashboardTipoDisponivelEnum? tipoSolicitacao, ResumoModel model)
        {
            // Consiste Entrada
            if (tipoSolicitacao == null)
                return null;

            // Busca Localizacao
            switch (tipoSolicitacao)
            {
                case DashboardTipoDisponivelEnum.FuncionarioDisponivel:
                    break;
            }

            // Titulo
            ViewBag.TituloSolicitacao = tipoSolicitacao.Value.ToString().UpperCaseSeparete();

            return View();
        }

        [HttpGet]
        [ValidacaoUsuarioAttribute()]
        public JsonResult BuscaLocalizacao(DashboardTipoDisponivelEnum? tipoSolicitacao)
        {
            tipoSolicitacao = DashboardTipoDisponivelEnum.ChamadoAguardandoCarros;

            // Consiste Entrada
            if (tipoSolicitacao == null)
                return null;
            
            // Modelo
            ExibeLocalizacaoModel[] localizacao = new ExibeLocalizacaoModel[] { };

            // Busca Localizacao
            switch (tipoSolicitacao)
            {
                case DashboardTipoDisponivelEnum.MotociclistaDisponivel:
                    localizacao = resumoServico.BuscaLocalizacaoProfissionais(TipoProfissional.Motociclista)?.Select(x => new ExibeLocalizacaoModel()
                    {
                        id = x.idColaboradorEmpresaSistema,
                        geoPosicao = x.geoPosicao
                    }).ToArray();
                    break;
                case DashboardTipoDisponivelEnum.MotoristaDisponivel:
                    localizacao = resumoServico.BuscaLocalizacaoProfissionais(TipoProfissional.Motorista)?.Select(x => new ExibeLocalizacaoModel()
                    {
                        id = x.idColaboradorEmpresaSistema,
                        geoPosicao = x.geoPosicao
                    }).ToArray();
                    break;
                case DashboardTipoDisponivelEnum.ChamadoAguardandoCarros:
                    localizacao = resumoServico.BuscaLocalizacaoCorridas(StatusCorrida.AguardandoAtendimento, TipoProfissional.Motorista)?.Select(x => new ExibeLocalizacaoModel()
                    {
                        id = x.idCorrida,
                        geoPosicao = x.geoPosicao
                    }).ToArray();
                    break;
                case DashboardTipoDisponivelEnum.ChamadoAguardandoMotociclista:
                    localizacao = resumoServico.BuscaLocalizacaoCorridas(StatusCorrida.AguardandoAtendimento, TipoProfissional.Motociclista)?.Select(x => new ExibeLocalizacaoModel()
                    {
                        id = x.idCorrida,
                        geoPosicao = x.geoPosicao
                    }).ToArray();
                    break;
            }

            // Return
            return Json(localizacao, JsonRequestBehavior.AllowGet);
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
