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
			return View(new ClienteModel()
			{
				Codigo = 14897,
				NomeRazaoSocial = "Sarah Dias Martins",
				NomeFantasia = "",
				CPFCNPJ = "834.748.174-18",
				InscricaoEstadual = "10.4564.8889-00",
				ISS = true,
				Endereco = "Rua Valdemar",
				NumeroEndereco = "332 A",
				Complemento = "Casa",
				Bairro = "Cristo Redentor",
				Cidade = "João Pessoa",
				Estado = "PB",
				CEP = "58070-570",
				Observacoes = "is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
				HomePage = "https://www.bhjet.com.br/"
			});
		}
    }
}