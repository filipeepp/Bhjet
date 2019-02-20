using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System.Collections.Generic;

namespace BHJet_Repositorio.Admin
{
    public class TarifaRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca tarifas de um cliente especifico
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public TarifaEntidade BuscaTarificaPorCliente(long? clienteID)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Adicionar busca de tarifia exclusia do cliente na NOVA TABELA

                // Query
                string query = @"DECLARE @ExisteCliente int;  
                                 DECLARE @IDCliente int;  
                                    set @IDCliente = @id;
                                    set @ExisteCliente = (select CAST(COUNT(1) AS BIT) from tblClienteTarifario where idCliente = @IDCliente )
                                IF @ExisteCliente = 1  
                                    select  top(1)  idClienteTarifario as ID,
												   vcDescricaoTarifario as Descricao,
												   dtDataInicioVigencia as DataInicioVigencia,
												   decValorContrato as ValorContrato,
												   decFranquiaKM as FranquiaKM,
												   decValorKMAdicional as ValorKMAdicional,
												   bitAtivo as Ativo,
												  vcObservacao as Observacao from tblClienteTarifario where idCliente = @IDCliente  and bitAtivo = 1 order by dtDataInicioVigencia desc
                                ELSE   
                                    select top(1) idTarifario as ID,
												   vcDescricaoTarifario as Descricao,
												   dtDataInicioVigencia as DataInicioVigencia,
												   decValorContrato as ValorContrato,
												   intFranquiaMinutosParados as MinutosParados,
												   decValorMinutoParado as ValorMinutosParados,
												   decFranquiaKM as FranquiaKM,
												   decValorKMAdicional as ValorKMAdicional,
												   bitAtivo as Ativo from tblTarifario where bitAtivo = 1 order by dtDataInicioVigencia desc";

                // Execução
                return sqlConnection.QueryFirstOrDefault<TarifaEntidade>(query, new { id = clienteID });
            }
		}

		/// <summary>
		/// Busca tarifa ativa na tabela de preço
		/// </summary>
		/// <param name="filtro">TipoProfissional</param>
		/// <returns>TarifaDTO</returns>
		public TarifaEntidade BuscaTarfaPadraoAtiva(int codigoTipoVeiculo)
		{
			using (var sqlConnection = this.InstanciaConexao())
			{
				// Query
				string query = @"SELECT
										idTarifario as ID,
										vcObservacao as Observacao,
										decValorContrato as  ValorContrato,
										decFranquiaKM as FranquiaKM,
										decValorKMAdicional as ValorKMAdicional,
                                        bitAtivo as Ativo
									FROM
										tblTarifario
									WHERE
										idTipoVeiculo = @TipoVeiculo
									AND
										bitAtivo = 1";

				// Execução
				return sqlConnection.QueryFirstOrDefault<TarifaEntidade>(query, new { TipoVeiculo = codigoTipoVeiculo });
			}
		}
	}
}
