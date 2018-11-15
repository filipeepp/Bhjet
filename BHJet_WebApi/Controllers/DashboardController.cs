using BHJet_DTO.Dashboard;
using BHJet_Repositorio.Admin;
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


    }
}