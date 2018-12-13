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
		public ActionResult Index()
		{
			return View();
		}

		[ValidacaoUsuarioAttribute()]
		public ActionResult Clientes()
        {
			try
			{
				var entidade = clienteServico.BuscaClientesValorAtivo();

				// Return
				return View(new TabelaClienteModel()
				{
					ListModel = entidade.Select(x => new LinhaClienteModel()
					{
						ClienteID = x.ID,
						NomeRazaoSocial = x.vcNomeRazaoSocial,
						TipoContrato = x.vcDescricaoTarifario,
						ValorAtivado = x.bitAtivo == 1 ? "Ativo" : "Inativo"
					}).ToList()
				});
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
		public ActionResult NovoCliente(bool edicao = false, string clienteID = "")
		{
			if (edicao)
			{
				try
				{
					var clienteIDLong = long.Parse(clienteID);
					var entidade = clienteServico.BuscaClientePorID(clienteIDLong);


				}
				catch(Exception e)
				{

				}

			}
			return View(new ClienteModel()
			{
				DadosCadastrais = new DadosCadastraisModel() { }
			});
		}

	

		[HttpPost]
		[ValidacaoUsuarioAttribute()]
		public ActionResult Clientes(string palavraChave)
		{
			try
			{
				var entidade = clienteServico.BuscaClienteContrato(palavraChave);

				//Ok
				return View(new TabelaClienteModel()
				{
					ListModel = entidade.Select(x => new LinhaClienteModel()
					{
						ClienteID = x.ID,
						NomeRazaoSocial = x.vcNomeRazaoSocial,
						TipoContrato = x.vcDescricaoTarifario,
						ValorAtivado = x.bitAtivo == 1 ? "Ativo" : "Inativo"
					}).ToList()
				});
			}
			catch (Exception e)
			{
				this.TrataErro(e);
				return View(new TabelaClienteModel());
			}
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

				this.MensagemSucesso("Cliente incluido com sucesso.");
				ModelState.Clear();

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


		[ValidacaoUsuarioAttribute()]
		public ActionResult CarregarNovoValor(ClienteModel model)
		{
			return PartialView("_Valor", model);
		}
	}
}