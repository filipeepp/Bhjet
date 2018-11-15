﻿using BHJet_Core.Enum;
using BHJet_Core.Variaveis;
using BHJet_DTO.Corrida;
using BHJet_Repositorio.Admin;
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
        [Route("id/{idOS:long}")]
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
                    Realizar = entidade.FirstOrDefault().Realizar,
                    StatusCorrida = entidade.FirstOrDefault().StatusCorrida,
                    TempoEspera = entidade.FirstOrDefault().TempoEspera,
                    Observacao = entidade.FirstOrDefault().Observacao
                },
                Destinos = entidade.Skip(1).Select(x => new DetalheOSEnderecoModel()
                {
                    EnderecoCompleto = x.EnderecoCompleto,
                    ProcurarPor = x.ProcurarPor,
                    Realizar = x.Realizar,
                    StatusCorrida = x.StatusCorrida,
                    TempoEspera = x.TempoEspera,
                    Observacao = x.Observacao
                }).ToArray()
            });
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
                geoPosicao = pf.GeoLocalizacao,
            }));
        }

    }
}