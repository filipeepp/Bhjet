using BHJet_Core.Enum;
using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System.Collections.Generic;


namespace BHJet_Repositorio.Admin
{
    public class ProfissionalRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca Profissionais Disponiveis
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ProfissionalDisponivelEntidade> BuscaLocalizacaoProfissionaisDisponiveis(TipoProfissional tipo)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select * from tblColaboradoresEmpresaDisponiveis
					                where bitDisponivel = 0 and
					                     geoPosicao is not null and
						                 idTipoProfissional = @TipoProfissional";

                // Execução
                return sqlConnection.Query<ProfissionalDisponivelEntidade>(query, new
                {
                    TipoProfissional = ((int)tipo)
                });
            }
        }

        /// <summary>
        /// Busca Profissional Disponivel
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public ProfissionalDisponivelEntidade BuscaLocalizacaoProfissionalDisponiveil(long idProfissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select * from tblColaboradoresEmpresaDisponiveis
					                where bitDisponivel = 0 and
					                     geoPosicao is not null and
						                 idColaboradorEmpresaSistema = @id";

                // Execução
                return sqlConnection.QueryFirstOrDefault<ProfissionalDisponivelEntidade>(query, new
                {
                    id = idProfissional,
                });
            }
        }

        /// <summary>
        /// Busca Lista de Profissionais
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ProfissionalEntidade> BuscaProfissionais()
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select PRO.idColaboradorEmpresaSistema as ID,
										PRO.vcNomeCompleto as NomeCompleto,
										TP.idTipoProfissional as TipoProfissional,
									    CASE (PRO.bitRegimeContratacaoCLT)
                                   WHEN  0 THEN 'CLT'
                                   WHEN 1 THEN 'MEI' END as TipoContrato
										from tblColaboradoresEmpresaSistema as PRO
    							   JOIN tblDOMTipoProfissional TP on (TP.idTipoProfissional = PRO.idTipoProfissional)";

                // Execução
                return sqlConnection.Query<ProfissionalEntidade>(query);
            }
        }

        /// <summary>
        /// Busca Profissional
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public ProfissionalCompletoEntidade BuscaProfissional(long idProfissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select  PRO.idColaboradorEmpresaSistema as ID,
		                                    PRO.vcNomeCompleto as NomeCompleto,
		                            		PRO.vcCPFCNPJ AS CPF,
				                            PRO.vcDocumentoHabilitacao AS CNH,
				                            PRO.vcCategoriaDocumentoHabilitacao AS TipoCNH,
				                            concat(ED.vcRua, ', ', ED.vcNumero, ' - ', ED.vcBairro, '/' ,ED.vcUF) as EnderecoCompleto,
				                            PRO.vcTelefoneResidencial AS TelefoneResidencial,
				                            PRO.vcTelefoneCelular AS TelefoneCelular,
				                            PRO.bitTelefoneCelularWhatsApp AS CelularWpp,
				                            PRO.vcEmail as Email,
				                            PRO.bitRegimeContratacaoCLT as ContratoCLT,
				                            PRO.vcObservacoes as Observacao,
				                            TP.idTipoProfissional as TipoProfissional,
				                                CASE (PRO.bitRegimeContratacaoCLT)
                                   WHEN  0 THEN 'CLT'
                                   WHEN 1 THEN 'MEI' END as TipoContrato
			                            from tblColaboradoresEmpresaSistema as PRO
		    		                    join tblDOMTipoProfissional TP on (TP.idTipoProfissional = PRO.idTipoProfissional)
		                                join tblEnderecos ED on (ED.idEndereco = pro.idEndereco)
	                               where
		                                PRO.idColaboradorEmpresaSistema = @id";

                // Execução
                return sqlConnection.QueryFirstOrDefault<ProfissionalCompletoEntidade>(query, new
                {
                    id = idProfissional
                });
            }
        }

        /// <summary>
        /// Atualiza Profissional
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public void AtualizaProfissional(ProfissionalCompletoEntidade profissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @" UPDATE dbo.tblColaboradoresEmpresaSistema
                                    SET vcNomeCompleto = @NomeCompleto,
	                                    vcCPFCNPJ = @CPF,
	                                    vcDocumentoHabilitacao = @CNH,
	                                    vcCategoriaDocumentoHabilitacao = @TipoCNH,
	                                    vcTelefoneResidencial = @TelefoneResidencial,
	                                    vcTelefoneCelular = @TelefoneCelular,
	                                    bitTelefoneCelularWhatsApp = @CelularWpp,
	                                    bitRegimeContratacaoCLT = @TipoContrato,
	                                    vcObservacoes = @Observacao,
	                                    vcEmail = @Email
                                    where 
                                        idColaboradorEmpresaSistema = @id

                                  UPDATE dbo.tblEnderecos
                                            SET vcRua = @vcrua,
                                            	vcNumero = @vcnumero,
	                                            vcComplemento = @vccomplemento,
	                                            vcBairro = @vcbairro,
	                                            vcCidade = @vccidade,
                                            	vcUF = @vcuf,
	                                            vcCEP = @vccep,
	                                            bitPrincipal = @bitprincipal
	                                        WHERE idEndereco = (SELECT * FROM tblColaboradoresEmpresaSistema WHERE idColaboradorEmpresaSistema =  @id)";

                // Execução
                sqlConnection.QueryMultiple(query, profissional);
            }
        }

    }
}
