using BHJet_Admin.Models;
using BHJet_Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class ClientesController : Controller
    {
        // GET: Clientes
        public ActionResult Clientes()
        {
            return View(new ClientesModel()
            {
                ListModel = new List<ClienteModel>()
                 {
                    new ClienteModel()
                    {
                        ClienteID = 1,
                        NomeRazaoSocial = "Tânia Azevedo Araujo",
                        TipoContrato = TipoContrato.ChamadosAvulsos
                    },
                    new ClienteModel()
                    {
                        ClienteID = 2,
                        NomeRazaoSocial = "André Rodrigues Oliveira",
                        TipoContrato = TipoContrato.ChamadosAvulsos
                    },
                    new ClienteModel()
                    {
                        ClienteID = 3,
                        NomeRazaoSocial = "Kauã Souza Azevedo",
                        TipoContrato = TipoContrato.ChamadosAvulsos
                    },
                    new ClienteModel()
                    {
                        ClienteID = 4,
                        NomeRazaoSocial = "Melissa Castro Ribeiro",
                        TipoContrato = TipoContrato.ContratoLocacao
                    },
                    new ClienteModel()
                    {
                        ClienteID = 5,
                        NomeRazaoSocial = "Caio Cunha Fernandes",
                        TipoContrato = TipoContrato.ContratoLocacao
                    },
                    new ClienteModel()
                    {
                        ClienteID = 6,
                        NomeRazaoSocial = "Emilly Costa Rodrigues",
                        TipoContrato = TipoContrato.ChamadosAvulsos
                    },
                    new ClienteModel()
                    {
                        ClienteID = 7,
                        NomeRazaoSocial = "Maria Pinto Lima",
                        TipoContrato = TipoContrato.ContratoLocacao
                    },
                    new ClienteModel()
                    {
                        ClienteID = 8,
                        NomeRazaoSocial = "Alex Melo Pereira",
                        TipoContrato = TipoContrato.ContratoLocacao
                    },
                    new ClienteModel()
                    {
                        ClienteID = 9,
                        NomeRazaoSocial = "Sarah Dias Martins",
                        TipoContrato = TipoContrato.ChamadosAvulsos
                    }
                 }
            });

        }
    }
}