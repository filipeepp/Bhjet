using BHJet_Admin.Infra;
using BHJet_Admin.Models;
using BHJet_Admin.Models.Clientes;
using BHJet_Core.Enum;
using BHJet_DTO.Cliente;
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
				DadosCadastrais = new DadosCadastraisModel() { },
				Contato = new ContatoModel[]
                {
                    new ContatoModel(){}
                },
				Valor = new ValorModel[]
                {
                    new ValorModel(){}
                }
			});
		}


		[HttpPost]
        public ActionResult NovoCliente(ClienteModel model)
        {
			var entidade = new ClienteDTO()
			{
				ID = model.ID,
				DadosCadastrais = new ClienteDadosCadastraisDTO()
				{
					Codigo = model.DadosCadastrais.Codigo,
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
				Contato = model.Contato.Select(x => new ClienteContatoDTO(){

					Contato = x.Contato,
					Email = x.Email,
					TelefoneComercial = x.TelefoneComercial,
					TelefoneCelular = x.TelefoneCelular,
					Setor = x.Setor,
					DataNascimento = x.DataNascimento

				}).ToArray(),
				Valor = model.Valor.Select(x => new ClienteValorDTO(){

					ValorUnitario = x.ValorUnitario,
					TipoTarifa = x.TipoTarifa,
					VigenciaInicio = x.VigenciaInicio,
					VigenciaFim = x.VigenciaFim,
					Franquia = x.Franquia,
					FranquiaAdicional = x.FranquiaAdicional,
					Observacao = x.Observacao

				}).ToArray()
			};

			return View(model);

			/*    var entidade = new ProfissionalCompletoModel()
            {
                ID = model.ID,
                NomeCompleto = model.NomeCompleto,
                Email = model.Email,
                CelularWpp = model.CelularWhatsapp,
                CPF = model.CpfCnpj,
                TelefoneResidencial = model.TelefoneResidencial,
                TelefoneCelular = model.TelefoneCelular,
                CNH = model.CNH,
                ContratoCLT = model.TipoRegimeContratacao == BHJet_Core.Enum.RegimeContratacao.CLT ? true : false,
                Observacao = model.Observacao,
                TipoCNH = model.TipoCarteiraMotorista,
                TipoRegime = model.TipoRegimeContratacao,
                Cep = model.Cep,
                Rua = model.Rua,
                Bairro = model.Bairro,
                Cidade = model.Cidade,
                Complemento = model.Complemento,
                EnderecoPrincipal = model.EnderecoPrincipal,
                PontoReferencia = model.PontoReferencia,
                RuaNumero = model.RuaNumero,
                UF = model.UF,
            };

            // Alteração
            if (model.EdicaoCadastro)
                profissionalServico.AtualizaDadosProfissional(entidade); // Atualiza dados do profissional
            else
                profissionalServico.IncluirProfissional(entidade); // Atualiza dados do profissional

            // Return
            return View(model);
        } */
		}

		public ActionResult CarregarNovoContato()
        {
            var model = new ContatoModel[]
            {
                new ContatoModel(){ }
            };
            return PartialView("_Contato", model);
        }

		/*public ActionResult NovoCliente(ClienteModel model)
        {
            return View(model);
        }*/

		public ActionResult CarregarNovoValor()
		{
            var model = new ValorModel[]
           {
                new ValorModel(){ }
           };
            return PartialView("_Valor", model);
		}
	}
}