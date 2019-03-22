using BHJet_Enumeradores;
using BHJet_DTO.Dashboard;
using BHJet_Repositorio.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System;

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
        /// Busca dados de resumo da quantidade de chamados concluidos e advetentes no ultimo ano
        /// </summary>
        /// <returns>ResumoModel</returns>
        [Authorize]
        [Route("resumo/chamado")]
        [ResponseType(typeof(IEnumerable<ResumoChamadoModel>))]
        public IHttpActionResult GetResumoChamados()
        {
            // Busca Dados resumidos
            var entidade = new DashboardRepositorio().BuscaResumoChamados();

            // Busca status corrida
            var statusCorrida = new CorridaRepositorio().BuscaStatusCorrida();
            var finaliza = statusCorrida.Where(s => s.bitFinaliza = true || s.bitCancela == true).Select(st => st.idStatusCorrida).ToArray();
            var naoFinaliza = statusCorrida.Where(s => s.bitFinaliza = false && s.bitCancela == false).Select(st => st.idStatusCorrida).ToArray();

            // valida retorno
            if (entidade != null && !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Separa chamados 
            List<ResumoChamadoModel> ResumoMensal = new List<ResumoChamadoModel>();

            // Percorre os meses
            var periodos = this.UltimosMesesAno(13);

            foreach (var periodo in periodos)
            {
                ResumoMensal.Add(new ResumoChamadoModel()
                {
                    Mes = periodo,
                    ChamadosAdvertentes = entidade.Where(x => x.DataRegistro?.ToString("MM/yyyy") == periodo && Array.Exists(naoFinaliza, z => z == (int)x.Status))?.Count() ?? 0,
                    ChamadosConcluidos = entidade.Where(x => x.DataRegistro?.ToString("MM/yyyy") == periodo &&  Array.Exists(finaliza, z => z == (int)x.Status))?.Count() ?? 0
                });
            }

            // Return
            return Ok(ResumoMensal);
        }

        /// <summary>
        /// Busca dados de resumo da quantidade de chamados atendidos por motoristas ou motociclistas  no ultimo ano
        /// </summary>
        /// <returns>ResumoModel</returns>
        [Authorize]
        [Route("resumo/atendimento")]
        [ResponseType(typeof(List<ResumoAtendimentoModel>))]
        public IHttpActionResult GetResumoChamadosFuncionarios()
        {
            // Busca Dados resumidos
            var entidade = new DashboardRepositorio().BuscaResumoAtendimentosProfissionais();

            // valida retorno
            if (entidade != null && !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Separa chamados 
            List<ResumoAtendimentoModel> ResumoMensal = new List<ResumoAtendimentoModel>();

            // Percorre os meses
            var periodos = this.UltimosMesesAno(13);
            foreach (var periodo in periodos)
            {
                ResumoMensal.Add(new ResumoAtendimentoModel()
                {
                    Mes = periodo,
                    QtdAtendimentoMotociclista = entidade.Where(x => x.DataRegistro?.ToString("MM/yyyy") == periodo && x.TipoProfissional == TipoProfissional.Motociclista)?.Count() ?? 0,
                    QtdAtendimentoMotorista = entidade.Where(x => x.DataRegistro?.ToString("MM/yyyy") == periodo && x.TipoProfissional == TipoProfissional.Motorista)?.Count() ?? 0,
                });

            }
           
            // Return
            return Ok(ResumoMensal);
        }


        private IEnumerable<string> UltimosMesesAno(int qtdMeses)
        {
            return Enumerable.Range(0, qtdMeses).Select(i => DateTime.Now.AddMonths(1).AddMonths(i - qtdMeses)).Select(date => date.ToString("MM/yyyy")).Reverse();
        }

    }
}