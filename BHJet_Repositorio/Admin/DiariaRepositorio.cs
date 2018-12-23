using BHJet_Repositorio.Admin.Filtro;
using Dapper;

namespace BHJet_Repositorio.Admin
{
    public class DiariaRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca Resumo de chamados concluidos e advertentes
        /// </summary>
        /// <param name="filtro">NovaDiariaAvulsaFiltro</param>
        /// <returns>UsuarioEntidade</returns>
        public void IncluirDiariaAvulsa(NovaDiariaAvulsaFiltro model)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"INSERT INTO [dbo].[tblRegistroDiarias]
                                               ([idCliente]
                                               ,[idTarifario]
                                               ,[idColaboradorEmpresaSistema]
                                               ,[idUsuarioSolicitacao]
                                               ,dtDataHoraInicioExpediente
											   ,dtDataHoraFimExpediente
											   ,decValorDiariaNegociado
											   ,decValorDiariaComissaoNegociado
											   ,decValorKMAdicionalNegociado
											   ,decFranquiaKMDiaria
											  ,dtDataHoraSolicitacao
											  ,timHoraInicioSolicitacao
											  ,timHoraFimSolicitacao, bitFaturarComoDiaria)
                                         VALUES
										  (@IDCliente
                                               ,@IDTarifario
                                               ,@IDColaboradorEmpresa
                                               ,@IDUsuarioSolicitacao
                                               ,@DataHoraInicioExpediente
                                               ,@DataHoraFimExpediente
                                               ,@ValorDiariaNegociado
                                               ,@ValorDiariaComissaoNegociado
                                               ,@ValorKMAdicionalNegociado
                                               ,@FranquiaKMDiaria
                                               ,GETDATE()
                                               ,GETDATE()
                                               ,GETDATE()
                                               ,1)";

                sqlConnection.Execute(query, model);
            }
        }

    }
}
