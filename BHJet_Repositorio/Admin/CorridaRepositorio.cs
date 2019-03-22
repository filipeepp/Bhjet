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
								    left join tblEnderecosCorrida as EC on (CD.idCorrida = CD.idCorrida)
							  where cd.idStatusCorrida = @StatusCorrida
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
								    left join tblColaboradoresEmpresaSistema as CLB on (CD.idUsuarioColaboradorEmpresa = CLB.idColaboradorEmpresaSistema)
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
        public CorridaEncontradaEntidade BuscaCorridaAberta(long colaborador, int tipo)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    // Query
                    string query = @"select top(1) CD.idCorrida as ID,
							        CD.decValorComissaoNegociado as Comissao,
							        E.vcRua + ' - ' + E.vcNumero + ', ' + E.vcBairro + ' / ' + E.vcCidade as EnderecoCompleto,
                                    C.vcNomeFantasia as NomeCliente
							     from tblCorridas CD
								     join tblLogCorrida LGCD on (CD.idCorrida = LGCD.idCorrida)
								    left join tblColaboradoresEmpresaSistema as CLB on (CD.idUsuarioColaboradorEmpresa = CLB.idColaboradorEmpresaSistema)
								    join tblEnderecosCorrida as EC on (CD.idCorrida = CD.idCorrida)
								    join tblEnderecos as E on (EC.idEndereco = e.idEndereco)
                                    join tblClientes as C on (CD.idCliente = C.idCliente)
                                    left join tblCorridasRecusadas as CR on (CR.idCorrida = CD.idCorrida )
								 where CD.idStatusCorrida = 3
										AND (CD.idTipoProfissional = @tp or CD.idTipoProfissional is null)
                                        AND (CD.idUsuarioColaboradorEmpresa IS NULL or CD.idUsuarioColaboradorEmpresa = @profissional)
                                        AND (CR.idCorrida IS NULL OR  CR.idCorrida != CD.idCorrida)
										order by CD.dtDataHoraRegistroCorrida DESC";

                    // Execução
                    var corrida = trans.Connection.QueryFirstOrDefault<CorridaEncontradaEntidade>(query, new
                    {
                        tp = tipo,
                        profissional = colaborador
                    }, trans);

                    // Trava corrida temporariamente
                    if (corrida != null)
                    {
                        string queryAceitaTemp = @"update tblCorridas set idStatusCorrida  = 1, 
                                                                      idUsuarioColaboradorEmpresa = @idCol
                                                                where idCorrida = @corrida";
                        trans.Connection.Execute(queryAceitaTemp, new
                        {
                            idCol = colaborador,
                            corrida = corrida.ID
                        }, trans);
                    }

                    // Commit
                    trans.Commit();

                    // Return
                    return corrida;
                }
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
        public IEnumerable<StatusEntidade> BuscaStatusCorrida()
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select * from tblDOMStatusCorrida";

                // Execução
                return sqlConnection.Query<StatusEntidade>(query);
            }
        }

        /// <summary>
        /// Busca Ocorrencias
        /// </summary>
        /// <returns>OcorrenciaEntidade</returns>
        public void AtualizaStatusCorrida(int ocorrencia, long idLogCorrida, long idCorrida)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Query
                        string query = @"update tblCorridas set idStatusCorrida = @status where idCorrida = @id";

                        // Execução
                        trans.Connection.Execute(query, new
                        {
                            id = idCorrida,
                            status = ocorrencia
                        }, trans);

                        // Query
                        query = @"update tblLogCorrida set idStatusCorrida = @status where idCorrida = @id";

                        // Execução
                        trans.Connection.Execute(query, new
                        {
                            id = idCorrida,
                            status = ocorrencia
                        }, trans);

                        // Commit
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Encerrar Ordem Servico
        /// </summary>
        /// <param name="idCorrida"></param>
        public void EncerrarOrdemServico(long idCorrida, int? idOcorrencia, int kmPercorrido, int MinutosParados)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {

                //--busca tipo profissional
                string queryTipo = @"select idTipoProfissional as Tipo, (SELECT count(*) as QuantidadePontos FROM tblLogCorrida where idCorrida = @id) from tblCorridas C
			                        where C.idCorrida = @id";

                var tipoPro = sqlConnection.QueryFirstOrDefault<LogCorrida>(queryTipo, new
                {
                    id = idCorrida
                });

                //-- busca tarifa padrao
                string queryTarifario = @"select * from tblTarifario where idTipoServico = 2 and idTipoVeiculo = @tipo";

                var tarifario = sqlConnection.QueryFirstOrDefault<TarifarioEntidade>(queryTarifario, new
                {
                    tipo = tipoPro.Tipo == TipoProfissional.Motociclista ? 1 : 2
                });

                var VB1 = tarifario.decValorContrato;
                var VB2 = kmPercorrido > tarifario.intFranquiaKM ? (kmPercorrido - tarifario.intFranquiaKM ?? 0) * tarifario.decValorKMAdicional : 0;
                var VB3 = tipoPro.QuantidadePontos > 1 ? tipoPro.QuantidadePontos-- * (tarifario.decValorPontoExcedente ?? 0) : 0;
                var VB4 = MinutosParados > tarifario.intFranquiaMinutosParados ? (MinutosParados - tarifario.intFranquiaMinutosParados ?? 0) * tarifario.decValorMinutoParado : 0;

                // Valor finalizado
                var valorFinal = VB1 + VB2 + VB3 + VB4;

                // Query
                string query = @"update tblCorridas set idStatusCorrida    = @status, 
			            	                            dtDataHoraTermino  = getdate(),
							                            decValorFinalizado = @valor,
                                                        intKMPercorrido = @km
							                      where idCorrida = @id";

                // Execução
                sqlConnection.Execute(query, new
                {
                    id = idCorrida,
                    status = (idOcorrencia != null) ? idOcorrencia : 11,
                    valor = valorFinal,
                    km = kmPercorrido
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
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Query recusar
                        string query = @"INSERT INTO [tblCorridasRecusadas] VALUES (@corrida, @profissional)";
                        trans.Connection.Execute(query, new
                        {
                            corrida = idCorrida,
                            profissional = profissional
                        }, trans);

                        // Query STATUS
                        string queryCorrida = @"update tblCorridas set idStatusCorrida  = 3, idUsuarioColaboradorEmpresa = null
                                                                 where idCorrida = @id";
                        trans.Connection.Execute(queryCorrida, new
                        {
                            id = idCorrida
                        }, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///  Liberar Ordem Servico
        /// </summary>
        /// <param name="idCorrida"></param>
        public void LiberarOrdemServico(long idCorrida, long profissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query STATUS
                string queryCorrida = @"update tblCorridas set idUsuarioColaboradorEmpresa = @prof, idStatusCorrida  = 3 where idCorrida = @id";
                // Executa
                sqlConnection.Execute(queryCorrida, new
                {
                    prof = profissional,
                    id = idCorrida
                });

            }
        }

        /// <summary>
        /// Busca Ocorrencias
        /// </summary>
        /// <returns>OcorrenciaEntidade</returns>
        public IEnumerable<OcorrenciaEntidade> BuscaOcorrenciaCorrida()
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select * from tblDOMTipoOcorrenciaCorrida";

                // Execução
                return sqlConnection.Query<OcorrenciaEntidade>(query);
            }
        }
    }

    public struct LogCorrida
    {
        public TipoProfissional Tipo { get; set; }
        public int QuantidadePontos { get; set; }
    }
}
