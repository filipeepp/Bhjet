using BHJet_Core.Enum;
using BHJet_Core.Variaveis;
using BHJet_Repositorio.Admin;
using BHJet_WebApi.Model.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        /// <summary>
        /// Busca dados de resumo do dashboard da area interna
        /// </summary>
        /// <returns>ResumoModel</returns>
        [Authorize]
        [Route("resumo")]
        [ResponseType(typeof(ResumoModel))]
        public IHttpActionResult GetResumoDashboard()
        {
            // Busca Dados resumidos
            var entidade = new DashboardRepositorio().BuscaResumoDashboard();

            // Return
            return Ok(new ResumoModel()
            {
                ChamadosAguardandoMotociclista = entidade.ChamadosAguardandoMotociclista,
                ChamadosAguardandoMotorista = entidade.ChamadosAguardandoMotorista,
                MotociclistaDisponiveis = entidade.MotociclistaDisponiveis,
                MotoristasDisponiveis = entidade.MotoristasDisponiveis
            });
        }

        /// <summary>
        /// Busca localização de profissionais disponíveis
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("profissional/{tipoProfissional:int}/localizacao")]
        [ResponseType(typeof(List<LocalizacaoProfissionalModel>))]
        public IHttpActionResult GetLocalizacaoMotoristasDisponiveis(int tipoProfissional)
        {
            // Tipo profissional desejado
            TipoProfissional tipo;
            if (!Enum.TryParse(tipoProfissional.ToString(), out tipo))
                BadRequest(Mensagem.Validacao.TipoProfissionalInvalido);

            // Busca Dados resumidos
            var entidade = new DashboardRepositorio().BuscaLocalizacaoProfissionaisDisponiveis(tipo);

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(pf => new LocalizacaoProfissionalModel()
            {
                idColaboradorEmpresaSistema = pf.idColaboradorEmpresaSistema,
                vcLatitude = pf.vcLatitude,
                vcLongitude = pf.vcLongitude
            }));
        }

        /// <summary>
        /// Busca localização de chamados de um tipo especifico para um tipo especifico de motorista
        /// </summary>
        /// <returns>List<LocalizacaoProfissional></returns>
        [Authorize]
        [Route("corridas/{statusCorrida:int}/profissional/{tipoProfissional:int}/localizacao")]
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
            var entidade = new DashboardRepositorio().BuscaLocalizacaoCorrida(status, tipo);

            // Validacao
            if (entidade == null || !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Return
            return Ok(entidade.Select(pf => new LocalizacaoCorridaModel()
            {
                idCorrida = pf.idCorrida,
                GeoLocalizacao = pf.GeoLocalizacao,
            }));
        }
    }
}