using BHJet_Enumeradores;
using BHJet_Core.Variaveis;
using BHJet_DTO.Corrida;
using BHJet_Repositorio.Admin;
using BHJet_Repositorio.Admin.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("api/Corrida")]
    public class CorridaController : ApiController
    {
        /// <summary>
        /// Busca localização de chamados de um tipo especifico para um tipo especifico de motorista
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("{idOS:long}")]
        [ResponseType(typeof(DetalheCorridaModel))]
        public IHttpActionResult GetCorrida(int idOS)
        {
            // Busca Dados detalhados da corrida/OS
            var entidade = new CorridaRepositorio().BuscaDetalheCorrida(idOS);

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(new DetalheCorridaModel()
            {
                IDCliente = entidade.FirstOrDefault().IDCliente,
                IDProfissional = entidade.FirstOrDefault().IDProfissional,
                NumeroOS = entidade.FirstOrDefault().NumeroOS,
                Origem = new DetalheOSEnderecoModel()
                {
                    EnderecoCompleto = entidade.FirstOrDefault().EnderecoCompleto,
                    ProcurarPor = entidade.FirstOrDefault().ProcurarPor,
                    Realizar = Realizar(entidade.FirstOrDefault()),
                    StatusCorrida = entidade.FirstOrDefault().StatusCorrida,
                    TempoEspera = entidade.FirstOrDefault().TempoEspera?.TimeOfDay,
                    Observacao = entidade.FirstOrDefault().Observacao,
                    CaminhoProtocolo = entidade.FirstOrDefault().CaminhoProtocolo
                },
                Destinos = entidade.Count() > 1 ? entidade.Skip(1).Select(x => new DetalheOSEnderecoModel()
                {
                    EnderecoCompleto = x.EnderecoCompleto,
                    ProcurarPor = x.ProcurarPor,
                    Realizar = Realizar(x),
                    StatusCorrida = x.StatusCorrida,
                    TempoEspera = x.TempoEspera?.TimeOfDay,
                    Observacao = x.Observacao,
                    CaminhoProtocolo = x.CaminhoProtocolo
                }).ToArray() : new DetalheOSEnderecoModel[] { }
            });
        }

        /// <summary>
        /// Busca Corrida em aberto
        /// </summary>
        /// <returns>CorridaEncontradaEntidade</returns>
        [Authorize]
        [Route("aberta/{tipoProfissional:int}")]
        [ResponseType(typeof(CorridaEncontradaEntidade))]
        public IHttpActionResult GetCorridaAberta(int tipoProfissional)
        {
            // Busca Dados detalhados da corrida/OS
            var entidade = new CorridaRepositorio().BuscaCorridaAberta(tipoProfissional);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade);
        }

        private string Realizar(OSCorridaEntidade entidade)
        {
            List<string> realizar = new List<string>();
            if (entidade.bitColetarAssinatura)
                realizar.Add("Coletar Assinatura");
            if (entidade.bitEntregarDocumento)
                realizar.Add("Entregar Documento");
            if (entidade.bitEntregarObjeto)
                realizar.Add("Entregar Objeto");
            if (entidade.bitRetirarDocumento)
                realizar.Add("Retirar Documento");
            if (entidade.bitRetirarObjeto)
                realizar.Add("Retirar Objeto");

            return string.Join(", ", realizar.ToArray());
        }

        /// <summary>
        /// Busca localização de chamados de um tipo especifico para um tipo especifico de motorista
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("status/{statusCorrida:int}/profissional/{tipoProfissional:int}/localizacao")]
        [ResponseType(typeof(List<LocalizacaoCorridaModel>))]
        public IHttpActionResult GetLocalizacaoCorridas(int statusCorrida, int tipoProfissional)
        {
            #region Validações
            // Tipo profissional desejado
            TipoProfissional tipo;
            if (!Enum.TryParse(tipoProfissional.ToString(), out tipo))
                BadRequest(Mensagem.Validacao.TipoProfissionalInvalido);
            // Status de corrida
            StatusCorrida status;
            if (!Enum.TryParse(statusCorrida.ToString(), out status))
                BadRequest(Mensagem.Validacao.StatusCorridaInvalido);
            #endregion

            // Busca Dados resumidos
            var entidade = new CorridaRepositorio().BuscaLocalizacaoCorrida(status, tipo);

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(pf => new LocalizacaoCorridaModel()
            {
                idCorrida = pf.idCorrida,
                geoPosicao = pf.vcLatitude + ";" + pf.vcLongitude,
            }));
        }

		/// <summary>
		/// Busca detalhes da corrida por cliente
		/// </summary>
		/// <returns>List<DetalheCorridaModel></returns>
		[Authorize]
		[Route("cliente/{clienteID:long}")]
		[ResponseType(typeof(DetalheCorridaModel))]
		public IHttpActionResult GetCorridaCliente(long clienteID)
		{
			// Busca Dados detalhados da corrida/OS
			var entidade = new CorridaRepositorio().BuscaDetalheCorridaCliente(clienteID);

			// Validacao
			if (entidade == null || !entidade.Any())
				return StatusCode(System.Net.HttpStatusCode.NoContent);

			// Return
			return Ok(entidade.Select(dtm => new DetalheCorridaModel()
			{
				IDCliente = dtm.IDCliente,
				NumeroOS = dtm.NumeroOS,
				DataInicio = dtm.DataHoraInicio,
				NomeProfissional = dtm.NomeProfissional,
				ValorFinalizado = dtm.ValorFinalizado
			}));
		}

	}
}



//select
//    LC.idCorrida,
//     ED.vcRua + ' - ' + ED.vcNumero + ', ' + ED.vcBairro + ' / ' + ED.vcCidade as EnderecoCompleto,
//     EC.vcPessoaContato,
//     EC.vcObservacao,
//     EC.bitEntregarDocumento,
//       EC.bitColetarAssinatura,
//     EC.bitRetirarDocumento,
//     EC.bitRetirarObjeto,
//     EC.bitEntregarObjeto,
//     EC.bitOutros,
//     LC.geoPosicao
//				 -- TELEFONE NAO TEM
// from tblLogCorrida LC
//        join tblEnderecosCorrida EC on (LC.idCorrida = EC.idCorrida)
//                    JOIN tblEnderecos ED on(EC.idEndereco = ED.idEndereco)
//                    where LC.idCorrida = 6



//                    select* from
//                        tblDOMTipoOcorrenciaCorrida