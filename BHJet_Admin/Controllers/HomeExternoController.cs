﻿using BHJet_Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class HomeExternoController : Controller
    {
        #region Entregas Origem
        public ActionResult Index()
        {
            var origem = new EntregaModel
            {
                Enderecos = new List<EnderecoModel>()
                 {
                     new EnderecoModel()
                     {
                     }
                 }
            };

            if (TempData["origemSolicitacao"] != null)
                origem = (EntregaModel)TempData["origemSolicitacao"];

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
                model.Enderecos.Add(new EnderecoModel()
                {

                });

                // Model
                this.TempData["origemSolicitacao"] = model;

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