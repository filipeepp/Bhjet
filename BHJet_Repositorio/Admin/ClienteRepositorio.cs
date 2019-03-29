using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BHJet_Repositorio.Admin
{
    public class ClienteRepositorio : RepositorioBase
    {
        /// <summary>
        /// Verifica se já existe o cliente
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public IEnumerable<ValidaClienteExistente> VerificaExistenciaCliente(string CpfCnpj)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"SELECT vcCPFCNPJ  FROM  tblClientes
			                        WHERE vcCPFCNPJ = @CPFCNPJ";

                // Execução
                return sqlConnection.Query<ValidaClienteExistente>(query, new
                {
                    CPFCNPJ = CpfCnpj
                });
            }
        }

        /// <summary>
        /// Busca dados cadastrais cliente
        /// </summary>
        /// <param name="filtro">clienteID</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ClienteDadosCadastraisEntidade> BuscaClienteDadosCadastrais(long clienteID)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"SELECT
									Cliente.vcNomeRazaoSocial AS NomeRazaoSocial,
									Cliente.vcNomeFantasia AS NomeFantasia,
									Cliente.vcCPFCNPJ AS CPFCNPJ,
									Cliente.vcInscricaoMunicipal AS InscricaoMunicipal,
									Cliente.vcInscricaoEstadual AS InscricaoEstadual,
									Cliente.bitRetemISS AS ISS,
									Cliente.vcObservacoes AS Observacoes,
									Cliente.vcSite AS HomePage,
									Endereco.vcRua AS Endereco,
									Endereco.vcNumero AS NumeroEndereco,
									Endereco.vcComplemento AS Complemento,
									Endereco.vcBairro AS Bairro,
									Endereco.vcCidade AS Cidade,
									Endereco.vcUF AS Estado,
									Endereco.vcCEP AS CEP,
                                    Cliente.bitAvulso as ClienteAvulso
								FROM
									tblClientes Cliente
								INNER JOIN
									tblEnderecos Endereco ON Endereco.idEndereco = Cliente.idEndereco 
								WHERE
									Cliente.idCliente = @ClienteID";

                // Execução
                return sqlConnection.Query<ClienteDadosCadastraisEntidade>(query, new
                {
                    ClienteID = clienteID
                });

            }
        }


        /// <summary>
        /// Busca contatos do cliente
        /// </summary>
        /// <param name="filtro">clienteID</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ClienteContatoEntidade> BuscaClienteContatos(long clienteID)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"SELECT
									idColaboradorCliente AS ID,
									vcNomeContato AS Contato,
									vcDepartamento AS Setor,
									dtDataNascimento AS DataNascimento,
									vcTelefoneComercial AS TelefoneComercial,
									vcTelefoneCelular AS TelefoneCelular,
									vcRamalComercial AS TelefoneRamal,
									bitTelefoneCelularWhatsapp AS Whatsapp,
									vcEmail AS Email
								FROM
									tblColaboradoresCliente
								WHERE
									idCliente = @ClienteID";

                // Execução
                return sqlConnection.Query<ClienteContatoEntidade>(query, new
                {
                    ClienteID = clienteID
                });
            }
        }

        /// <summary>
        /// Busca valor cliente
        /// </summary>
        /// <param name="filtro">clienteID</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ClienteValorEntidade> BuscaClienteValores(long clienteID)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"SELECT * FROM tblClienteTarifario CT
                                     LEFT JOIN [tblDOMTipoVeiculo] DTP on (CT.idTipoVeiculo = DTP.[idTipoVeiculo])
								WHERE
									idCliente = @ClienteID
								ORDER BY CT.bitAtivo desc";

                // Execução
                return sqlConnection.Query<ClienteValorEntidade>(query, new
                {
                    ClienteID = clienteID
                });
            }
        }

        /// <summary>
        /// Busca dados resumidos do(s) cliente(s) e seu(s) contrato(s)
        /// </summary>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ClienteEntidade> BuscaListaClientes(bool avulso = false)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                //Se cliente avulso, adiciona parametro na busca
                string parametroAvulso = "";
                if (avulso)
                    parametroAvulso = " AND Cliente.bitAvulso = 1";

                // Query
                string query = $@"SELECT 
									Cliente.idCliente,
									Cliente.vcNomeRazaoSocial,
									Cliente.vcNomeFantasia,
									Cliente.vcCPFCNPJ,
									Cliente.vcInscricaoEstadual,
									Cliente.bitRetemISS,
									Cliente.vcObservacoes,
									Cliente.vcSite,
                                    Cliente.bitAvulso,
									Endereco.vcRua,
									Endereco.vcNumero,
									Endereco.vcComplemento,
									Endereco.vcBairro,
									Endereco.vcCidade,
									Endereco.vcUF,
									Endereco.vcCEP,
									1 AS bitAtivo
								FROM
									tblClientes Cliente
								INNER JOIN
									tblEnderecos Endereco ON Endereco.idEndereco = Cliente.idEndereco 
								--LEFT JOIN
									--tblClienteTarifario Valor ON Valor.idCliente = Cliente.idCliente
								 --WHERE (1 = 1) {parametroAvulso}";

                // Execução
                return sqlConnection.Query<ClienteEntidade>(query);
            }
        }

        /// <summary>
        /// Busca dados do cliente e de seu contrato ativo
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ClienteEntidade> BuscaClienteContrato(string trecho)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"SELECT
						  			Cliente.idCliente,
									Cliente.vcNomeRazaoSocial,
									Cliente.vcNomeFantasia,
									Cliente.vcCPFCNPJ,
									Cliente.vcInscricaoEstadual,
									Cliente.bitRetemISS,
									Cliente.vcObservacoes,
									Cliente.vcSite,
                                    Cliente.bitAvulso
								FROM tblClientes Cliente
									WHERE 
									CONVERT(VARCHAR(250), Cliente.idCliente) LIKE @valorPesquisa OR
									Cliente.vcNomeFantasia LIKE @valorPesquisa OR
									Cliente.vcNomeRazaoSocial LIKE @valorPesquisa";

                // Execução
                return sqlConnection.Query<ClienteEntidade>(query, new
                {
                    valorPesquisa = "%" + trecho + "%",
                });
            }
        }

        /// <summary>
        /// Busca Profissionais Disponiveis
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ClienteEntidade> BuscaClientes(string trecho)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select * from tblClientes where 
									  convert(varchar(250), idCliente) like @valorPesquisa
									  or
									  vcNomeFantasia like @valorPesquisa
									  or
									  vcNomeRazaoSocial like @valorPesquisa";

                if (string.IsNullOrWhiteSpace(trecho))
                    query.Replace("*", "top(50)");

                // Execução
                return sqlConnection.Query<ClienteEntidade>(query, new
                {
                    valorPesquisa = "%" + trecho + "%",
                });
            }
        }

        /// <summary>
        /// Busca dados da tarifa
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ClienteValorEntidade> BuscaClienteContratoAtivo(int idValor)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"SELECT 
									*
								FROM
									tblClienteTarifario
								WHERE
									idClienteTarifario = @ValorID";

                // Execução
                return sqlConnection.Query<ClienteValorEntidade>(query, new
                {
                    ValorID = idValor,
                });
            }
        }

        /// <summary>
        /// Busca endereço de um cliente específico
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ClienteEntidade> BuscaClienteEndereco(long idCliente)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"SELECT 
									*
								FROM
									tblEnderecos
								INNER JOIN
									tblClientes ON tblClientes.idEndereco = tblEnderecos.idEndereco
								WHERE
									tblClientes.idCliente = @ClienteID";

                // Execução
                return sqlConnection.Query<ClienteEntidade>(query, new
                {
                    ClienteID = idCliente,
                });
            }
        }

        /// <summary>
        /// Inclui Cliente
        /// </summary>
        /// <param name="filtro">ProfissionalCompletoEntidade</param>
        /// <returns>UsuarioEntidade</returns>
        public long? IncluirCliente(ClienteCompletoEntidade cliente)
        {
            long? idCliente = null;
            int? idEndereco = null;
            int? idContato = null;
            int? idValor = null;
            List<int?> Contatos = new List<int?>();
            List<int?> Valores = new List<int?>();

            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Insert Novo Endereco
                        string queryEndereco = @"INSERT INTO [dbo].[tblEnderecos]
                                           ([vcRua]
                                           ,[vcNumero]
                                           ,[vcComplemento]
                                           ,[vcBairro]
                                           ,[vcCidade]
                                           ,[vcUF]
                                           ,[vcCEP]
                                           ,[bitPrincipal])
                                     VALUES
                                           (@Rua
                                           ,@RuaNumero
                                           ,@Complemento
                                           ,@Bairro
                                           ,@Cidade
                                           ,@UF
                                           ,@Cep
                                           ,@EnderecoPrincipal)
                                           select @@identity;";
                        // Execute
                        idEndereco = trans.Connection.ExecuteScalar<int?>(queryEndereco, new
                        {
                            Rua = cliente.DadosCadastrais.Endereco,
                            RuaNumero = cliente.DadosCadastrais.NumeroEndereco,
                            Complemento = cliente.DadosCadastrais.Complemento,
                            Bairro = cliente.DadosCadastrais.Bairro,
                            Cidade = cliente.DadosCadastrais.Cidade,
                            UF = cliente.DadosCadastrais.Estado,
                            Cep = cliente.DadosCadastrais.CEP,
                            EnderecoPrincipal = 1
                        }, trans);

                        // Insere Cliente
                        string queryCliente = @"INSERT INTO [dbo].[tblClientes]
                                                ([idEndereco]
												,[vcNomeRazaoSocial]
                                                ,[vcNomeFantasia]
                                                ,[vcCPFCNPJ]
												,[vcInscricaoEstadual]
												,[bitRetemISS]
												,[vcObservacoes]
                                                ,[vcSite]
                                                ,[bitAvulso])
                                            VALUES
                                                    (@codEndereco
                                                    ,@NomeRazaoSocial
                                                    ,@NomeFantasia
                                                    ,@CPFCNPJ
                                                    ,@InscricaoEstadual
                                                    ,@ISS
                                                    ,@Observacoes
                                                    ,@HomePage
                                                    ,@ClienteAvulso) 
													select @@identity;";

                        // Execute
                        idCliente = trans.Connection.ExecuteScalar<int?>(queryCliente, new
                        {
                            codEndereco = idEndereco,
                            NomeRazaoSocial = cliente.DadosCadastrais.NomeRazaoSocial,
                            NomeFantasia = cliente.DadosCadastrais.NomeFantasia,
                            CPFCNPJ = cliente.DadosCadastrais.CPFCNPJ,
                            InscricaoEstadual = cliente.DadosCadastrais.InscricaoEstadual,
                            ISS = cliente.DadosCadastrais.ISS,
                            Observacoes = cliente.DadosCadastrais.Observacoes,
                            HomePage = cliente.DadosCadastrais.HomePage,
                            ClienteAvulso = cliente.DadosCadastrais.ClienteAvulso
                        }, trans);

                        // Se nao for avulso inclui contrato

                        // Insere Contato
                        string queryContato = @"INSERT INTO [dbo].[tblColaboradoresCliente]
                                           ([idCliente]
                                           ,[vcNomeContato]
                                           ,[vcDepartamento]
                                           ,[dtDataNascimento]
                                           ,[vcTelefoneComercial]
                                           ,[vcTelefoneCelular]
                                           ,[vcEmail])
                                     VALUES
                                           (@codCliente
                                           ,@Contato
                                           ,@Setor
                                           ,@DataNascimento
                                           ,@TelefoneComercial
                                           ,@TelefoneCelular
                                           ,@Email)
                                           select @@identity;";
                        // Execute
                        foreach (var contato in cliente.Contato)
                        {
                            idContato = trans.Connection.ExecuteScalar<int?>(queryContato, new
                            {
                                codCliente = idCliente,
                                Contato = contato.Contato,
                                Setor = contato.Setor,
                                DataNascimento = contato.DataNascimento,
                                TelefoneComercial = contato.TelefoneComercial,
                                TelefoneCelular = contato.TelefoneCelular,
                                Email = contato.Email
                            }, trans);

                            Contatos.Add(idContato);
                        }

                        if (!cliente.DadosCadastrais.ClienteAvulso)
                        {
                            // Insere Tarifa
                            if (cliente.ContratoMoto != null)
                            {
                                var cont = InsereContratoCliente(trans, idCliente ?? 99999999, 1, cliente.ContratoMoto);
                                Valores.Add(cont);
                            }

                            if (cliente.ContratoCarro != null)
                            {
                                var cont = InsereContratoCliente(trans, idCliente ?? 99999999, 2, cliente.ContratoCarro);
                                Valores.Add(cont);
                            }
                        }
                        else
                        {
                            // Insere Contato
                            string queryDadosBancarios = @"insert tblDadosCartaoCredito values (@idCli, @numCartao, @nomCartao, @validade)";

                            // Execute
                            idContato = trans.Connection.ExecuteScalar<int?>(queryDadosBancarios, new
                            {
                                idCli = idCliente,
                                numCartao = cliente.DadosBancarios.NumeroCartaoCredito,
                                nomCartao = cliente.DadosBancarios.NomeCartaoCredito,
                                validade = cliente.DadosBancarios.ValidadeCartaoCredito
                            }, trans);

                            Contatos.Add(idContato);
                        }

                        // Commit
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        if (trans.Connection != null)
                            trans.Rollback();
                        RoolbackCliente(idCliente, idEndereco, Contatos.ToArray(), Valores.ToArray());
                        throw e;
                    }
                }
            }

            return idCliente;
        }

        /// <summary>
        /// Inclui Contato Cliente
        /// </summary>
        /// <param name="filtro">ProfissionalCompletoEntidade</param>
        /// <returns>UsuarioEntidade</returns>
        public void IncluirContato(long clienteID, ClienteContatoEntidade contato)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        string queryContato = @"INSERT INTO [dbo].[tblColaboradoresCliente]
                                           ([idCliente]
                                           ,[vcNomeContato]
                                           ,[vcDepartamento]
                                           ,[dtDataNascimento]
                                           ,[vcTelefoneComercial]
                                           ,[vcTelefoneCelular]
                                           ,[vcEmail])
                                     VALUES
                                           (@codCliente
                                           ,@Contato
                                           ,@Setor
                                           ,@DataNascimento
                                           ,@TelefoneComercial
                                           ,@TelefoneCelular
                                           ,@Email)
                                           select @@identity;";
                        // Execute
                        trans.Connection.ExecuteScalar(queryContato, new
                        {
                            codCliente = clienteID,
                            Contato = contato.Contato,
                            Setor = contato.Setor,
                            DataNascimento = contato.DataNascimento,
                            TelefoneComercial = contato.TelefoneComercial,
                            TelefoneCelular = contato.TelefoneCelular,
                            Email = contato.Email
                        }, trans);

                        //Comit
                        trans.Commit();

                    }
                    catch (Exception e)
                    {
                        if (trans.Connection != null)
                            trans.Rollback();
                    }
                }
            }
        }

        /// <summary>
        /// Deleta Cliente
        /// </summary>
        /// <returns>UsuarioEntidade</returns>
        public void DeletaCliente(long? clienteID)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        string queryContato = @"delete from tblClientes where idCliente = @codCliente";

                        if (clienteID == null)
                        {
                            // Execute
                            trans.Connection.ExecuteScalar(queryContato, new
                            {
                                codCliente = clienteID
                            }, trans);
                        }

                        //Comit
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        if (trans.Connection != null)
                            trans.Rollback();
                    }
                }
            }
        }

        public int InsereContratoCliente(SqlTransaction trans, long idCliente, int tipoVeiculo, ClienteValorEntidade model)
        {
            string queryTarifario = @"INSERT INTO [dbo].[tblClienteTarifario]
                                                      ([idTipoServico]
                                                      ,[idTipoVeiculo]
                                                      ,[idCliente]
                                                      ,[vcDescricaoTarifario]
                                                      ,[dtDataInicioVigencia]
                                                      ,[dtDataFimVigencia]
                                                      ,[decValorContrato]
                                                      ,[intFranquiaKM]
                                                      ,[decValorKMAdicional]
                                                      ,[intFranquiaHoras]
                                                      ,[decValorHoraAdicional]
                                                      ,[intFranquiaMinutosParados]
                                                      ,[decValorMinutoParado]
                                                      ,[bitAtivo]
                                                      ,[vcObservacao])
                                                VALUES
                                                      (2
                                                      ,@tipoVeiculo
                                                      ,@cli
                                                      ,'Contrato cliente'
                                                      ,getdate()
                                                      ,getdate()
                                                      ,@vlContrato
                                                      ,@fqKM
                                                      ,@vlKM
                                                      ,@fqHoras
                                                      ,@vlHoras
                                                      ,@fqMinutos
                                                      ,@vlMinutos
                                                      ,1
                                                      ,@obs)
                                           select @@identity;";

            // Execute carro
            return trans.Connection.ExecuteScalar<int?>(queryTarifario, new
            {
                cli = idCliente,
                tipoVeiculo = tipoVeiculo,
                vlContrato = model.decValorContrato,
                fqKM = model.intFranquiaKM,
                vlKM = model.decValorKMAdicional,
                fqHoras = model.intFranquiaHoras,
                vlHoras = model.decValorHoraAdicional,
                fqMinutos = model.intFranquiaMinutosParados,
                vlMinutos = model.decValorMinutoParado,
                obs = model.Observacao
            }, trans) ?? 0;
        }

        ///// <summary>
        ///// Inclui Contato Cliente
        ///// </summary>
        ///// <param name="filtro">ProfissionalCompletoEntidade</param>
        ///// <returns>UsuarioEntidade</returns>
        //public void IncluirValor(long clienteID, ClienteValorEntidade tarifa)
        //{
        //	using (var sqlConnection = this.InstanciaConexao())
        //	{
        //		using (var trans = sqlConnection.BeginTransaction())
        //		{
        //			try
        //			{
        //				string queryTarifario = @"INSERT INTO [dbo].[tblClienteTarifario]
        //                                         ([idCliente]
        //								   ,[vcDescricaoTarifario]
        //                                         ,[dtDataInicioVigencia]
        //                                         ,[dtDataFimVigencia]
        //                                         ,decValorContrato
        //                                         ,decFranquiaKM
        //                                         ,decValorKMAdicional
        //                                         ,[bitAtivo]
        //                                         ,[vcObservacao])
        //                                   VALUES
        //                                         (@ClienteID
        //								   ,@TipoTarifa
        //                                         ,@VigenciaInicio
        //                                         ,@VigenciaFim
        //                                         ,@ValorContrato
        //                                         ,@QuantidadeKM
        //								   ,@ValorKMAdicional
        //								   ,@TarifaAtivada
        //                                         ,@Observacao)
        //                                         select @@identity;";
        //				// Execute
        //				var idValor = trans.Connection.ExecuteScalar<int?>(queryTarifario, new
        //				{
        //					ClienteID = clienteID,
        //					TipoTarifa = tarifa.DescricaoTarifa,
        //					VigenciaInicio = tarifa.VigenciaInicio,
        //					VigenciaFim = tarifa.VigenciaFim,
        //                          ValorContrato = tarifa.ValorContrato,
        //                          QuantidadeKM = tarifa.QuantidadeKMContratado,
        //                          ValorKMAdicional = tarifa.ValorKMAdicional,
        //					TarifaAtivada = tarifa.status,
        //					Observacao = tarifa.Observacao
        //				}, trans);

        //				//Comit
        //				trans.Commit();
        //			}
        //			catch (Exception e)
        //			{
        //				if (trans.Connection != null)
        //					trans.Rollback();
        //			}
        //		}
        //	}
        //}

        /// <summary>
        /// Atualiza Cliente
        /// </summary>
        /// <param name="filtro">ProfissionalCompletoEntidade</param>
        /// <returns>UsuarioEntidade</returns>
        public void AtualizaCliente(int idEndereco, ClienteCompletoEntidade cliente)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Atualiza Endereco
                        string queryEndereco = @"UPDATE
													[dbo].[tblEnderecos]
												SET
													[vcRua] = @Rua
												   ,[vcNumero] = @RuaNumero
												   ,[vcComplemento] = @Complemento
												   ,[vcBairro] = @Bairro
												   ,[vcCidade] = @Cidade
												   ,[vcUF] = @UF
												   ,[vcCEP] = @Cep
												WHERE
													idEndereco = @EnderecoID";
                        // Execute
                        trans.Connection.Execute(queryEndereco, new
                        {
                            Rua = cliente.DadosCadastrais.Endereco,
                            RuaNumero = cliente.DadosCadastrais.NumeroEndereco,
                            Complemento = cliente.DadosCadastrais.Complemento,
                            Bairro = cliente.DadosCadastrais.Bairro,
                            Cidade = cliente.DadosCadastrais.Cidade,
                            UF = cliente.DadosCadastrais.Estado,
                            Cep = cliente.DadosCadastrais.CEP,
                            EnderecoID = idEndereco
                        }, trans);


                        // Atualiza Cliente
                        using (var sqlConnectionCom = this.InstanciaConexao())
                        {
                            string queryCliente = @"UPDATE
														[dbo].[tblClientes]
													SET
														[vcNomeRazaoSocial] = @NomeRazaoSocial
														,[vcNomeFantasia] = @NomeFantasia
														,[vcCPFCNPJ] = @CPFCNPJ
														,[vcInscricaoEstadual] = @InscricaoEstadual
														,[bitRetemISS] = @ISS
														,[vcObservacoes] = @Observacoes
														,[vcSite] = @HomePage
                                                        ,[bitAvulso] = @avulso
													WHERE
														idCliente = @ClienteID";

                            // Execute
                            trans.Connection.Execute(queryCliente, new
                            {
                                NomeRazaoSocial = cliente.DadosCadastrais.NomeRazaoSocial,
                                NomeFantasia = cliente.DadosCadastrais.NomeFantasia,
                                CPFCNPJ = cliente.DadosCadastrais.CPFCNPJ,
                                InscricaoEstadual = cliente.DadosCadastrais.InscricaoEstadual,
                                ISS = cliente.DadosCadastrais.ISS,
                                Observacoes = cliente.DadosCadastrais.Observacoes,
                                HomePage = cliente.DadosCadastrais.HomePage,
                                ClienteID = cliente.ID,
                                avulso = cliente.DadosCadastrais.ClienteAvulso
                            }, trans);

                        }

                        // Atualiza Contato
                        using (var sqlConnectionCom = this.InstanciaConexao())
                        {
                            string queryContato = @"UPDATE
														[dbo].[tblColaboradoresCliente]
													SET
													   [vcNomeContato] = @Contato
													   ,[vcDepartamento] = @Setor
													   ,[dtDataNascimento] = @DataNascimento
													   ,[vcTelefoneComercial] = @TelefoneComercial
													   ,[vcTelefoneCelular] = @TelefoneCelular
													   ,[vcEmail] = @Email
													 WHERE
														idColaboradorCliente = @ContatoID";
                            // Execute
                            foreach (var contato in cliente.Contato)
                            {
                                trans.Connection.Execute(queryContato, new
                                {
                                    Contato = contato.Contato,
                                    Setor = contato.Setor,
                                    DataNascimento = contato.DataNascimento,
                                    TelefoneComercial = contato.TelefoneComercial,
                                    TelefoneCelular = contato.TelefoneCelular,
                                    Email = contato.Email,
                                    ContatoID = contato.ID
                                }, trans);

                            }
                        }

                        // Atualiza Tarifa
                        if (cliente.ContratoMoto != null && cliente.ContratoCarro != null)
                        {
                            using (var sqlConnectionCom = this.InstanciaConexao())
                            {
                                string queryTarifario = @"delete from tblClienteTarifario
						                                    where idCliente = @cli";

                                trans.Connection.Execute(queryTarifario, new
                                {
                                    cli = cliente.ID
                                }, trans);

                                InsereContratoCliente(trans, cliente.ID, 1, cliente.ContratoMoto);
                                InsereContratoCliente(trans, cliente.ID, 2, cliente.ContratoCarro);
                            }
                        }
                        // Commit
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        if (trans.Connection != null)
                            trans.Rollback();
                        throw e;

                    }
                }
            }
        }

        /// <summary>
        /// Remove contato da base
        /// </summary>
        /// <returns></returns>
        public void ExcluiContato(int idContato)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"DELETE
								FROM
									tblColaboradoresCliente
								WHERE
									idColaboradorCliente = @ContatoID";

                // Execução
                sqlConnection.ExecuteScalar(query, new
                {
                    ContatoID = idContato
                });
            }
        }

        /// <summary>
        /// Remove tarifa da base
        /// </summary>
        /// <returns></returns>
        public void ExcluiContrato(long idValor)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"DELETE
								FROM
									tblClientesTarifario
								WHERE
									idClienteTarifario = @TarifarioID";

                // Execução
                sqlConnection.ExecuteScalar(query, new
                {
                    TarifarioID = idValor
                });
            }
        }

        private void RoolbackCliente(long? idCliente, int? idEndereco, int?[] contatos, int?[] valores)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                if (idCliente != null)
                    sqlConnection.ExecuteScalar($"delete from tblClientes where idCliente = {idCliente}");
                if (idEndereco != null)
                    sqlConnection.ExecuteScalar($"delete from tblEnderecos where idEndereco = {idEndereco}");
                if (contatos.Length <= 0)
                    sqlConnection.ExecuteScalar($"delete from tblColaboradoresCliente where idColaboradorCliente in ({string.Join(",", contatos)})");
                if (valores.Length <= 0)
                    sqlConnection.ExecuteScalar($"delete from tblClienteTarifario where idClienteTarifario in ({string.Join(",", valores)})");

            }
        }

        /// <summary>
        /// Busca dados bancarios
        /// </summary>
        /// <param name="filtro">clienteID</param>
        /// <returns>UsuarioEntidade</returns>
        public DadosBancariosEntidade BuscaDadosBancarios(long clienteID)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select * from tblDadosCartaoCredito where idCliente = @ClienteID";

                // Execução
                return sqlConnection.QueryFirstOrDefault<DadosBancariosEntidade>(query, new
                {
                    ClienteID = clienteID
                });
            }
        }
    }
}
