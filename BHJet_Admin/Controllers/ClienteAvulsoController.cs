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

		public ActionResult Visualizar(string clienteID)
		{ 
			try
			{
				//Converte para long
				long clienteIDLong = Convert.ToInt64(clienteID);

				//Model dados do cliente
				var entidadeDadosCliente = clienteServico.BuscaClientePorID(clienteIDLong);

				//Model dados da(s) os(`s)
				var entidadeOs = clienteServico.BuscaOsCliente(clienteIDLong);

				return View(new ClienteAvulsoModel()
				{
					DadosCadastrais = new DadosCadastraisClienteAvulsoModel()
					{
						NomeRazaoSocial = entidadeDadosCliente.DadosCadastrais.NomeRazaoSocial,
						CPFCNPJ = entidadeDadosCliente.DadosCadastrais.CPFCNPJ,
						Endereco = entidadeDadosCliente.DadosCadastrais.Endereco,
						TelefoneResidencial = entidadeDadosCliente.Contato.FirstOrDefault().TelefoneComercial,
						TelefoneCelular = entidadeDadosCliente.Contato.FirstOrDefault().TelefoneCelular,
						Email = entidadeDadosCliente.Contato.FirstOrDefault().Email,
						DataNascimento = entidadeDadosCliente.Contato.FirstOrDefault().DataNascimento.ToString("dd/MM/YYYY")
					},
					OrdemServico = entidadeOs.Any() ? entidadeOs.Select(eos => new OsClienteAvulsoModel()
					{
						ID = eos.NumeroOS,
						Data = Convert.ToString(eos.DataInicio),
						NomeMotorista = eos.NomeProfissional
						//Valor = eos.ValorFinalizado

					}).ToList() : new List<OsClienteAvulsoModel>() { }
					
				});

			}
			catch(Exception e)
			{
				this.TrataErro(e);
				return View(new ClienteAvulsoModel() { });
			}

		}
    }
}