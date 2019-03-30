using BHJet_Admin.Infra;
using BHJet_DTO.Area;
using BHJet_Enumeradores;
using BHJet_Servico.Area;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class AtuacaoController : Controller
    {
        private readonly IAreaAtuacaoServico areaServico;

        public AtuacaoController(IAreaAtuacaoServico _area)
        {
            areaServico = _area;
        }

        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        public ActionResult Area()
        {
            return View();
        }

        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
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

        [ValidacaoUsuarioAttribute(TipoUsuario.Administrador)]
        [HttpPost]
        public JsonResult CadastraAreas(IEnumerable<AreasFiltroDTO> data)
        {
            try
            {
                // Busca area atuação
                areaServico.AtualizaAreaAtuacao(data);

                // Return
                return Json("");
            }
            catch
            {
                // Return
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}


