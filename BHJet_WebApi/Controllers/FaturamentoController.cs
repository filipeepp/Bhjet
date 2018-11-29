using BHJet_DTO.Faturamento;
using BHJet_Repositorio.Admin;
using System.Linq;
using System.Web.Http;

namespace BHJet_WebApi.Controllers
{
    /// <summary>
    /// Controller de Faturamentos
    /// </summary>
    [RoutePrefix("api/Faturamento")]
    public class FaturamentoController : ApiController
    {
        /// <summary>
        /// Método para inclusão de diaria avulsa
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public IHttpActionResult PostGerarFaturamento([FromBody]GerarFaturamentoDTO model)
        {
            // Busca Dados detalhados da corrida/OS
            var fatRepositosio = new FaturamentoRepositorio();
            var listaClientes = model.IdCliente != null ? model.IdCliente.ToArray() : new long[] { };

            // Gera faturamento
            fatRepositosio.GeraFaturamento(listaClientes, model.DataInicioFaturamento, model.DataFimFaturamento);

            // Busca Itens Faturamentos incluidos
            var entidade = fatRepositosio.BuscaItemFaturamento(listaClientes, null, model.DataInicioFaturamento, model.DataFimFaturamento);

            // valida retorno
            if (entidade != null && entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(x => new ItemFaturamentoDTO()
            {
                ID = x.ID,
                NomeCliente = x.NomeCliente,
                Periodo = x.Periodo,
                TipoContrato = x.TipoDescContrato,
                Valor = x.Valor
            }));
        }

        /// <summary>
        /// Busca faturamento comum de acordo com filtros
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("comum")]
        public IHttpActionResult PostFaturamentoComum([FromBody]ConsultarFaturamentoDTO filtro)
        {
            // Busca Dados detalhados da corrida/OS
            var fatRepositosio = new FaturamentoRepositorio();
            var listaClientes = filtro.IdClienteFiltro != null ? filtro.IdClienteFiltro.ToArray() : new long[] { };

            // Busca Itens Faturamentos incluidos
            var entidade = fatRepositosio.BuscaItemFaturamento(listaClientes, filtro.TipoContratoFiltro, filtro.DataInicioFaturamentoFiltro, filtro.DataFimFaturamentoFiltro);

            // valida retorno
            if (entidade != null && !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(x => new ItemFaturamentoDTO()
            {
                ID = x.ID,
                NomeCliente = x.NomeCliente,
                Periodo = x.Periodo,
                TipoContrato = x.TipoDescContrato,
                Valor = x.Valor
            }));
        }
    }
}