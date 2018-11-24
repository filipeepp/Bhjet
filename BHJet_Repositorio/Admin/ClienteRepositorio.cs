using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System.Collections.Generic;

namespace BHJet_Repositorio.Admin
{
    public class ClienteRepositorio : RepositorioBase
    {
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
									  convert(varchar(250), idCliente) like {0}
									  or
									  vcNomeFantasia like {0}
									  or
									  vcNomeRazaoSocial like {0}";

                // Execução
                return sqlConnection.Query<ClienteEntidade>(query, new
                {
                    valorPesquisa = "%" + trecho + "%",
                });
            }
        }
    }
}
