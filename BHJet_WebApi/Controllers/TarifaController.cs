using BHJet_DTO.Tarifa;
using BHJet_Enumeradores;
using BHJet_Repositorio.Admin;
using BHJet_Repositorio.Admin.Entidade;
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
        /// Busca todo o tarifario
        /// </summary>
        /// <returns>IEnumerable<TarifaDTO></returns>
        [Authorize]
        [Route("")]
        [ResponseType(typeof(TarifarioDTO[]))]
        public IHttpActionResult GetTarifas()
        {
            // Busca tarifa
            var tarifas = new TarifaRepositorio().BuscaTarifaPadrao();

            // Validacao
            if (tarifas == null)
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(tarifas.Select(tf => new TarifarioDTO()
            {
                idTarifario = tf.idTarifario,
                Ativo = tf.bitAtivo,
                idTipoServico = (TipoServico)tf.idTipoServico,
                idTipoVeiculo = (TipoVeiculo)tf.idTipoVeiculo,
                DataFimVigencia = tf.dtDataFimVigencia,
                DataInicioVigencia = tf.dtDataInicioVigencia,
                DescricaoTarifario = tf.vcDescricaoTarifario,
                FranquiaHoras = tf.intFranquiaHoras,
                FranquiaKM = tf.intFranquiaKM,
                FranquiaMinutosParados = tf.intFranquiaMinutosParados,
                Observacao = tf.vcObservacao,
                ValorContrato = tf.decValorContrato,
                ValorHoraAdicional = tf.decValorHoraAdicional,
                ValorKMAdicional = tf.decValorKMAdicional,
                ValorMinutoParado = tf.decValorMinutoParado,
                ValorPontoExcedente = tf.decValorPontoExcedente,
                ValorPontoColeta = tf.decValorPontoColeta
            }));
        }

        /// <summary>
        /// Busca todo o tarifario
        /// </summary>
        /// <returns>IEnumerable<TarifaDTO></returns>
        [Route("")]
        [Authorize]
        public IHttpActionResult PutTarifas([FromBody]TarifarioDTO[] filtro)
        {
            // Busca tarifa
            new TarifaRepositorio().AtualizaTarifaPadrao(filtro.Select(tf => new TarifarioEntidade()
            {
                idTarifario = tf.idTarifario,
                bitAtivo = tf.Ativo,
                idTipoServico = (int)tf.idTipoServico,
                idTipoVeiculo = (int)tf.idTipoVeiculo,
                dtDataFimVigencia = tf.DataFimVigencia,
                dtDataInicioVigencia = tf.DataInicioVigencia,
                vcDescricaoTarifario = tf.DescricaoTarifario,
                intFranquiaHoras = tf.FranquiaHoras,
                intFranquiaKM = tf.FranquiaKM,
                intFranquiaMinutosParados = tf.FranquiaMinutosParados,
                vcObservacao = tf.Observacao,
                decValorContrato = tf.ValorContrato,
                decValorHoraAdicional = tf.ValorHoraAdicional,
                decValorKMAdicional = tf.ValorKMAdicional,
                decValorMinutoParado = tf.ValorMinutoParado,
                decValorPontoExcedente = tf.ValorPontoExcedente,
                decValorPontoColeta = tf.ValorPontoColeta
            }).ToArray());

            // Return
            return Ok();
        }

        /// <summary>
        /// Busca lista de Tarifias de um cliente especifico
        /// </summary>
        /// <returns>IEnumerable<TarifaDTO></returns>
        [Authorize]
        [Route("cliente")]
        [ResponseType(typeof(TarifaDTO))]
        public IHttpActionResult GetTarifaCliente([FromUri]long? idCliente, [FromUri]int? tipoVeiculo)
        {
            // Busca Dados resumidos
            var entidade = new TarifaRepositorio().BuscaTarificaPorCliente(idCliente, tipoVeiculo);

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
