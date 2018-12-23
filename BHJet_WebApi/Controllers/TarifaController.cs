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
        [ResponseType(typeof(TarifaDTO))]
        public IHttpActionResult GetTarifaCliente(long idCliente)
        {
            // Busca Dados resumidos
            var entidade = new TarifaRepositorio().BuscaTarificaPorCliente(idCliente);

            // Validacao
            if (entidade == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(new TarifaDTO()
            {
                idTarifario = entidade.idTarifario,
                bitPagamentoAVista = entidade.bitPagamentoAVista,
                decFranquiaKMBandeirada = entidade.decFranquiaKMBandeirada,
                decFranquiaKMDiaria = entidade.decFranquiaKMDiaria,
                decFranquiaKMMensalidade = entidade.decFranquiaKMMensalidade,
                decValorBandeirada = entidade.decValorBandeirada,
                decValorDiaria = entidade.decValorDiaria,
                decValorKMAdicionalCorrida = entidade.decValorKMAdicionalCorrida,
                decValorKMAdicionalDiaria = entidade.decValorKMAdicionalDiaria,
                decValorKMAdicionalMensalidade = entidade.decValorKMAdicionalMensalidade,
                decValorMensalidade = entidade.decValorMensalidade,
                decValorMinutoParado = entidade.decValorMinutoParado,
                intFranquiaMinutosParados = entidade.intFranquiaMinutosParados,
                timFaixaHorarioFinal = entidade.timFaixaHorarioFinal,
                timFaixaHorarioInicial = entidade.timFaixaHorarioInicial
            });
        }



    }
}