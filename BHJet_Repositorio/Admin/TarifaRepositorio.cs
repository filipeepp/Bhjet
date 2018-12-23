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
        public TarifaEntidade BuscaTarificaPorCliente(long clienteID)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Adicionar busca de tarifia exclusia do cliente na NOVA TABELA

                // Query
                string query = @"select top(1) * from tblTarifario
	                                where bitAtivo = 1
		                        order by dtDataInicioVigencia desc";

                // Execução
                return sqlConnection.QueryFirstOrDefault<TarifaEntidade>(query, new { idCli = clienteID });
            }
        }
    }
}
