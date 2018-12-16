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
using System.Globalization;

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
				ViewBag.MetodoPagina = "Edição";
				try
				{
					var clienteIDLong = long.Parse(clienteID);
					var entidade = clienteServico.BuscaClientePorID(clienteIDLong);

					return View(new ClienteModel()
					{
						ID = entidade.ID,
						DadosCadastrais = new DadosCadastraisModel()
						{
							NomeRazaoSocial = entidade.DadosCadastrais.NomeRazaoSocial,
							NomeFantasia = entidade.DadosCadastrais.NomeFantasia,
							CPFCNPJ = entidade.DadosCadastrais.CPFCNPJ,
							InscricaoEstadual = entidade.DadosCadastrais.InscricaoEstadual,
							ISS = entidade.DadosCadastrais.ISS == 1 ? true : false,
							Endereco = entidade.DadosCadastrais.Endereco,
							NumeroEndereco = entidade.DadosCadastrais.NumeroEndereco,
							Complemento = entidade.DadosCadastrais.Complemento,
							Bairro = entidade.DadosCadastrais.Bairro,
							Cidade = entidade.DadosCadastrais.Cidade,
							Estado = entidade.DadosCadastrais.Estado,
							CEP = entidade.DadosCadastrais.CEP,
							Observacoes = entidade.DadosCadastrais.Observacoes,
							HomePage = entidade.DadosCadastrais.HomePage,
						},

						Contato = entidade.Contato != null ? entidade.Contato.Select(c => new ContatoModel()
						{
							ID = c.ID,
							Contato = c.Contato,
							Email = c.Email,
							TelefoneComercial = c.TelefoneComercial,
							TelefoneCelular = c.TelefoneCelular,
							Setor = c.Setor,
							DataNascimento = c.DataNascimento.ToShortDateString(),

						}).ToList() : new List<ContatoModel>() { },

						Valor = entidade.Valor != null ? entidade.Valor.Select(v => new ValorModel()
						{
							ID = v.ID,
							ValorAtivado = v.ValorAtivado == 1 ? true : false,
							ValorUnitario = Convert.ToString(v.ValorUnitario),
							TipoTarifa = v.TipoTarifa.Contains("Avulso Mensal") ? TipoTarifa.AvulsoMensal : TipoTarifa.AlocacaoMensal,
							VigenciaInicio = v.VigenciaInicio.ToShortDateString(),
							VigenciaFim = v.VigenciaFim.ToShortDateString(),
							Franquia = Convert.ToString(v.Franquia),
							FranquiaAdicional = Convert.ToString(v.FranquiaAdicional),
							Observacao = v.Observacao

						}).ToList() : new List<ValorModel>() { }

					});


				}
				catch(Exception e)
				{
					this.TrataErro(e);
					return View(new ClienteModel());
				}

			}
			ViewBag.MetodoPagina = "Novo";
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
			var edicao = BHJet_Core.Customizavel.MetodoPagina.PaginaEdicao();
			if (edicao)
			{
				return View();
			}
			else
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
							DataNascimento = Convert.ToDateTime(x.DataNascimento)

						}).ToArray(),
						Valor = listValorTratada.Select(x => new ClienteValorModel()
						{

							ValorUnitario = x.ValorUnitario.ToDecimalCurrency(),
							TipoTarifa = x.TipoTarifa.RetornaDisplayNameEnum(),
							VigenciaInicio = Convert.ToDateTime(x.VigenciaInicio),
							VigenciaFim = Convert.ToDateTime(x.VigenciaFim),
							Franquia = x.Franquia.ToDecimalCurrency(),
							FranquiaAdicional = x.FranquiaAdicional.ToDecimalCurrency(),
							ValorAtivado = x.ValorAtivado == true ? 1 : 0,
							Observacao = x.Observacao

						}).ToArray()
					};

					clienteServico.IncluirCliente(entidade); // Atualiza dados do cliente

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
		}

		[ValidacaoUsuarioAttribute()]
		[HttpPost]
		public ActionResult ExcluirContato(string idContato = "")
		{
			var idContatoInt = Convert.ToInt32(idContato);

			try
			{
				clienteServico.ExcluirContato(idContatoInt);
				this.MensagemSucesso("Contato removido com sucesso.");
				return View();

			}
			catch(Exception e)
			{
				this.TrataErro(e);
				return View();
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