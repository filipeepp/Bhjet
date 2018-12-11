using BHJet_Core.Extension;
using BHJet_DTO.Cliente;
using BHJet_Repositorio.Admin;
using BHJet_WebApi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("api/Cliente")]
    public class ClienteController : ApiController
    {
		private UsuarioLogado _usuarioAutenticado;

		/// <summary>
		/// Informações do usuário autenticado
		/// </summary>
		public UsuarioLogado UsuarioAutenticado
		{
			get
			{
				if (_usuarioAutenticado == null)
					_usuarioAutenticado = new UsuarioLogado();

				return _usuarioAutenticado;
			}
		}

		/// <summary>
		/// Busca dados de cliente e seus contratos
		/// </summary>
		/// <returns></returns>
		[Authorize]
		[Route("contrato")]
		[ResponseType(typeof(IEnumerable<ClienteDTO>))]
		public IHttpActionResult GetClienteContrato([FromUri]string trecho = "")
		{
			// Busca Dados resumidos
			var entidade = new ClienteRepositorio().BuscaClienteContrato(trecho);

			// Validacao
			if (entidade == null)
				return StatusCode(System.Net.HttpStatusCode.NoContent);

			// Return
			return Ok(entidade.Select(cli => new ClienteDTO()
			{
				ID = cli.idCliente,
				vcNomeRazaoSocial = cli.vcNomeRazaoSocial,
				vcDescricaoTarifario = cli.vcDescricaoTarifario,
				bitAtivo = cli.bitAtivo

			}));
		}


		/// <summary>
		/// Busca dados de cliente
		/// </summary>
		/// <returns></returns>
		[Authorize]
		[Route("contrato/ativo")]
		[ResponseType(typeof(IEnumerable<ClienteDTO>))]
		public IHttpActionResult GetClientesValorAtivo()
		{
			// Busca Dados
			var entidade = new ClienteRepositorio().BuscaListaClientes();

			// Validacao
			if (entidade == null)
				return StatusCode(System.Net.HttpStatusCode.NoContent);

			// Return
			return Ok(entidade.Select(cli => new ClienteDTO()
			{
				ID = cli.idCliente,
				vcNomeFantasia = cli.vcNomeFantasia,
				vcNomeRazaoSocial = cli.vcNomeRazaoSocial,
				vcCPFCNPJ = cli.vcCPFCNPJ,
				vcInscricaoEstadual = cli.vcInscricaoEstadual,
				bitRetemISS = cli.bitRetemISS,
				vcObservacoes = cli.vcObservacoes,
				vcSite = cli.vcSite,
				vcRua = cli.vcRua,
				vcNumero = cli.vcNumero,
				vcComplemento = cli.vcComplemento,
				vcBairro = cli.vcBairro,
				vcCidade = cli.vcCidade,
				vcUF = cli.vcUF,
				bitAtivo = cli.bitAtivo,
				vcDescricaoTarifario = cli.vcDescricaoTarifario

			}));
		}

		/// <summary>
		/// Busca Lista de Clientes
		/// </summary>
		/// <returns></returns>
		[Authorize]
        [Route("")]
        [ResponseType(typeof(IEnumerable<ClienteDTO>))]
        public IHttpActionResult GetListaClientes([FromUri]string trecho = "")
        {
            // Busca Dados resumidos
            var entidade = new ClienteRepositorio().BuscaClientes(trecho);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(cli => new ClienteDTO()
            {
                ID = cli.idCliente,
                bitRetemISS = cli.bitRetemISS,
                idEndereco = cli.idEndereco,
                idUsuario = cli.idEndereco,
                vcNomeRazaoSocial = cli.vcNomeRazaoSocial,
                vcNomeFantasia = cli.vcNomeFantasia,
                vcCPFCNPJ = cli.vcCPFCNPJ,
                vcInscricaoMunicipal = cli.vcInscricaoMunicipal,
                vcInscricaoEstadual = cli.vcInscricaoEstadual,
                vcObservacoes = cli.vcObservacoes,
                vcSite = cli.vcSite,
            }));
        }
		/// <summary>
		/// Post Cliente
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Authorize]
		[Route("")]
		public IHttpActionResult PostCliente([FromBody]ClienteCompletoModel model)
		{
			// Busca Dados resumidos
			var clienteRepositorio = new ClienteRepositorio();

			// Verifica existencia
			var entidade = clienteRepositorio.VerificaExistenciaProfissional(model.DadosCadastrais.CPFCNPJ);

			// VALIDACAO
			if (entidade.Any())
			{
				bool existeCPF = entidade.Where(x => x.vcCPFCNPJ == model.DadosCadastrais.CPFCNPJ).Any();

				string msg = "";
				if (existeCPF)
					msg = "mesmo CPF.";

				return BadRequest($"Existe um cadastro com este {msg}. Favor atualizar os dados corretamente");
			}

			// Inclui profissional
			clienteRepositorio.IncluirCliente(new BHJet_Repositorio.Admin.Entidade.ClienteCompletoEntidade()
			{
				DadosCadastrais = new BHJet_Repositorio.Admin.Entidade.ClienteDadosCadastraisEntidade()
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
				Contato = model.Contato.Any() ? model.Contato.Select(x => new BHJet_Repositorio.Admin.Entidade.ClienteContatoEntidade()
				{
					Contato = x.Contato,
					Email = x.Email,
					TelefoneComercial = x.TelefoneComercial,
					TelefoneCelular = x.TelefoneCelular,
					Setor = x.Setor,
					DataNascimento = x.DataNascimento

				}).ToArray() : new BHJet_Repositorio.Admin.Entidade.ClienteContatoEntidade[] { },
				Valor = model.Valor.Any() ? model.Valor.Select(x => new BHJet_Repositorio.Admin.Entidade.ClienteValorEntidade()
				{
					ValorUnitario = x.ValorUnitario,
					TipoTarifa = x.TipoTarifa,
					VigenciaInicio = x.VigenciaInicio,
					VigenciaFim = x.VigenciaFim,
					Franquia = x.Franquia,
					FranquiaAdicional = x.FranquiaAdicional,
					Observacao = x.Observacao,
					ValorAtivado = x.ValorAtivado

				}).ToArray() : new BHJet_Repositorio.Admin.Entidade.ClienteValorEntidade[] { }
			});

			// Return
			return Ok();
		}


	}
}