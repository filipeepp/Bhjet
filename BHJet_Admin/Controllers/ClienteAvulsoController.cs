using BHJet_Admin.Models.ClienteAvulso;
using BHJet_Servico.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class ClienteAvulsoController : Controller
    {
		private readonly IClienteServico clienteServico;

		public ClienteAvulsoController(IClienteServico _cliente)
		{
			clienteServico = _cliente;
		}

		public ActionResult Index()
        {
            return View();
        }

		public ActionResult Listar()
		{
			try
			{
				var entidade = clienteServico.BuscaClientesAvulsosValorAtivo();

				return View(new ListaClienteAvulsoModel()
				{
					ListClienteAvulso = entidade.Select(x => new ComponenteClienteAvulsoModel()
					{
						ClienteID = x.ID,
						NomeRazaoSocial = x.vcNomeRazaoSocial
					}).ToList()
				});
			}
			catch(Exception e)
			{
				this.TrataErro(e);
				return View(new ListaClienteAvulsoModel()
				{
					ListClienteAvulso = new List<ComponenteClienteAvulsoModel>() { }

				});
			}
		}

		public ActionResult Visualizar()
		{
			return View();
		}
    }
}