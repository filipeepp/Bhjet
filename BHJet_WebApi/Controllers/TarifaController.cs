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
        [Route("cliente/{idCliente:long}")]
        [ResponseType(typeof(IEnumerable<TarifaDTO>))]
        public IHttpActionResult GetTarifasCliente(long idCliente)
        {
            // Busca Dados resumidos
            var entidade = new TarifaRepositorio().BuscaTarificaPorCliente(idCliente);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(trf => new TarifaDTO()
            {
                ID = trf.ID,
                Descricao = trf.Descricao,
                ValorDiaria = trf.ValorDiaria
            }));
        }



    }
}