using BHJet_Core.Enum;
using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System;
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
                string query = @"select * from tblColaboradoresEmpresaDisponiveis CED
                                        join tblColaboradoresEmpresaSistema CE ON(CED.idColaboradorEmpresaSistema = ce.idColaboradorEmpresaSistema)
					                where CED.bitDisponivel = 0 and
					                     CED.geoPosicao is not null and
						                 CED.idTipoProfissional = @TipoProfissional";

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
                string query = @"select * from tblColaboradoresEmpresaDisponiveis CED
                                        join tblColaboradoresEmpresaSistema CE ON(CED.idColaboradorEmpresaSistema = ce.idColaboradorEmpresaSistema)
					                where CED.bitDisponivel = 0 and
					                     CED.geoPosicao is not null and
						                 CED.idColaboradorEmpresaSistema = @id";

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
        public IEnumerable<ProfissionalEntidade> BuscaProfissionais(string trecho)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select PRO.idColaboradorEmpresaSistema as ID,
										PRO.vcNomeCompleto as NomeCompleto,
										TP.idTipoProfissional as TipoProfissional,
									    PRO.bitRegimeContratacaoCLT TipoRegime
										from tblColaboradoresEmpresaSistema as PRO
    							   JOIN tblDOMTipoProfissional TP on (TP.idTipoProfissional = PRO.idTipoProfissional)
                                     where convert(varchar(250), PRO.idColaboradorEmpresaSistema) like @valorPesquisa
									       or
									       PRO.vcNomeCompleto like @valorPesquisa";

                if (string.IsNullOrWhiteSpace(trecho))
                    query.Replace("*", "top(50)");

                // Execução
                return sqlConnection.Query<ProfissionalEntidade>(query, new
                {
                    valorPesquisa = "%" + trecho + "%",
                });
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
                                            ED.vcCEP as Cep,
				                            ED.vcRua AS Rua,
                                            ED.vcNumero AS RuaNumero,
											ED.vcComplemento as Complemento,
											ED.vcBairro as Bairro,
											ED.vcCidade AS Cidade,
											ED.vcUF as UF,
											ED.vcPontoDeReferencia as PontoReferencia,
											ED.bitPrincipal as EnderecoPrincipal,
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
        /// <param name="filtro">ProfissionalCompletoEntidade</param>
        /// <returns>UsuarioEntidade</returns>
        public void AtualizaProfissional(ProfissionalCompletoEntidade profissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Update tblColaboradoresEmpresaSistema
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
                                        idColaboradorEmpresaSistema = @ID";
                        // Execução 
                        trans.Connection.ExecuteScalar(query, new
                        {
                            NomeCompleto = profissional.NomeCompleto,
                            CPF = profissional.CPF,
                            CNH = profissional.CNH,
                            TipoCNH = profissional.TipoCNH,
                            TelefoneResidencial = profissional.TelefoneResidencial,
                            TelefoneCelular = profissional.TelefoneCelular,
                            CelularWpp = profissional.CelularWpp,
                            TipoContrato = profissional.TipoRegime,
                            Observacao = profissional.Observacao,
                            Email = profissional.Email,
                            ID = profissional.ID
                        }, trans);

                        // Insert Novo Endereco
                        query = @"UPDATE [dbo].[tblEnderecos]
   	                                    SET [vcRua] = @Rua
      	                                    ,[vcNumero] = @RuaNumero
      	                                    ,[vcComplemento] = @Complemento
      	                                    ,[vcBairro] = @Bairro
	                                        ,[vcCidade] = @Cidade
      	                                    ,[vcUF] = @UF
      	                                    ,[vcCEP] = @Cep
      	                                    ,[vcPontoDeReferencia] = @PontoReferencia
      	                                    ,[bitPrincipal] = @EnderecoPrincipal
 	                                    WHERE idEndereco = (select idEndereco from tblColaboradoresEmpresaSistema where idColaboradorEmpresaSistema = @id)";
                        // Execute
                        var idEndereco = trans.Connection.Query<int>(query, profissional, trans);
                        
                        // Commit
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw e;
                    }
                }   
            }
        }



        /// <summary>
        /// Inclui Profissional
        /// </summary>
        /// <param name="filtro">ProfissionalCompletoEntidade</param>
        /// <returns>UsuarioEntidade</returns>
        public void IncluirProfissional(ProfissionalCompletoEntidade profissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Insert Novo Endereco
                        string query = @"INSERT INTO [dbo].[tblEnderecos]
                                           ([vcRua]
                                           ,[vcNumero]
                                           ,[vcComplemento]
                                           ,[vcBairro]
                                           ,[vcCidade]
                                           ,[vcUF]
                                           ,[vcCEP]
                                           ,[vcPontoDeReferencia]
                                           ,[bitPrincipal])
                                     VALUES
                                           (@Rua
                                           ,@RuaNumero
                                           ,@Complemento
                                           ,@Bairro
                                           ,@Cidade
                                           ,@UF
                                           ,@Cep
                                           ,@PontoReferencia
                                           ,@EnderecoPrincipal)
                                           select @@identity;";
                        // Execute
                        var idEndereco = trans.Connection.ExecuteScalar<int?>(query, new
                        {
                            Rua = profissional.Rua,
                            RuaNumero = profissional.RuaNumero,
                            Complemento = profissional.Complemento,
                            Bairro = profissional.Bairro,
                            Cidade = profissional.Cidade,
                            UF = profissional.UF,
                            Cep = profissional.Cep,
                            PontoReferencia = profissional.PontoReferencia,
                            EnderecoPrincipal = profissional.EnderecoPrincipal
                        }, trans);


                        // Update tblColaboradoresEmpresaSistema
                         query = @" INSERT INTO [dbo].[tblColaboradoresEmpresaSistema]
                                                     ([idUsuario]
                                                     ,[idEndereco]
                                                     ,[idTipoProfissional]
                                                     ,[vcNomeCompleto]
                                                     ,[vcCPFCNPJ]
                                                     ,[vcDocumentoHabilitacao]
                                                     ,[vcCategoriaDocumentoHabilitacao]
                                                     ,[vcTelefoneResidencial]
                                                     ,[vcTelefoneCelular]
                                                     ,[bitTelefoneCelularWhatsApp]
                                                     ,[bitRegimeContratacaoCLT]
                                                     ,[vcObservacoes]
                                                     ,[vcEmail])
                                               VALUES
                                                     (@IDGestor
                                                     ,@idEndereco
                                                     ,@TipoProfissional
                                                     ,@NomeCompleto
                                                     ,@CPF
                                                     ,@CNH
                                                     ,@CategoriaCNH
                                                     ,@TelefoneResidencial
                                                     ,@TelefoneCelular
                                                     ,@WPP
                                                     ,@CLT
                                                     ,@Observacao
                                                     ,@Email)";
                        // Execução 
                        trans.Connection.Execute(query, new
                        {
                            IDGestor = profissional.IDGestor,
                            idEndereco = idEndereco,
                            TipoProfissional = (profissional.TipoCNH == TipoCarteira.A ? 1 : 2),
                            NomeCompleto = profissional.NomeCompleto,
                            CPF = profissional.CPF,
                            CNH = profissional.CNH,
                            CategoriaCNH = profissional.TipoCNH,
                            TelefoneResidencial = profissional.TelefoneResidencial,
                            TelefoneCelular = profissional.TelefoneCelular,
                            WPP = profissional.CelularWpp,
                            CLT = profissional.ContratoCLT,
                            Observacao = profissional.Observacao,
                            Email = profissional.Email
                        }, trans);

                        // Commit
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Verifica se existe profissional
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public IEnumerable<ValidaProfissionalExistente> VerificaProfissionalExistente(string email, string cpf)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"SELECT vcCPFCNPJ, vcEmail  FROM  tblColaboradoresEmpresaSistema
			                        WHERE vcEmail = @Email OR vcCPFCNPJ = @CPF";

                // Execução
                return sqlConnection.Query<ValidaProfissionalExistente>(query, new
                {
                    Email = email,
                    CPF = cpf
                });
            }
        }
    }
}
