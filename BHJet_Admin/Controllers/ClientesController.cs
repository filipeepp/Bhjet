using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Clientes;
using BHJet_Core.Enum;
using BHJet_DTO.Cliente;
using BHJet_Servico.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BHJet_Core.Extension;
namespace BHJet_Admin.Controllers
{
    public class ClientesController : Controller
    {
		private readonly IClienteServico clienteServico;

		public ClientesController(IClienteServico _cliente)
		{
			clienteServico = _cliente;
		}

		[ValidacaoUsuarioAttribute()]
		public ActionResult Clientes()
        {
			try
			{
				var entidade = clienteServico.BuscaClientesValorAtivo();
				
				// Return
				return Json(entidade.Select(x => new AutoCompleteModel()
				{
					label = x.ID + " - " + x.vcNomeFantasia,
					value = x.ID
				}), JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				this.TrataErro(e);
				return View(new TabelaClienteModel()
				{
					ListModel = new List<LinhaClienteModel>() { }

				});
			}	
		}

		[ValidacaoUsuarioAttribute()]
		public ActionResult NovoCliente()
		{
			return View(new ClienteModel()
			{
				DadosCadastrais = new DadosCadastraisModel() { }
			});
		}

		[HttpGet]
		[ValidacaoUsuarioAttribute()]
		public ActionResult BuscaCliente(string trechoPesquisa)
		{
			/*try
			{
				var entidade = clienteServico.BuscaListaClientes(trechoPesquisa);

				//Ok
				return View(new TabelaClienteModel()
				{
					ListModel = entidade.Select(x => new LinhaClienteModel()
					{
						ClienteID = x.ID,
						NomeRazaoSocial = x.vcNomeRazaoSocial,
						TipoContrato = x.
					}).ToArray()
				});
			}
			catch (Exception e)
			{
				this.TrataErro(e);
				return View();
			}*/
			return View();
		}


		[HttpPost]
		[ValidacaoUsuarioAttribute()]
		public ActionResult NovoCliente(ClienteModel model)
		{
			try
			{
				var listContatoTratata = new List<ContatoModel>();
				var listValorTratada = new List<ValorModel>();

				foreach (var contato in model.Contato)
				{
					if (contato.ContatoRemovido == false)
						listContatoTratata.Add(contato);
				}

				foreach (var valor in model.Valor)
				{
					if (valor.ValorRemovido == false)
						listValorTratada.Add(valor);
				}

				var entidade = new ClienteCompletoModel()
				{
					DadosCadastrais = new ClienteDadosCadastraisModel()
					{
						NomeRazaoSocial = model.DadosCadastrais.NomeRazaoSocial,
						NomeFantasia = model.DadosCadastrais.NomeFantasia,
						CPFCNPJ = model.DadosCadastrais.CPFCNPJ,
						InscricaoEstadual = model.DadosCadastrais.InscricaoEstadual,
						ISS = model.DadosCadastrais.ISS == true ? 1 : 0,
						Endereco = model.DadosCadastrais.Endereco,
						NumeroEndereco = model.DadosCadastrais.NumeroEndereco,
						Complemento = model.DadosCadastrais.Complemento,
						Bairro = model.DadosCadastrais.Bairro,
						Cidade = model.DadosCadastrais.Cidade,
						Estado = model.DadosCadastrais.Estado,
						CEP = model.DadosCadastrais.CEP,
						Observacoes = model.DadosCadastrais.Observacoes,
						HomePage = model.DadosCadastrais.HomePage
					},
					Contato = listContatoTratata.Select(x => new ClienteContatoModel()
					{
						Contato = x.Contato,
						Email = x.Email,
						TelefoneComercial = x.TelefoneComercial,
						TelefoneCelular = x.TelefoneCelular,
						Setor = x.Setor,
						DataNascimento = x.DataNascimento

					}).ToArray(),
					Valor = listValorTratada.Select(x => new ClienteValorModel()
					{

						ValorUnitario = x.ValorUnitario.ToDecimalCurrency(),
						TipoTarifa = x.TipoTarifa.RetornaDisplayNameEnum(),
						VigenciaInicio = x.VigenciaInicio,
						VigenciaFim = x.VigenciaFim,
						Franquia = Convert.ToDecimal(x.Franquia),
						FranquiaAdicional = Convert.ToDecimal(x.FranquiaAdicional),
						ValorAtivado = x.ValorAtivado == true ? 1 : 0,
						Observacao = x.Observacao

					}).ToArray()
				};

				clienteServico.IncluirCliente(entidade); // Atualiza dados do profissional

				this.TrataSucesso("Cliente incluido com sucesso.");

				//Ok
				return View(new ClienteModel());
			}
			catch (Exception e)
			{
				this.TrataErro(e);
				return View(model);
			}

		}

		[ValidacaoUsuarioAttribute()]
		public ActionResult CarregarNovoContato(ClienteModel model)
        {
            return PartialView("_Contato", model);
        }

		/*public ActionResult NovoCliente(ClienteModel model)
        {
            return View(model);
        }*/

		[ValidacaoUsuarioAttribute()]
		public ActionResult CarregarNovoValor(ClienteModel model)
		{
			return PartialView("_Valor", model);
		}
	}
}