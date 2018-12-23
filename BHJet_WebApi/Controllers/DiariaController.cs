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
        public IHttpActionResult PostDiariaAvulsa([FromBody]DiariaAvulsaFiltroDTO model)
        {
            // Busca Tarifa Cliente
            var tarifa = new TarifaRepositorio().BuscaTarificaPorCliente(model.IDCliente);

            // Verifica periodo informado contem varios dias
            if (model.DataFimExpediente.Date > model.DataInicioExpediente)
            {
                // Quantos dias de diarias solicitados
                double qtdDias = (model.DataFimExpediente.Date - model.DataInicioExpediente).TotalDays;

                // Monta diarias por dia
                for (int i = 0; i <= qtdDias; i++)
                {
                    // Monta periodos
                    var dataRegistro = model.DataInicioExpediente.AddDays(i);

                    // Inicio
                    var horaInicio = model.HoraInicioExpediente < new System.TimeSpan(00, 00, 00) ? new System.TimeSpan(00, 00, 00) : model.HoraInicioExpediente;
                    var dataInicio = new System.DateTime(dataRegistro.Year, dataRegistro.Month, dataRegistro.Day, horaInicio.Hours, horaInicio.Minutes, horaInicio.Seconds);

                    // Fim
                    var horaFim = model.HoraFimExpediente < new System.TimeSpan(00, 00, 00) ? new System.TimeSpan(00, 00, 00) : model.HoraFimExpediente;
                    var dataFim = new System.DateTime(dataRegistro.Year, dataRegistro.Month, dataRegistro.Day, horaFim.Hours, horaFim.Minutes, horaFim.Seconds);

                    // Busca Dados detalhados da corrida/OS
                    new DiariaRepositorio().IncluirDiariaAvulsa(new BHJet_Repositorio.Admin.Filtro.NovaDiariaAvulsaFiltro()
                    {
                        IDCliente = model.IDCliente,
                        IDTarifario = tarifa.idTarifario,
                        IDColaboradorEmpresa = model.IDColaboradorEmpresa,
                        IDUsuarioSolicitacao = UsuarioAutenticado.LoginID.ToLong(),
                        DataHoraFimExpediente = dataInicio,
                        DataHoraInicioExpediente = dataFim,
                        FranquiaKMDiaria = model.FranquiaKMDiaria,
                        ValorDiariaComissaoNegociado = model.ValorDiariaComissaoNegociado,
                        ValorDiariaNegociado = model.ValorDiariaNegociado,
                        ValorKMAdicionalNegociado = model.ValorKMAdicionalNegociado
                    });
                }
            }
            else
            {
                // Monta periodos
                model.DataInicioExpediente = new System.DateTime(model.DataInicioExpediente.Year, model.DataInicioExpediente.Month, model.DataInicioExpediente.Day, model.HoraInicioExpediente.Hours, model.HoraInicioExpediente.Minutes, model.HoraInicioExpediente.Seconds);
                model.DataFimExpediente = new System.DateTime(model.DataFimExpediente.Year, model.DataFimExpediente.Month, model.DataFimExpediente.Day, model.HoraFimExpediente.Hours, model.HoraFimExpediente.Minutes, model.HoraFimExpediente.Seconds);

                // Busca Dados detalhados da corrida/OS
                new DiariaRepositorio().IncluirDiariaAvulsa(new BHJet_Repositorio.Admin.Filtro.NovaDiariaAvulsaFiltro()
                {
                    IDCliente = model.IDCliente,
                    IDTarifario = tarifa.idTarifario,
                    IDColaboradorEmpresa = model.IDColaboradorEmpresa,
                    IDUsuarioSolicitacao = UsuarioAutenticado.LoginID.ToLong(),
                    DataHoraFimExpediente = model.DataFimExpediente,
                    DataHoraInicioExpediente = model.DataInicioExpediente,
                    FranquiaKMDiaria = model.FranquiaKMDiaria,
                    ValorDiariaComissaoNegociado = model.ValorDiariaComissaoNegociado,
                    ValorDiariaNegociado = model.ValorDiariaNegociado,
                    ValorKMAdicionalNegociado = model.ValorKMAdicionalNegociado
                });
            }

            // Return
            return Ok();
        }
    }
}