using BHJet_Core.Enum;
using BHJet_Core.Utilitario;
using BHJet_Repositorio.Entidade;
using Dapper;
using System.Collections.Generic;
using System.Text;

namespace BHJet_Repositorio.Admin
{
    public class UsuarioRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca Usuarios
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<UsuarioEntidade> BuscaUsuarios(string trecho)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select %topCondition% idUsuario, 
	                                    idTipoUsuario,
	                                    vcEmail,
	                                    bitAtivo,
                                    case bitAtivo
	                                when 1 then 'Ativo'
	                                when 0 then 'Inativo' end as bitDescAtivo
	                            from tblUsuarios 		
                                    where vcEmail like @usuemaillogin ";

                // Filtro quantidade
                if (string.IsNullOrWhiteSpace(trecho))
                    query = query.Replace("%topCondition%", "top(50)");
                else
                    query = query.Replace("%topCondition%", "");

                // Execução
                return sqlConnection.Query<UsuarioEntidade>(query, new
                {
                    usuemaillogin = "%" + trecho + "%",
                });
            }
        }

        /// <summary>
        /// Incluir Usuarios
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public void IncluirUsuario(UsuarioEntidade usuario)
        {
            // Monta senha
            var senhaOrigem = CriptografiaUtil.Descriptografa(usuario.vbIncPassword, "ch4v3S3m2nt3BHJ0e1tA9u4t4hu1s33r");
            var senhaEncryp = CriptografiaUtil.CriptografiaHash(senhaOrigem);
            var senhaEncrypByte = Encoding.UTF8.GetBytes(senhaEncryp);

            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"INSERT INTO [dbo].[tblUsuarios]
                                    ([idTipoUsuario]
                                    ,[vcEmail]
                                    ,[vbPassword]
                                    ,[bitAtivo])
                                VALUES
                                    (@tpUser
                                    ,@email
                                    ,@senha
                                    ,@situacao) select @@identity;";

                // Execução
                var idUsurario = sqlConnection.ExecuteScalar<int>(query, new
                {
                    tpUser = usuario.idTipoUsuario,
                    email = usuario.vcEmail,
                    senha = senhaEncrypByte,
                    situacao = usuario.bitAtivo
                });

                // Atualiza cliente
                switch (usuario.idTipoUsuario)
                {
                    case TipoUsuario.ClienteAvulsoSite:
                        string queryCliente = @"update tblClientes set idUsuario = @IdUsu where idCliente = @idCli";
                        sqlConnection.ExecuteScalar(queryCliente, new
                        {
                            IdUsu = idUsurario,
                            idCli = usuario.ClienteSelecionado
                        });
                        break;
                    case TipoUsuario.FuncionarioCliente:
                        string queryClienteCol = @"update tblColaboradoresCliente set idUsuario = @IdUsu where idCliente = @idCli";
                        sqlConnection.ExecuteScalar(queryClienteCol, new
                        {
                            IdUsu = idUsurario,
                            idCli = usuario.ClienteSelecionado
                        });
                        break;
                }

            }
        }

        /// <summary>
        /// Atualiza situacao Usuario
        /// </summary>
        /// <returns></returns>
        public void AtualizaSituacaoUsuario(long idUser, long situacao)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {

                // Query
                string query = @"UPDATE tblUsuarios set bitAtivo = @sit where idUsuario = @id";

                // Execução
                sqlConnection.ExecuteScalar(query, new
                {
                    id = idUser,
                    sit = situacao
                });
            }
        }

        /// <summary>
        /// Deleta situacao Usuario
        /// </summary>
        /// <returns></returns>
        public void DeletaUsuario(long idUser)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {

                // Query
                string query = @"delete from tblUsuarios where idUsuario = @id";

                // Execução
                sqlConnection.ExecuteScalar(query, new
                {
                    id = idUser
                });
            }
        }

        /// <summary>
        /// Busca situacao Usuario
        /// </summary>
        /// <returns></returns>
        public UsuarioEntidade BuscaUsuario(long idUser)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {

                // Query
                string query = @"select * from tblUsuarios where idUsuario = @id";

                // Execução
                return sqlConnection.QueryFirstOrDefault<UsuarioEntidade>(query, new
                {
                    id = idUser
                });
            }
        }
    }
}
