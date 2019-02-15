using BHJet_Admin.Infra;
using BHJet_DTO.Tarifa;
using BHJet_Servico.Tarifa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class TarifarioController : Controller
    {
		private readonly ITarifaServico tarifaServico;

		public TarifarioController(ITarifaServico _tarifa)
		{
			tarifaServico = _tarifa;
		}

		// GET: Tarifario
		public ActionResult Index()
        {
            return View();
        }

		[HttpGet]
		[ValidacaoUsuarioAttribute()]
		public JsonResult BuscarTarifarioPadraoAtivo(int codigoTipoVeiculo)
		{
			var data = tarifaServico.BuscaTarifaAtiva(codigoTipoVeiculo);

			var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

			return Json(json, JsonRequestBehavior.AllowGet);

		}
	}
}