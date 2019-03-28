using BHJet_DTO.Faturamento;
using BHJet_Repositorio.Admin;
using System;
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
        [Route("")]
        [Authorize]
        public IHttpActionResult PostGerarFaturamento([FromBody]GerarFaturamentoDTO model)
        {
            try
            {
                // Busca Dados detalhados da corrida/OS
                var fatRepositosio = new FaturamentoRepositorio();
                var listaClientes = model.IdCliente != null ? model.IdCliente.ToArray() : new long[] { };

                // Gera faturamento corridas
                fatRepositosio.GeraFaturamentoCorridas(listaClientes, model.DataInicioFaturamento, model.DataFimFaturamento);

                // Gera taturamento diarias
                fatRepositosio.GeraFaturamentoDiarias(listaClientes, model.DataInicioFaturamento, model.DataFimFaturamento);

                // Busca Itens Faturamentos incluidos
                var entidade = fatRepositosio.BuscaItemFaturamento(listaClientes, null, model.DataInicioFaturamento, model.DataFimFaturamento);

                // valida retorno
                if (entidade != null && !entidade.Any())
                    return StatusCode(System.Net.HttpStatusCode.NoContent);

                // Return
                return Ok(entidade.Select(x => new ItemFaturamentoDTO()
                {
                    ID = x.ID,
                    IDCliente = x.IDCliente,
                    NomeCliente = x.NomeCliente,
                    Periodo = x.Periodo,
                    TipoContrato = x.TipoDescContrato,
                    Valor = x.Valor
                }));
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
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
                IDCliente = x.IDCliente,
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
        [Route("detalhe")]
        public IHttpActionResult GetDetalheFaturamentoComum([FromUri]ConsultarFaturamentoDetalheDTO filtro)
        {
            // Busca Dados detalhados da corrida/OS
            var fatRepositosio = new FaturamentoRepositorio();

            // Busca  Diarias faturadas
            var entidadeDiarias = fatRepositosio.BuscaDetalheItemFaturadoDiaria(filtro.IDCliente, filtro.DataInicioFaturamentoFiltro, filtro.DataFimFaturamentoFiltro);
            // Busca  Corridas faturadas
            var entidadeCorridas = fatRepositosio.BuscaDetalheItemFaturadoCorrida(filtro.IDCliente, filtro.DataInicioFaturamentoFiltro, filtro.DataFimFaturamentoFiltro);
            // Uniao
            var resultado = entidadeDiarias.Union(entidadeCorridas);

            // valida retorno
            if (resultado != null && !resultado.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(resultado.Select(x => new ItemFaturamentoDetalheDTO()
            {
                Data = x.Data,
                KM = x.KM,
                NomeCliente = x.NomeCliente,
                OS = x.OS,
                Profissional = x.Profissional,
                Tipo = x.Tipo,
                Valor = x.Valor
            }));
        }
    }
}