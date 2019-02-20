using BHJet_DTO.Tarifa;
using BHJet_Repositorio.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("api/Tarifa")]
    public class TarifaController : ApiController
    {

        /// <summary>
        /// Busca lista de Tarifias de um cliente especifico
        /// </summary>
        /// <returns>IEnumerable<TarifaDTO></returns>
        [Authorize]
        [Route("cliente")]
        [ResponseType(typeof(TarifaDTO))]
        public IHttpActionResult GetTarifaCliente([FromUri]long? idCliente)
        {
            // Busca Dados resumidos
            var entidade = new TarifaRepositorio().BuscaTarificaPorCliente(idCliente);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(new TarifaDTO()
            {
                ID = entidade.ID,
                Ativo = entidade.Ativo,
                DataFimVigencia = entidade.DataFimVigencia,
                DataInicioVigencia = entidade.DataInicioVigencia,
                Descricao = entidade.Descricao,
                FranquiaKM = entidade.FranquiaKM,
                MinutosParados = entidade.MinutosParados,
                Observacao = entidade.Observacao,
                ValorContrato = entidade.ValorContrato,
                ValorKMAdicional = entidade.ValorKMAdicional,
                ValorMinutosParados = entidade.ValorMinutosParados
            });
        }

        /// <summary>
        /// Busca tarifário padrão ativo
        /// </summary>
        /// <returns>IEnumerable<TarifaDTO></returns>
        [Authorize]
        [Route("tipoVeiculo/{codigoTipoVeiculo:int}")]
        [ResponseType(typeof(TarifaDTO))]
        public IHttpActionResult GetTarifaPadrao(int codigoTipoVeiculo)
        {
            // Busca tarifa
            var entidade = new TarifaRepositorio().BuscaTarfaPadraoAtiva(codigoTipoVeiculo);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(new TarifaDTO()
            {
                ID = entidade.ID,
                Ativo = entidade.Ativo,
                DataFimVigencia = entidade.DataFimVigencia,
                DataInicioVigencia = entidade.DataInicioVigencia,
                Descricao = entidade.Descricao,
                FranquiaKM = entidade.FranquiaKM,
                MinutosParados = entidade.MinutosParados,
                Observacao = entidade.Observacao,
                ValorContrato = entidade.ValorContrato,
                ValorKMAdicional = entidade.ValorKMAdicional,
                ValorMinutosParados = entidade.ValorMinutosParados
            });
        }
    }
}
