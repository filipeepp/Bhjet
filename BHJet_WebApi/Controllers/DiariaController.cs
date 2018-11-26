using BHJet_Core.Extension;
using BHJet_DTO.Diaria;
using BHJet_Repositorio.Admin;
using BHJet_WebApi.Util;
using System.Web.Http;

namespace BHJet_WebApi.Controllers
{
    [RoutePrefix("api/Diaria")]
    public class DiariaController : ApiController
    {
        private UsuarioLogado _usuarioAutenticado;

        /// <summary>
        /// Informações do usuário autenticado
        /// </summary>
        public UsuarioLogado UsuarioAutenticado
        {
            get
            {
                if (_usuarioAutenticado == null)
                    _usuarioAutenticado = new UsuarioLogado();

                return _usuarioAutenticado;
            }
        }

        /// <summary>
        /// Método para inclusão de diaria avulsa
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public IHttpActionResult PostDiariaAvulsa([FromBody]DiariaAvulsaDTO model)
        {
            // Busca Dados detalhados da corrida/OS
            new DiariaRepositorio().IncluirDiariaAvulsa(new BHJet_Repositorio.Admin.Entidade.DiariaAvulsaEntidade()
            {
                IDCliente = model.IDCliente,
                IDColaboradorEmpresa = model.IDColaboradorEmpresa,
                IDTarifario = model.IDTarifario,
                DataHoraFimExpediente = model.DataHoraFimExpediente,
                DataHoraFimIntervalo = model.DataHoraFimIntervalo,
                DataHoraInicioExpediente = model.DataHoraInicioExpediente,
                DataHoraInicioIntervalo = model.DataHoraInicioIntervalo,
                DataHoraSolicitacao = model.DataHoraSolicitacao,
                FaturarComoDiaria = model.FaturarComoDiaria,
                FranquiaKMDiaria = model.FranquiaKMDiaria,
                IDUsuarioSolicitacao = UsuarioAutenticado.LoginID.ToLong(),
                OdometroFimExpediente = model.OdometroFimExpediente,
                OdometroInicioExpediente = model.OdometroInicioExpediente,
                OdometroInicioIntervalo = model.OdometroInicioIntervalo,
                ValorDiariaComissaoNegociado = model.ValorDiariaComissaoNegociado,
                ValorDiariaNegociado = model.ValorDiariaNegociado,
                ValorKMAdicionalNegociado = model.ValorKMAdicionalNegociado
            });

            // Return
            return Ok();
        }
    }
}