using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System;
using System.Collections.Generic;

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
		/// Busca dados resumidos do(s) cliente(s) e seu(s) contrato(s)
		/// </summary>
		/// <param name="filtro">TipoProfissional</param>
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
									vcNomeFantasia LIKE @valorPesquisa OR
									vcNomeRazaoSocial LIKE @valorPesquisa";

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
