using BHJet_Repositorio.Admin.Entidade;
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

        /// <summary>
        /// Veriifica se a diaria foi aberta
        /// </summary>
        /// <param name="idProfissional"></param>
        /// <returns></returns>
        public bool VerificaDiariaAberta(long idProfissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select 
		                            cast(count(intOdometroInicioExpediente) as bit) as DiariaAberta
	                            from tblRegistroDiarias
		                        where idColaboradorEmpresaSistema = 13
		                            and convert(varchar(10), dtDataHoraInicioExpediente, 120)  = convert(varchar(10), getdate(), 120)";

                return sqlConnection.QueryFirstOrDefault<bool>(query, new
                {
                    id = idProfissional
                });
            }
        }

        /// <summary>
        /// Busca dados do turno da diaria
        /// </summary>
        /// <param name="idProfissional"></param>
        /// <returns></returns>
        public TurnoEntidade BuscaDadosTurno(long idProfissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select dtDataHoraInicioExpediente as DataInicio,
		                                intOdometroInicioExpediente as KMInicio,
				                        dtDataHoraInicioExpediente as DataInicioIntervalo,
				                        intOdometroInicioExpediente as KMInicioIntervalo,
				                        dtDataHoraFimIntervalo as DataFimIntervalo,
                                        intOdometroFimIntervalo as KMFimInvervalo,
				                        dtDataHoraFimExpediente as DataFim,
				                        intOdometroFimExpediente as KMFim
	 		                        from tblRegistroDiarias
				                        where idColaboradorEmpresaSistema = 13
		     		                and convert(varchar(10), dtDataHoraInicioExpediente, 120)  = convert(varchar(10), getdate(), 120)";

                return sqlConnection.QueryFirstOrDefault<TurnoEntidade>(query, new
                {
                    id = idProfissional
                });
            }
        }

        /// <summary>
        /// Cadastra dados do turno da diaria
        /// </summary>
        /// <param name="filtro">TurnoEntidade</param>
        /// <returns></returns>
        public void CadastraDadosTurno(TurnoEntidade filtro, long idProfissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"update tblRegistroDiarias set
		                            dtDataHoraInicioExpediente = @DataInicio,
		                            intOdometroInicioExpediente = @KMInicio,
		                            dtDataHoraInicioExpediente = @DataInicioIntervalo,
		                            intOdometroInicioExpediente = @KMInicioIntervalo,
		                            dtDataHoraFimIntervalo = @DataFimIntervalo,
		                            intOdometroFimIntervalo = @KMFimInvervalo,    
		                            dtDataHoraFimExpediente = @DataFim,
		                            intOdometroFimExpediente = @KMFim
	                        where idColaboradorEmpresaSistema = @IDProfissional
		                          and convert(varchar(10), dtDataHoraInicioExpediente, 120)  = convert(varchar(10), getdate(), 120)";

                sqlConnection.Execute(query, filtro);
            }
        }
    }
}
