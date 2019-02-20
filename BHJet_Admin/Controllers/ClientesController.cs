﻿using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Clientes;
using BHJet_Enumeradores;
using BHJet_Core.Extension;
using BHJet_DTO.Cliente;
using BHJet_Servico.Cliente;
using BHJet_Servico.Tarifa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BHJet_Admin.Controllers
{
    public class ClientesController : Controller
    {
		private readonly IClienteServico clienteServico;
        private readonly ITarifaServico tarifaServico;

        public ClientesController(IClienteServico _cliente, ITarifaServico _tarifaServico)
		{
			clienteServico = _cliente;
            tarifaServico = _tarifaServico;
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
							DataNascimento = c.DataNascimento?.ToShortDateString(),

						}).ToList() : new List<ContatoModel>() { },

						Valor = entidade.Valor != null ? entidade.Valor.Select(v => new ValorModel()
						{
							ID = v.ID,
							ValorAtivado = v.status,
							ValorUnitario = Convert.ToString(v.ValorContrato),
							TipoTarifa = v.DescricaoTarifa.Contains("Avulso Mensal") ? TipoTarifa.AvulsoMensal : TipoTarifa.AlocacaoMensal,
							VigenciaInicio = v.VigenciaInicio.ToShortDateString(),
							VigenciaFim = v.VigenciaFim.ToShortDateString(),
							Franquia = Convert.ToString(v.QuantidadeKMContratado),
							FranquiaAdicional = Convert.ToString(v.ValorKMAdicional),
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
			var edicao = false;
			//Verifica se a requisição é edição
			if (Request.UrlReferrer.Query.Contains("edicao"))
				edicao = Request.UrlReferrer.Query.Split('&')[0].Split('=')[1].Trim().ToLower() == "true" ? true : false;

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
					ID = model.ID,
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
					Contato = listContatoTratata.Select(c => new ClienteContatoModel()
					{
						ID = c.ID,
						Contato = c.Contato,
						Email = c.Email,
						TelefoneComercial = c.TelefoneComercial,
						TelefoneCelular = c.TelefoneCelular,
						Setor = c.Setor,
						DataNascimento = string.IsNullOrEmpty(c.DataNascimento) ? (DateTime?)null : Convert.ToDateTime(c.DataNascimento)

					}).ToArray(),
					Valor = listValorTratada.Select(v => new ClienteValorModel()
					{
						IDCliente = v.ID,
						ValorContrato = v.ValorUnitario.ToDecimalCurrency(),
						DescricaoTarifa = v.TipoTarifa.RetornaDisplayNameEnum(),
						VigenciaInicio = Convert.ToDateTime(v.VigenciaInicio),
						VigenciaFim = Convert.ToDateTime(v.VigenciaFim),
						QuantidadeKMContratado = v.Franquia != null ? int.Parse(v.Franquia) : 0,
                        ValorKMAdicional = v.FranquiaAdicional.ToDecimalCurrency(),
						status = v.ValorAtivado,
						Observacao = v.Observacao

					}).ToArray()
				};

				if(edicao)
				{
					//Incluir Contato se adicionado novo
					foreach(var contato in entidade.Contato)
					{
						if (contato.ID == 0)
							clienteServico.IncluirContato(contato, entidade.ID);
					}

					//Incluir Valor se adicionado novo
					foreach (var valor in entidade.Valor)
					{
						if (valor.ID == 0)
							clienteServico.IncluirValor(valor, entidade.ID);
					}

					clienteServico.EditarCliente(entidade);// Atualiza dados do cliente

					this.MensagemSucesso("Cliente atualizado com sucesso.");

					return View(model);
				}
				else
				{
					clienteServico.IncluirCliente(entidade); // Insere dados do cliente

					this.MensagemSucesso("Cliente incluido com sucesso.");
					ModelState.Clear();

					//Ok
					return View(new ClienteModel());
				}
			}
			catch (Exception e)
			{
				this.TrataErro(e);
				return View(model);
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
				return new EmptyResult();

			}
			catch(Exception e)
			{
				this.TrataErro(e);
				return View();
			}
		}

		[ValidacaoUsuarioAttribute()]
		[HttpPost]
		public ActionResult ExcluirValor(string idValor = "")
		{
			var idValorInt = Convert.ToInt32(idValor);

			try
			{
				clienteServico.ExcluirValor(idValorInt);
				return new EmptyResult();

			}
			catch (Exception e)
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