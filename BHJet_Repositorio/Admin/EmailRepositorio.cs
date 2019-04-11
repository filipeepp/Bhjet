using BHJet_Repositorio.Admin.Entidade;
using Dapper;

namespace BHJet_Repositorio.Admin
{
    public class EmailRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca situacao Usuario
        /// </summary>
        /// <returns></returns>
        public EmailEntidade BuscaTemplate(long idEmail)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select * from [tblDOMTemplateEmail] where id = @id";

                // Execução
                return sqlConnection.QueryFirstOrDefault<EmailEntidade>(query, new
                {
                    id = idEmail
                });
            }
        }
    }
}
