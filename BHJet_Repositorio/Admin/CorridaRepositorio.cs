﻿using BHJet_Core.Enum;
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
										AND CLB.idTipoProfissional = @TipoProfissional";

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
	}
}
