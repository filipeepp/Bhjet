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
        public IEnumerable<TarifaEntidade> BuscaTarificaPorCliente(long clienteID)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select TFC.idTarifario as ID,
	                                    TF.vcDescricaoTarifario as Descricao,
	                                    TF.decValorDiaria as ValorDiaria
	                            from tblClientesTarifario TFC
		                                inner join tblTarifario TF ON (TF.idTarifario = TFC.idTarifario)
		                        where TFC.idCliente = @idCli";

                // Execução
                return sqlConnection.Query<TarifaEntidade>(query, new { idCli = clienteID });
            }
        }
    }
}
