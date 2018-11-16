using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Clientes;
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
            return View(new TabelaClienteModel()
            {
                ListModel = new List<LinhaClienteModel>()
                 {
                    new LinhaClienteModel()
                    {
                        ClienteID = 1,
                        NomeRazaoSocial = "Tânia Azevedo Araujo",
                        TipoContrato = TipoContrato.ChamadosAvulsos
                    },
                    new LinhaClienteModel()
                    {
                        ClienteID = 2,
                        NomeRazaoSocial = "André Rodrigues Oliveira",
                        TipoContrato = TipoContrato.ChamadosAvulsos
                    },
                    new LinhaClienteModel()
                    {
                        ClienteID = 3,
                        NomeRazaoSocial = "Kauã Souza Azevedo",
                        TipoContrato = TipoContrato.ChamadosAvulsos
                    },
                    new LinhaClienteModel()
                    {
                        ClienteID = 4,
                        NomeRazaoSocial = "Melissa Castro Ribeiro",
                        TipoContrato = TipoContrato.ContratoLocacao
                    },
                    new LinhaClienteModel()
                    {
                        ClienteID = 5,
                        NomeRazaoSocial = "Caio Cunha Fernandes",
                        TipoContrato = TipoContrato.ContratoLocacao
                    },
                    new LinhaClienteModel()
                    {
                        ClienteID = 6,
                        NomeRazaoSocial = "Emilly Costa Rodrigues",
                        TipoContrato = TipoContrato.ChamadosAvulsos
                    },
                    new LinhaClienteModel()
                    {
                        ClienteID = 7,
                        NomeRazaoSocial = "Maria Pinto Lima",
                        TipoContrato = TipoContrato.ContratoLocacao
                    },
                    new LinhaClienteModel()
                    {
                        ClienteID = 8,
                        NomeRazaoSocial = "Alex Melo Pereira",
                        TipoContrato = TipoContrato.ContratoLocacao
                    },
                    new LinhaClienteModel()
                    {
                        ClienteID = 9,
                        NomeRazaoSocial = "Sarah Dias Martins",
                        TipoContrato = TipoContrato.ChamadosAvulsos
                    }
                 }
            });

        }


        public ActionResult NovoCliente()
        {
			return View( new ClienteModel()
            {
                DadosCadastrais = new DadosCadastraisModel() {},
                Contato = new ContatoModel[] {},
                Valor = new ValorModel() { }
            });
        }

        public ActionResult CarregarNovoContato()
        {
            return PartialView("_Contato");
        }

        /*public ActionResult NovoCliente(ClienteModel model)
        {
            return View(model);
        }*/
    }
}