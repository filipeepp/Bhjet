using BHJet_Repositorio.Admin.Entidade;
using Dapper;

namespace BHJet_Repositorio.Admin
{
    public class DiariaRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca Resumo de chamados concluidos e advertentes
        /// </summary>
        /// <param name="filtro">ValidaUsuarioFiltro</param>
        /// <returns>UsuarioEntidade</returns>
        public void IncluirDiariaAvulsa(DiariaAvulsaEntidade model)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"INSERT INTO [dbo].[tblRegistroDiarias]
                                               ([idCliente]
                                               ,[idTarifario]
                                               ,[idColaboradorEmpresaSistema]
                                               ,[idUsuarioSolicitacao]
                                               ,[dtDataHoraInicioExpediente]
                                               ,[intOdometroInicioExpediente]
                                               ,[dtDataHoraInicioIntervalo]
                                               ,[intOdometroInicioIntervalo]
                                               ,[dtDataHoraFimIntervalo]
                                               ,[intOdometroFimIntervalo]
                                               ,[dtDataHoraFimExpediente]
                                               ,[intOdometroFimExpediente]
                                               ,[decValorDiariaNegociado]
                                               ,[decValorDiariaComissaoNegociado]
                                               ,[decValorKMAdicionalNegociado]
                                               ,[decFranquiaKMDiaria]
                                               ,[dtDataHoraSolicitacao]
                                               ,[timHoraInicioSolicitacao]
                                               ,[timHoraFimSolicitacao]
                                               ,[bitFaturarComoDiaria])
                                         VALUES
                                               (@IDCliente
                                               ,@IDTarifario
                                               ,@IDColaboradorEmpresa
                                               ,@IDUsuarioSolicitacao
                                               ,@DataHoraInicioExpediente
                                               ,0
                                               ,null
                                               ,null
                                               ,null
                                               ,null
                                               ,@DataHoraFimExpediente
                                               ,null
                                               ,@ValorDiariaNegociado
                                               ,@ValorDiariaComissaoNegociado
                                               ,null
                                               ,null
                                               ,GETDATE()
                                               ,GETDATE()
                                               ,GETDATE()
                                               ,1)";

                sqlConnection.Execute(query, model);
            }
        }

    }
}
