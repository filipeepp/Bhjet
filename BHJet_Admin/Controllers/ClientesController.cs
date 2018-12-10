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

		// GET: Clientes
		[ValidacaoUsuarioAttribute()]
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

		[ValidacaoUsuarioAttribute()]
		public ActionResult NovoCliente()
		{
			return View(new ClienteModel()
			{
				DadosCadastrais = new DadosCadastraisModel() { }
			});
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
						ISS = model.DadosCadastrais.ISS,
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

						ValorUnitario = x.ValorUnitario,
						TipoTarifa = x.TipoTarifa.RetornaDisplayNameEnum(),
						VigenciaInicio = x.VigenciaInicio,
						VigenciaFim = x.VigenciaFim,
						Franquia = x.Franquia,
						FranquiaAdicional = x.FranquiaAdicional,
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

		public ActionResult CarregarNovoContato(ClienteModel model)
        {
            return PartialView("_Contato", model);
        }

		/*public ActionResult NovoCliente(ClienteModel model)
        {
            return View(model);
        }*/

		public ActionResult CarregarNovoValor(ClienteModel model)
		{
			return PartialView("_Valor", model);
		}
	}
}