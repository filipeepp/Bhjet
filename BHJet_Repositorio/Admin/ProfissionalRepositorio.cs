using BHJet_Enumeradores;
using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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
                string query = @"select  geoPosicao.STY  as vcLatitude, 
       geoPosicao.STX  as vcLongitude,
	   idRegistro, CE.idColaboradorEmpresaSistema, CE.idTipoProfissional, vcNomeCompleto, bitDisponivel from tblColaboradoresEmpresaDisponiveis CED
                                        join tblColaboradoresEmpresaSistema CE ON(CED.idColaboradorEmpresaSistema = ce.idColaboradorEmpresaSistema)
					                where CED.bitDisponivel = 1 and
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
                string query = @"select geoPosicao.STY  as vcLatitude, 
       geoPosicao.STX  as vcLongitude,
	   idRegistro, CE.idColaboradorEmpresaSistema, CE.idTipoProfissional, vcNomeCompleto, bitDisponivel from tblColaboradoresEmpresaDisponiveis CED
                                        join tblColaboradoresEmpresaSistema CE ON(CED.idColaboradorEmpresaSistema = ce.idColaboradorEmpresaSistema)
					                where CED.bitDisponivel = 1 and
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
		                                PRO.idColaboradorEmpresaSistema = @id

                                select * from tblComissaoColaboradorEmpresaSistema
												where idColaboradorEmpresaSistema = @id";

                // Query Multiple
                using (var multi = sqlConnection.QueryMultiple(query, new { id = idProfissional }))
                {
                    // chamadosAguardando
                    var profissional = multi.Read<ProfissionalCompletoEntidade>().FirstOrDefault();

                    // motoristasAguardando
                    var comissoes = multi.Read<ProfissionalComissaoEntidade>().AsList();

                    if (profissional != null)
                        profissional.Comissoes = comissoes.ToArray();

                    // Return
                    return profissional;
                }
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

                        // Comissoes
                        string queryComissao = @"UPDATE  [dbo].[tblComissaoColaboradorEmpresaSistema]
                                        SET
                                            [decPercentualComissao] = @decCom
                                           ,[dtDataInicioVigencia] = @dtIni
                                           ,[dtDataFimVigencia] = @dtFim
                                           ,[vcObservacoes] = @Obs
 	                                   WHERE idComissaoColaboradorEmpresaSistema = @id";


                        string queryAddComissao = @"INSERT INTO [dbo].[tblComissaoColaboradorEmpresaSistema]
                                                    ([idColaboradorEmpresaSistema]
                                                    ,[decPercentualComissao]
                                                    ,[dtDataInicioVigencia]
                                                    ,[dtDataFimVigencia]
                                                    ,vcObservacoes
                                                    ,[bitAtivo])
                                                VALUES
                                                        (@id
                                                        ,@decCom
                                                        ,@dtIni
                                                        ,@dtFim
                                                        ,@Obs
                                                        ,1) select @@identity;";

                        string queryRemoveComissao = @"Delete from [dbo].[tblComissaoColaboradorEmpresaSistema] WHERE idComissaoColaboradorEmpresaSistema not in (@ids) and idColaboradorEmpresaSistema = @idCol";

                        // Notificacoes a excluir
                        long?[] notificacoesAntigasMantidas = profissional.Comissoes.Where(x => x.idComissaoColaboradorEmpresaSistema != null).Select(x => x.idComissaoColaboradorEmpresaSistema).ToArray();
                        trans.Connection.Execute(queryRemoveComissao, new
                        {
                            ids = string.Join(",", notificacoesAntigasMantidas),
                            idCol = profissional.ID
                        }, trans);

                        // Notificacoes antigas e novas
                        foreach (var com in profissional.Comissoes)
                        {
                            if (com.dtDataInicioVigencia == null || com.dtDataInicioVigencia == null)
                                continue;

                            if (com.idComissaoColaboradorEmpresaSistema != null || ExisteComissao(trans, com.idComissaoColaboradorEmpresaSistema, profissional.ID))
                                trans.Connection.Execute(queryComissao, new
                                {
                                    decCom = com.decPercentualComissao,
                                    dtIni = com.dtDataInicioVigencia,
                                    dtFim = com.dtDataFimVigencia,
                                    Obs = com.vcObservacoes,
                                    id = com.idComissaoColaboradorEmpresaSistema
                                }, trans);
                            else
                                trans.Connection.Execute(queryAddComissao, new
                                {
                                    decCom = com.decPercentualComissao,
                                    dtIni = com.dtDataInicioVigencia,
                                    dtFim = com.dtDataFimVigencia,
                                    Obs = com.vcObservacoes,
                                    id = profissional.ID
                                }, trans);
                        }

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

        private bool ExisteComissao(SqlTransaction con, long? idComissao, long idColaborador)
        {
            // Query
            string query = @"select cast(count(*) as bit) from [tblComissaoColaboradorEmpresaSistema]
	                                where idComissaoColaboradorEmpresaSistema = @id and idColaboradorEmpresaSistema = @idCol";

            // Execução
            return con.Connection.QueryFirstOrDefault<bool>(query, new
            {
                id = idComissao,
                idCol = idColaborador
            }, con);
        }

        /// <summary>
        /// Inclui Profissional
        /// </summary>
        /// <param name="filtro">ProfissionalCompletoEntidade</param>
        /// <returns>UsuarioEntidade</returns>
        public void IncluirProfissional(ProfissionalCompletoEntidade profissional)
        {
            int? idColaborador = null;
            int? idEndereco = null;
            List<int> Comissoes = new List<int>();
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
                        idEndereco = trans.Connection.ExecuteScalar<int?>(query, new
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

                        int tipoProfissional = profissional.TipoCNH == TipoCarteira.A ? 1 : 2;

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
                                                     ,@TpProfissional
                                                     ,@NomeCompleto
                                                     ,@CPF
                                                     ,@CNH
                                                     ,@CategoriaCNH
                                                     ,@TelefoneResidencial
                                                     ,@TelefoneCelular
                                                     ,@WPP
                                                     ,@CLT
                                                     ,@Observacao
                                                     ,@Email) select @@identity;";
                        // Execução 
                        idColaborador = trans.Connection.ExecuteScalar<int?>(query, new
                        {
                            IDGestor = profissional.IDGestor,
                            idEndereco = idEndereco,
                            TpProfissional = tipoProfissional,
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

                        // Insere comissões
                        using (var sqlConnectionCom = this.InstanciaConexao())
                        {
                            if (profissional.Comissoes != null && profissional.Comissoes.Any())
                            {
                                string queryComissao = @"INSERT INTO [dbo].[tblComissaoColaboradorEmpresaSistema]
                                                    ([idColaboradorEmpresaSistema]
                                                    ,[decPercentualComissao]
                                                    ,[dtDataInicioVigencia]
                                                    ,[dtDataFimVigencia]
                                                    ,vcObservacoes
                                                    ,[bitAtivo])
                                                VALUES
                                                        (@idCol
                                                        ,@vlComissao
                                                        ,@dtIni
                                                        ,@dtFim
                                                        ,@Obs
                                                        ,1) select @@identity;";

                                foreach (var com in profissional.Comissoes)
                                {
                                    if (!ValidaComissao(sqlConnectionCom, com, idColaborador))
                                    {
                                        var comissao = sqlConnectionCom.ExecuteScalar<int>(queryComissao, new
                                        {
                                            idCol = idColaborador,
                                            vlComissao = com.decPercentualComissao,
                                            dtIni = com.dtDataInicioVigencia,
                                            dtFim = com.dtDataFimVigencia,
                                            Obs = com.vcObservacoes
                                        }, trans);

                                        Comissoes.Add(comissao);
                                    }
                                    else
                                        throw new Exception("Você não pode incluir comissões com periodo de vigência já existentes. Favor preencher corretamente.");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (trans.Connection != null)
                            trans.Rollback();
                        RoolbackColaborador(idEndereco, idColaborador, Comissoes.ToArray());
                        throw e;
                    }
                }
            }
        }

        private void RoolbackColaborador(int? idEndereco, int? idColaborador, int[] comissoes)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                if (comissoes.Any())
                    sqlConnection.ExecuteScalar($"delete from tblComissaoColaboradorEmpresaSistema where idComissaoColaboradorEmpresaSistema in ({string.Join(",", comissoes)})");
                if (idColaborador != null)
                    sqlConnection.ExecuteScalar($"delete from tblColaboradoresEmpresaSistema where idColaboradorEmpresaSistema = {idColaborador}");
                if (idEndereco != null)
                    sqlConnection.ExecuteScalar($"delete from tblEnderecos where idEndereco = {idEndereco}");
            }
        }

        private bool ValidaComissao(SqlConnection con, ProfissionalComissaoEntidade comissao, int? colaboradorID)
        {
            // Query
            string query = @"select cast(count(*) as bit) from 
                                    [tblComissaoColaboradorEmpresaSistema]
                                 where idColaboradorEmpresaSistema = @Col and
		                                (dtDataFimVigencia <= @dtFim or dtDataInicioVigencia >= @dtFim) and
		                                (dtDataInicioVigencia >= @dtIni or dtDataFimVigencia <= @dtIni)";

            // Execução
            return con.QueryFirstOrDefault<bool>(query, new
            {
                Col = colaboradorID,
                dtFim = comissao.dtDataFimVigencia,
                dtIni = comissao.dtDataInicioVigencia
            });
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

        /// <summary>
        /// Busca Comissao do Profissional
        /// </summary>
        /// <param name="idProfissional">idProfissional</param>
        /// <returns>ComissaoProfissionalEntidade</returns>
        public ComissaoProfissionalEntidade BuscaComissaoProfissional(long idProfissional)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select TOP(1) * from tblComissaoColaboradorEmpresaSistema CC
				                    join tblColaboradoresEmpresaSistema CE on (CC.idColaboradorEmpresaSistema = ce.idColaboradorEmpresaSistema)
					                where CC.idColaboradorEmpresaSistema = @id
						              and CC.bitAtivo = 1
					             order by CC.dtDataInicioVigencia desc";

                // Query Multiple
                return sqlConnection.QueryFirstOrDefault<ComissaoProfissionalEntidade>(query, new
                {
                    id = idProfissional
                });
            }
        }

        /// <summary>
        /// Busca Perfil do Profissional
        /// </summary>
        /// <param name="idUsuario">ID usuario logado</param>
        /// <returns>ComissaoProfissionalEntidade</returns>
        public ProfissionalPerfilEntidade BuscaPerfilProfissional(string idUsuario)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select US.idUsuario, 
                                           CE.idColaboradorEmpresaSistema,
                                    	   RD.idRegistroDiaria,
	                                       CE.vcNomeCompleto,
	                                       CE.vcEmail,
	                                       CE.idTipoProfissional
	                                from tblUsuarios US
		                               join tblDOMTiposUsuario TS on (US.idTipoUsuario = TS.idTipoUsuario)
		                               join tblColaboradoresEmpresaSistema CE on (CE.idUsuario = US.idUsuario)
		                               left join tblRegistroDiarias RD on (ce.idColaboradorEmpresaSistema = RD.idColaboradorEmpresaSistema AND
		                                                                  convert(varchar(10), RD.dtDataHoraInicioExpediente, 120)  = convert(varchar(10), getdate(), 120))
                                    WHERE 
		                                  US.idUsuario = @idUser";

                // Query Multiple
                return sqlConnection.QueryFirstOrDefault<ProfissionalPerfilEntidade>(query, new
                {
                    idUser = idUsuario
                });
            }
        }

        /// <summary>
        /// Busca ID do Profissional
        /// </summary>
        /// <param name="idUsuario">ID usuario logado</param>
        /// <returns>long</returns>
        public long BuscaIDProfissional(long idUsuario)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select idColaboradorEmpresaSistema as ID from tblColaboradoresEmpresaSistema where idUsuario = @idUser";

                // Query Multiple
                return sqlConnection.QueryFirstOrDefault<long>(query, new
                {
                    idUser = idUsuario
                });
            }
        }
    }
}
