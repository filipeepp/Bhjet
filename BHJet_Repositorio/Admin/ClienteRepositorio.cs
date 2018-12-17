using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System;
using System.Collections.Generic;
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
		public IEnumerable<ValidaClienteExistente> VerificaExistenciaProfissional(string CpfCnpj)
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
									Endereco.vcCEP AS CEP
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
				string query = @"SELECT
									Valor.idTarifario AS ID,
									Valor.vcDescricaoTarifario AS TipoTarifa,
									Valor.dtDataInicioVigencia AS VigenciaInicio,
									Valor.dtDataFimVigencia AS VigenciaFim,
									Valor.decValorBandeirada AS ValorBandeira,
									Valor.decFranquiaKMBandeirada AS ValorKMBandeiradada,
									Valor.decValorKMAdicionalCorrida AS ValorKMAdicionalCorrida,
									Valor.intFranquiaMinutosParados AS MinutosParado,
									Valor.decValorMinutoParado AS ValorMinutosParado,
									Valor.timFaixaHorarioInicial AS HorarioInicial,
									Valor.timFaixaHorarioFinal AS HorarioFinal,
									Valor.decValorDiaria AS ValorDiaria,
									Valor.decFranquiaKMDiaria AS ValorKMDiaria,
									Valor.decValorKMAdicionalDiaria AS ValorKMAdicionalDiaria,
									Valor.decValorMensalidade AS ValorUnitario,
									Valor.decFranquiaKMMensalidade AS Franquia,
									Valor.decValorKMAdicionalMensalidade AS FranquiaAdicional,
									Valor.bitAtivo AS ValorAtivado,
									Valor.bitPagamentoAvista AS PagamentoAVista,
									Valor.vcObservacao AS Observacao
								FROM
									tblTarifario Valor
								INNER JOIN
									tblClientesTarifario ON tblClientesTarifario.idTarifario = Valor.idTarifario 
								WHERE
									tblClientesTarifario.idCliente = @ClienteID
								ORDER BY Valor.bitAtivo desc";

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
		public IEnumerable<ClienteEntidade> BuscaListaClientes()
		{
			using (var sqlConnection = this.InstanciaConexao())
			{
				// Query
				string query = @"SELECT TOP 50 
									Cliente.idCliente,
									Cliente.vcNomeRazaoSocial,
									Cliente.vcNomeFantasia,
									Cliente.vcCPFCNPJ,
									Cliente.vcInscricaoEstadual,
									Cliente.bitRetemISS,
									Cliente.vcObservacoes,
									Cliente.vcSite,
									Endereco.vcRua,
									Endereco.vcNumero,
									Endereco.vcComplemento,
									Endereco.vcBairro,
									Endereco.vcCidade,
									Endereco.vcUF,
									Endereco.vcCEP,
									Valor.bitAtivo,
									Valor.vcDescricaoTarifario
								FROM
									tblClientes Cliente
								INNER JOIN
									tblEnderecos Endereco ON Endereco.idEndereco = Cliente.idEndereco 
								INNER JOIN
									tblClientesTarifario ClienteValor ON ClienteValor.idCliente = Cliente.idCliente
								INNER JOIN
									tblTarifario Valor ON Valor.idTarifario = ClienteValor.idTarifario
								WHERE
									Valor.bitAtivo = 1";

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
									Valor.vcDescricaoTarifario,
									Valor.bitAtivo
								FROM tblClientes Cliente
								INNER JOIN 
									tblClientesTarifario on tblClientesTarifario.idCliente = Cliente.idCliente 
								INNER JOIN
									tblTarifario Valor on Valor.idTarifario = tblClientesTarifario.idTarifario 
								WHERE 
									CONVERT(VARCHAR(250), Cliente.idCliente) LIKE @valorPesquisa OR
									Cliente.vcNomeFantasia LIKE @valorPesquisa OR
									Cliente.vcNomeRazaoSocial LIKE @valorPesquisa OR
									Valor.vcDescricaoTarifario LIKE @valorPesquisa";

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
									tblTarifario
								WHERE
									idTarifario = @ValorID";

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
		public void IncluirCliente(ClienteCompletoEntidade cliente)
		{
			int? idCliente = null;
			int? idEndereco = null;
			int? idContato = null;
			int? idValor = null;
			int? idValorCliente = null;
			List<int?> Contatos = new List<int?>();
			List<int?> Valores = new List<int?>();
			List<int?> ValoresClientes = new List<int?>();

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
						using (var sqlConnectionCom = this.InstanciaConexao())
						{
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
								ClienteAvulso = 0
							}, trans);

						}

						// Insere Contato
						using (var sqlConnectionCom = this.InstanciaConexao())
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
							foreach(var contato in cliente.Contato)
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
						}

						// Insere Tarifa
						using (var sqlConnectionCom = this.InstanciaConexao())
						{
							string queryTarifario = @"INSERT INTO [dbo].[tblTarifario]
                                           ([vcDescricaoTarifario]
                                           ,[dtDataInicioVigencia]
                                           ,[dtDataFimVigencia]
                                           ,[decFranquiaKMMensalidade]
                                           ,[decValorKMAdicionalMensalidade]
                                           ,[decValorMensalidade]
                                           ,[bitAtivo]
                                           ,[bitPagamentoAVista]
                                           ,[vcObservacao])
                                     VALUES
                                           (@TipoTarifa
                                           ,@VigenciaInicio
                                           ,@VigenciaFim
                                           ,@Franquia
                                           ,@FranquiaAdicional
										   ,@ValorUnitario
										   ,@TarifaAtivada
										   ,@PagamentoAVista
                                           ,@Observacao)
                                           select @@identity;";

							string queryClienteTarifario = @"INSERT INTO [dbo].[tblClientesTarifario]
                                           ([idCliente]
                                           ,[idTarifario])
                                     VALUES
                                           (@codCliente
                                           ,@codTarifario)
                                           select @@identity;";

							// Execute
							foreach (var tarifa in cliente.Valor)
							{
								idValor = trans.Connection.ExecuteScalar<int?>(queryTarifario, new
								{
									TipoTarifa = tarifa.TipoTarifa,
									VigenciaInicio = tarifa.VigenciaInicio,
									VigenciaFim = tarifa.VigenciaFim,
									Franquia = tarifa.Franquia,
									FranquiaAdicional = tarifa.FranquiaAdicional,
									ValorUnitario = tarifa.ValorUnitario,
									TarifaAtivada = tarifa.ValorAtivado,
									PagamentoAVista = 0,
									Observacao = tarifa.Observacao
								}, trans);

								Valores.Add(idValor);

								idValorCliente = trans.Connection.ExecuteScalar<int?>(queryClienteTarifario, new
								{
									codCliente = idCliente,
									codTarifario = idValor
								}, trans);

								ValoresClientes.Add(idValorCliente);
							}
						}

						// Commit
						trans.Commit();
					}
					catch (Exception e)
					{
						if (trans.Connection != null)
							trans.Rollback();
						RoolbackCliente(idCliente, idEndereco, Contatos.ToArray(), Valores.ToArray(), ValoresClientes.ToArray());
						throw e;
					}
				}
			}
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
		/// Inclui Contato Cliente
		/// </summary>
		/// <param name="filtro">ProfissionalCompletoEntidade</param>
		/// <returns>UsuarioEntidade</returns>
		public void IncluirValor(long clienteID, ClienteValorEntidade tarifa)
		{
			using (var sqlConnection = this.InstanciaConexao())
			{
				using (var trans = sqlConnection.BeginTransaction())
				{
					try
					{
						string queryTarifario = @"INSERT INTO [dbo].[tblTarifario]
                                           ([vcDescricaoTarifario]
                                           ,[dtDataInicioVigencia]
                                           ,[dtDataFimVigencia]
                                           ,[decFranquiaKMMensalidade]
                                           ,[decValorKMAdicionalMensalidade]
                                           ,[decValorMensalidade]
                                           ,[bitAtivo]
                                           ,[bitPagamentoAVista]
                                           ,[vcObservacao])
                                     VALUES
                                           (@TipoTarifa
                                           ,@VigenciaInicio
                                           ,@VigenciaFim
                                           ,@Franquia
                                           ,@FranquiaAdicional
										   ,@ValorUnitario
										   ,@TarifaAtivada
										   ,@PagamentoAVista
                                           ,@Observacao)
                                           select @@identity;";

						string queryClienteTarifario = @"INSERT INTO [dbo].[tblClientesTarifario]
                                           ([idCliente]
                                           ,[idTarifario])
                                     VALUES
                                           (@codCliente
                                           ,@codTarifario)
                                           select @@identity;";
						// Execute
						var idValor = trans.Connection.ExecuteScalar<int?>(queryTarifario, new
						{
							TipoTarifa = tarifa.TipoTarifa,
							VigenciaInicio = tarifa.VigenciaInicio,
							VigenciaFim = tarifa.VigenciaFim,
							Franquia = tarifa.Franquia,
							FranquiaAdicional = tarifa.FranquiaAdicional,
							ValorUnitario = tarifa.ValorUnitario,
							TarifaAtivada = tarifa.ValorAtivado,
							PagamentoAVista = 0,
							Observacao = tarifa.Observacao
						}, trans);

						trans.Connection.ExecuteScalar(queryClienteTarifario, new
						{
							codCliente = clienteID,
							codTarifario = idValor
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
								ClienteID = cliente.ID
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
						using (var sqlConnectionCom = this.InstanciaConexao())
						{
							string queryTarifario = @"UPDATE
														[dbo].[tblTarifario]
													SET
													   [vcDescricaoTarifario] = @TipoTarifa
													   ,[dtDataInicioVigencia] = @VigenciaInicio
													   ,[dtDataFimVigencia] = @VigenciaFim
													   ,[decFranquiaKMMensalidade] = @Franquia
													   ,[decValorKMAdicionalMensalidade] = @FranquiaAdicional
													   ,[decValorMensalidade] = @ValorUnitario
													   ,[bitAtivo] = @TarifaAtivada
													   ,[vcObservacao] = @Observacao
													 WHERE
														idTarifario = @ValorID";

							// Execute
							foreach (var tarifa in cliente.Valor)
							{
								trans.Connection.Execute(queryTarifario, new
								{
									TipoTarifa = tarifa.TipoTarifa,
									VigenciaInicio = tarifa.VigenciaInicio,
									VigenciaFim = tarifa.VigenciaFim,
									Franquia = tarifa.Franquia,
									FranquiaAdicional = tarifa.FranquiaAdicional,
									ValorUnitario = tarifa.ValorUnitario,
									TarifaAtivada = tarifa.ValorAtivado,
									Observacao = tarifa.Observacao,
									ValorID = tarifa.ID
								}, trans);
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
									idTarifario = @TarifarioID

								DELETE
								FROM
									tblTarifario
								WHERE
									idTarifario = @TarifarioID";

				// Execução
				sqlConnection.QueryMultiple(query, new
				{
					TarifarioID = idValor
				});
			}
		}

		private void RoolbackCliente(int? idCliente, int? idEndereco, int?[] contatos, int?[] valores, int?[] valoresClientes)
		{
			using (var sqlConnection = this.InstanciaConexao())
			{
				if(idCliente != null)
					sqlConnection.ExecuteScalar($"delete from tblClientes where idCliente = {idCliente}");
				if (idEndereco != null)
					sqlConnection.ExecuteScalar($"delete from tblEnderecos where idEndereco = {idEndereco}");
				if (contatos.Length <= 0)
					sqlConnection.ExecuteScalar($"delete from tblColaboradoresCliente where idColaboradorCliente in ({string.Join(",", contatos)})");
				if (valores.Length <= 0)
					sqlConnection.ExecuteScalar($"delete from tblTarifario where idTarifario in ({string.Join(",", valores)})");
				if (valoresClientes.Length <= 0)
					sqlConnection.ExecuteScalar($"delete from tblClientesTarifario where idClienteTarifa in ({string.Join(",", valoresClientes)})");

			}
		}
	}
}
