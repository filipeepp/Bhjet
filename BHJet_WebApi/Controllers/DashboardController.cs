using BHJet_Core.Enum;
using BHJet_DTO.Dashboard;
using BHJet_Repositorio.Admin;
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

            // valida retorno
            if (entidade != null && !entidade.Any())
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            // Separa chamados 
            List<ResumoChamadoModel> ResumoMensal = new List<ResumoChamadoModel>();

            // Percorre os meses
            for (int i = 1; i <= 12; i++)
            {
                ResumoMensal.Add(new ResumoChamadoModel()
                {
                    Mes = i,
                    ChamadosAdvertentes = entidade.Where(x => x.DataRegistro?.Month == i &&
                                                             (x.Status == StatusCorrida.ClienteCancelou ||
                                                              x.Status == StatusCorrida.ProblemasNoVeiculo ||
                                                              x.Status == StatusCorrida.PessoaAusente))?.Count() ?? 0,
                    ChamadosConcluidos = entidade.Where(x => x.Status == StatusCorrida.Concluida && x.DataRegistro?.Month == i)?.Count() ?? 0
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
            for (int i = 1; i <= 12; i++)
            {
                ResumoMensal.Add(new ResumoAtendimentoModel()
                {
                    Mes = i,
                    QtdAtendimentoMotociclista  = entidade.Where(x => x.DataRegistro?.Month == i && x.TipoProfissional == TipoProfissional.Motociclista)?.Count() ?? 0,
                    QtdAtendimentoMotorista  = entidade.Where(x => x.DataRegistro?.Month == i && x.TipoProfissional == TipoProfissional.Motorista)?.Count() ?? 0,
                });
            }

            // Return
            return Ok(ResumoMensal);
        }



    }
}