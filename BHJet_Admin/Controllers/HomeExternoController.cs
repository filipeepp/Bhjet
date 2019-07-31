using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Enumeradores;
using BHJet_Servico.Area;
using BHJet_Servico.Profissional;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class HomeExternoController : Controller
    {
        private readonly IProfissionalServico profissionalServico;
        private readonly IAreaAtuacaoServico areaServico;

        public HomeExternoController(IProfissionalServico _profServico, IAreaAtuacaoServico _area)
        {
            profissionalServico = _profServico;
            areaServico = _area;
        }

        #region Entregas Origem
        public ActionResult Index(long? idCliente = null)
        {
            // Retorna Controle
            var controleOS =  this.RetornaOSAvulsa();

            // Cria Controle de OS Avulsa
            var origem = controleOS != null ? controleOS : this.CriaOSAvulsa(controleOS != null ? controleOS.IDCliente : idCliente);

            if(controleOS != null && idCliente != null && origem.IDCliente == null)
                origem.IDCliente = idCliente;

            // Return View
            return View(origem);
        }

        [HttpPost]
        public ActionResult Index(EntregaModel model)
        {
            try
            {
                // Validacoes
                if (model.Enderecos == null || !model.Enderecos.Any())
                    throw new Exception("Favor preencher todos os campos da solicitação.");
                else
                {
                    if (model.Enderecos.Any() && (string.IsNullOrWhiteSpace(model.Enderecos.First().Latitude) || string.IsNullOrWhiteSpace(model.Enderecos.First().Latitude)))
                        throw new Exception("Favor pesquisar o endereço e clicar na localização desejada na lista.");
                }

                // Adiciona destino
                this.FinalizaOrigem(model);

                // Redirect
                return RedirectToAction("Index", "Entregas");
            }
            catch (Exception e)
            {
                this.TrataErro(e);
                return View(model);
            }
        }
        #endregion

        [HttpGet]
        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador, TipoUsuario.FuncionarioCliente)]
        public JsonResult BuscaProfissionais(string trechoPesquisa, int? tipoProfissional)
        {
            // Recupera dados
            var entidade = profissionalServico.BuscaProfissionaisDisponiveis(trechoPesquisa, tipoProfissional != null ? (TipoProfissional)tipoProfissional.Value : (TipoProfissional?)null);

            // Return
            return Json(entidade.Select(x => new AutoCompleteModel()
            {
                label = x.ID + " - " + x.NomeCompleto?.ToUpper(),
                value = x.ID
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscaAreas()
        {
            try
            {
                // Busca area atuação
                var areas = areaServico.BuscaAreaAtuacao();

                // Serializa
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(areas);

                // Return
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}