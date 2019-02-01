using BHJet_Enumeradores;
using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System.Collections.Generic;

namespace BHJet_Repositorio.Admin
{
    public class CorridaRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca Profissionais Disponiveis
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<LocalizacaoCorridaEntidade> BuscaLocalizacaoCorrida(StatusCorrida status, TipoProfissional tipo)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select CD.idCorrida ,
							EC.geoPosicao.STY  as vcLatitude, 
							EC.geoPosicao.STX  as vcLongitude
							 from tblCorridas CD
								join tblLogCorrida LGCD on (CD.idCorrida = LGCD.idCorrida)
								join tblColaboradoresEmpresaSistema as CLB on (CD.idUsuarioColaboradorEmpresa = CLB.idColaboradorEmpresaSistema)
								join tblEnderecosCorrida as EC on (CD.idCorrida = CD.idCorrida)
									where LGCD.idStatusCorrida = @StatusCorrida
										AND CD.idTipoProfissional = @TipoProfissional";

                // Execução
                return sqlConnection.Query<LocalizacaoCorridaEntidade>(query, new
                {
                    StatusCorrida = ((int)status),
                    TipoProfissional = ((int)tipo)
                });
            }
        }

        /// <summary>
        /// Busca Profissionais Disponiveis
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<OSCorridaEntidade> BuscaDetalheCorrida(int idCorrida)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select CD.idCorrida as NumeroOS, 
	                                CD.idUsuarioChamador as IDCliente,
		                            CD.idUsuarioColaboradorEmpresa as IDProfissional,
	                                concat(EDC.vcRua, ', ', EDC.vcNumero, ' - ', EDC.vcBairro, '/' ,EDC.vcUF) as EnderecoCompleto,
		                            EC.vcPessoaContato as ProcurarPor,
		                            LGCD.idStatusCorrida as StatusCorrida,
	                                EC.bitColetarAssinatura,
									EC.bitEntregarDocumento,
									EC.bitEntregarObjeto,
									EC.bitRetirarDocumento,
									EC.bitRetirarObjeto,
		                            EC.dtHoraChegada - EC.dtHoraAtendido as TempoEspera,
                                    EC.vcObservacao AS Observacao,
                                    PT.vcCaminhoProtocolo as CaminhoProtocolo
							    from tblCorridas CD
								    join tblColaboradoresEmpresaSistema as CLB on (CD.idUsuarioColaboradorEmpresa = CLB.idColaboradorEmpresaSistema)
								    left join tblLogCorrida LGCD on (CD.idCorrida = LGCD.idCorrida)
								    left join tblEnderecosCorrida as EC on (CD.idCorrida = CD.idCorrida)
								    left join tblEnderecos EDC on (EC.idCorrida = edc.idEndereco)
                                    left join tblProtocoloEnderecoCorrida PT on (EC.idCorrida = PT.idEnderecoCorrida)
						        where CD.idCorrida = @id";

                // Execução
                return sqlConnection.Query<OSCorridaEntidade>(query, new
                {
                    id = idCorrida,
                });
            }
        }

		/// <summary>
		/// Busca Profissionais Disponiveis
		/// </summary>
		/// <param name="filtro">TipoProfissional</param>
		/// <returns>UsuarioEntidade</returns>
		public IEnumerable<OSCorridaEntidade> BuscaDetalheCorridaCliente(long clienteID)
		{
			using (var sqlConnection = this.InstanciaConexao())
			{
				// Query
				string query = @"SELECT
									Corrida.idCorrida AS NumeroOS,
									Corrida.dtDataHoraInicio AS DataHoraInicio,
									Corrida.decValorFinalizado AS ValorFinalizado,
									Profissional.vcNomeCompleto AS NomeProfissional
								FROM
									tblCorridas Corrida
								INNER JOIN
									tblColaboradoresEmpresaSistema Profissional ON Profissional.idColaboradorEmpresaSistema = Corrida.idUsuarioColaboradorEmpresa
								WHERE 
									Corrida.idCliente = @ClienteID";

				// Execução
				return sqlConnection.Query<OSCorridaEntidade>(query, new
				{
					ClienteID = clienteID,
				});
			}
		}

        /// <summary>
        /// Busca Corrida Aberta
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public CorridaEncontradaEntidade BuscaCorridaAberta(long colaborador,int tipo)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select top(1) CD.idCorrida as ID,
							        CD.decValorComissaoNegociado as Comissao,
							        E.vcRua + ' - ' + E.vcNumero + ', ' + E.vcBairro + ' / ' + E.vcCidade as EnderecoCompleto,
                                    C.vcNomeFantasia as NomeCliente
							     from tblCorridas CD
								    join tblLogCorrida LGCD on (CD.idCorrida = LGCD.idCorrida)
								    join tblColaboradoresEmpresaSistema as CLB on (CD.idUsuarioColaboradorEmpresa = CLB.idColaboradorEmpresaSistema)
								    join tblEnderecosCorrida as EC on (CD.idCorrida = CD.idCorrida)
								    join tblEnderecos as E on (EC.idEndereco = e.idEndereco)
                                    join tblClientes as C on (CD.idCliente = C.idCliente)
                               left join tblCorridasRecusadas as CR on (CR.idCorrida = CD.idCorrida )
								 where LGCD.idStatusCorrida in (select idStatusCorrida from tblDOMStatusCorrida where bitCancela = 0 and bitFinaliza = 0)
										AND (CD.idTipoProfissional = @tp or CD.idTipoProfissional is null)
                                        AND CR.idColaboradorEmpresaSistema IS NULL or (CR.idColaboradorEmpresaSistema != @profissional AND cr.idCorrida != cd.idCorrida)
										order by CD.dtDataHoraRegistroCorrida DESC";

                // Execução
                return sqlConnection.QueryFirstOrDefault<CorridaEncontradaEntidade>(query, new
                {
                    tp = tipo,
                    profissional = colaborador
                });
            }
        }

        /// <summary>
        /// Busca LOG Corrida
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<LogCorridaEntidade> BuscaLogCorrida(long idCorrida)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select
                                        LC.idCorrida,
                                        LC.idStatusCorrida as Status,
                                        EC.idEnderecoCorrida,
                                        EC.dtHoraChegada,
                                        ED.vcRua + ' - ' + ED.vcNumero + ', ' + ED.vcBairro + ' / ' + ED.vcCidade as EnderecoCompleto,
                                        EC.vcPessoaContato,
                                        EC.vcObservacao,
                                        EC.bitEntregarDocumento,
                                        EC.bitColetarAssinatura,
                                        EC.bitRetirarDocumento,
                                        EC.bitRetirarObjeto,
                                        EC.bitEntregarObjeto,
                                        EC.bitOutros,
                                        LC.geoPosicao.STY  as vcLatitude, 
	                                    LC.geoPosicao.STX  as vcLongitude,
                                       PEC.vcCaminhoProtocolo as Foto
				                        -- TELEFONE NAO TEM
                                  from tblLogCorrida LC
                                  join tblEnderecosCorrida EC on (LC.idCorrida = EC.idCorrida)
                                  join tblEnderecos ED on(EC.idEndereco = ED.idEndereco)
                                  left join tblProtocoloEnderecoCorrida PEC on (PEC.idEnderecoCorrida = EC.idEnderecoCorrida)
                                 where LC.idCorrida = @id";

                // Execução
                return sqlConnection.Query<LogCorridaEntidade>(query, new
                {
                    id = idCorrida
                });
            }
        }

        /// <summary>
        /// Insere registro de protocolo
        /// </summary>
        /// <param name="protocolo"></param>
        /// <param name="idEnderecoCorrida"></param>
        public void InsereRegistroProtocolo(byte[] protocolo, long idEnderecoCorrida)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"INSERT INTO [dbo].[tblProtocoloEnderecoCorrida]
                                                   ([idEnderecoCorrida]
                                                   ,[vcCaminhoProtocolo])
                                      VALUES
                                                   (@idEndCorrida
                                                   ,@fotoProtocolo)";

                // Execução
                sqlConnection.Execute(query, new
                {
                    idEndCorrida = idEnderecoCorrida,
                    fotoProtocolo = protocolo
                });
            }
        }

        /// <summary>
        /// Altera status corrida
        /// </summary>
        /// <param name="idStatus"></param>
        /// <param name="idCorrida"></param>
        public void RegistraChegadaEndereco(long idEnderecoCorrida)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"update tblEnderecosCorrida set dtHoraChegada = GETDATE() where idEnderecoCorrida = @id";

                // Execução
                sqlConnection.Execute(query, new
                {
                    id = idEnderecoCorrida
                });
            }
        }

        /// <summary>
        /// Busca Ocorrencias
        /// </summary>
        /// <returns>OcorrenciaEntidade</returns>
        public IEnumerable<OcorrenciaEntidade> BuscaOcorrenciasCorrida()
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select * from tblDOMStatusCorrida";

                // Execução
                return sqlConnection.Query<OcorrenciaEntidade>(query);
            }
        }

        /// <summary>
        /// Busca Ocorrencias
        /// </summary>
        /// <returns>OcorrenciaEntidade</returns>
        public void AtualizaOcorrenciasCorrida(int ocorrencia, long idCorrida)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"update tblCorridas set idStatusCorrida = @status where idCorrida = @id";

                // Execução
                sqlConnection.Execute(query, new
                {
                    id = idCorrida,
                    idStatusCorrida = ocorrencia
                });
            }
        }

        /// <summary>
        /// Encerrar Ordem Servico
        /// </summary>
        /// <param name="idCorrida"></param>
        public void EncerrarOrdemServico(long idCorrida, int? idOcorrencia, long kmPercorrido)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query tarifas
                string queryTarifa = @"	select C.decValorNegociado +  (T.decValorKMAdicionalCorrida * @kmRodado)
			                              from tblCorridas C
				                          join tblTarifario T on (C.idTarifario = t.idTarifario)
						                 where idCorrida = @id";

                // Execução
                var valorFinalizado = sqlConnection.QueryFirstOrDefault<decimal>(queryTarifa, new
                {
                    kmRodado = kmPercorrido,
                    id = idCorrida
                });

                // Query
                string query = @"update tblCorridas set idStatusCorrida    = @status, 
			            	                            dtDataHoraTermino  = getdate(),
							                            decValorFinalizado = @valor
							                      where idCorrida = @id";

                // Execução
                sqlConnection.Execute(query, new
                {
                    id = idCorrida,
                    status = (idOcorrencia != null) ? idOcorrencia : 11,
                    valor = valorFinalizado
                });
            }
        }

        /// <summary>
        /// RECUSAR Ordem Servico
        /// </summary>
        /// <param name="idCorrida"></param>
        public void RecusarOrdemServico(long idCorrida, long profissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"INSERT INTO [tblCorridasRecusadas] VALUES (@corrida, @profissional)";

                // Execução
                sqlConnection.Execute(query, new
                {
                    corrida = idCorrida,
                    profissional = profissional
                });
            }
        }
    }
}
