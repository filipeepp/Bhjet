using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class HomeExternoController : Controller
    {
        public HomeExternoController()
        {
            
        }

        #region Entregas Origem
        public ActionResult Index(long? idCliente = null)
        {
            // Retorna Controle
            var controleOS =  this.RetornaOSAvulsa();

            // Cria Controle de OS Avulsa
            var origem = this.CriaOSAvulsa(controleOS != null ? controleOS.IDCliente : null);

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
    }
}