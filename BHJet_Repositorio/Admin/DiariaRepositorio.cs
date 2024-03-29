﻿using BHJet_Repositorio.Admin.Entidade;
using BHJet_Repositorio.Admin.Filtro;
using Dapper;
using System;
using System.Collections.Generic;

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
		                        where idColaboradorEmpresaSistema = @id
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
        public TurnoEntidade BuscaDadosTurno(long idDiaria)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select CONVERT(VARCHAR(5),dtDataHoraInicioExpediente,108) as DataInicio,
		                                intOdometroInicioExpediente as KMInicio,
				                        CONVERT(VARCHAR(5),dtDataHoraInicioIntervalo,108) as DataInicioIntervalo,
				                        intOdometroInicioIntervalo as KMInicioIntervalo,
				                        CONVERT(VARCHAR(5),dtDataHoraFimIntervalo,108)  as DataFimIntervalo,
                                        intOdometroFimIntervalo as KMFimInvervalo,
				                        CONVERT(VARCHAR(5),dtDataHoraFimExpediente,108)  as DataFim,
				                        intOdometroFimExpediente as KMFim,
										CLI.vcNomeFantasia AS NomeCliente,
										EN.vcRua + ', ' + EN.vcNumero + ', ' + EN.vcBairro  + ' - ' + EN.vcCidade + '/' + en.vcUF AS EnderecoCliente
	 		                        from tblRegistroDiarias RD 
							   left join tblClientes CLI on (RD.idCliente = cli.idCliente)
							   left join tblEnderecos EN on (CLI.idEndereco = EN.idEndereco)
				                        where idRegistroDiaria = @id";

                return sqlConnection.QueryFirstOrDefault<TurnoEntidade>(query, new
                {
                    id = idDiaria
                });
            }
        }

        /// <summary>
        /// Cadastra dados do turno da diaria
        /// </summary>
        /// <param name="filtro">TurnoEntidade</param>
        /// <returns></returns>
        public void CadastraDadosTurno(TurnoEntidade filtro, long idDiaria)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"update tblRegistroDiarias set
		                            dtDataHoraInicioExpediente = @DataInicio,
		                            intOdometroInicioExpediente = @KMInicio,
		                            dtDataHoraInicioIntervalo = @DataInicioIntervalo,
		                            intOdometroInicioIntervalo = @KMInicioIntervalo,
		                            dtDataHoraFimIntervalo = @DataFimIntervalo,
		                            intOdometroFimIntervalo = @KMFimInvervalo,    
		                            dtDataHoraFimExpediente = @DataFim,
		                            intOdometroFimExpediente = @KMFim
	                        where idRegistroDiaria = @idTurnoDiaria";

                sqlConnection.Execute(query, new
                {
                    DataInicio = BuscaDataTurno(filtro.DataInicio),
                    KMInicio = filtro.KMInicio,
                    DataInicioIntervalo = BuscaDataTurno(filtro.DataInicioIntervalo),
                    KMInicioIntervalo = filtro.KMInicioIntervalo,
                    DataFimIntervalo = BuscaDataTurno(filtro.DataFimIntervalo),
                    KMFimInvervalo = filtro.KMFimInvervalo,
                    DataFim = BuscaDataTurno(filtro.DataFim),
                    KMFim = filtro.KMFim,
                    idTurnoDiaria = idDiaria
                });
            }
        }

        private DateTime? BuscaDataTurno(string horaTurno)
        {
            if (string.IsNullOrWhiteSpace(horaTurno))
                return null;
            else
            {
                // Hora e minuto
                var hora = int.Parse(horaTurno.Split(':')[0]);
                var minuto = int.Parse(horaTurno.Split(':')[1]);
                // Data
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hora, minuto, 00);
            }
        }


        /// <summary>
        /// Busca diarias cliente
        /// </summary>
        /// <param name="idCLiente"></param>
        /// <returns></returns>
        public IEnumerable<DiariaAvulsaEntidade> BuscaDiariasCliente(long idCLiente)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select RD.idRegistroDiaria AS idRegistroDiaria,
	                                    RD.dtDataHoraSolicitacao AS DataHoraSolicitacao,
	                                    CE.vcNomeCompleto as vcNomeCompleto,
                            	        RD.decValorDiariaNegociado as ValorDiariaNegociado from tblRegistroDiarias RD
				              left join tblColaboradoresEmpresaSistema CE on (RD.idColaboradorEmpresaSistema = ce.idColaboradorEmpresaSistema) 
                                  WHERE idCliente = @id";

                return sqlConnection.Query<DiariaAvulsaEntidade>(query, new
                {
                    id = idCLiente
                });
            }
        }

    }
}
