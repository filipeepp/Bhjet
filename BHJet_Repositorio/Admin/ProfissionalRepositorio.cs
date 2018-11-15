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
        public ProfissionalDisponivelEntidade BuscaLocalizacaoProfissionalDisponiveil(int idProfissional)
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

    }
}
