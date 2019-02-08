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
                                    select  top(1) * from tblClienteTarifario where idCliente = @IDCliente order by dtDataInicioVigencia desc
                                ELSE   
                                    select top(1) * from tblTarifario where bitAtivo = 1 order by dtDataInicioVigencia desc";

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
										idTarifario,
										vcObservacao,
										decValorDiaria,
										decFranquiaKMDiaria,
										decValorKMAdicionalDiaria,
										decFranquiaKMMensalidade,
										decValorKMAdicionalMensalidade
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
