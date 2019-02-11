using BHJet_CoreGlobal;
using BHJet_Repositorio.Entidade;
using BHJet_Repositorio.Filtro;
using Dapper;
using System.Text;

namespace BHJet_Repositorio.Admin
{
    public class AutenticacaoRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca usuário
        /// </summary>
        /// <param name="filtro">ValidaUsuarioFiltro</param>
        /// <returns>UsuarioEntidade</returns>
        public UsuarioEntidade BuscaUsuario(ValidaUsuarioFiltro filtro)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Parametro
                var senhaOrigem = CriptografiaUtil.Descriptografa(filtro.usuarioSenha, "ch4v3S3m2nt3BHJ0e1tA9u4t4hu1s33r");
                var senhaEncryp = CriptografiaUtil.CriptografiaHash(senhaOrigem);
                var senhaEncrypByte = Encoding.UTF8.GetBytes(senhaEncryp);

                // Query
                string query = @"select * from tblUsuarios where
                                    vcEmail = @usuemaillogin and 
                                    vbPassword = @usupass";
                                    // and 
                                    //bitAtivo = 1";

                if (filtro.usuarioTipo != null)
                    query += " and idTipoUsuario = @usutp";

                // Execução
                return sqlConnection.QueryFirstOrDefault<UsuarioEntidade>(query, new
                {
                    usuemaillogin = filtro.usuarioEmail,
                    usupass = senhaEncrypByte,
                    usutp = filtro.usuarioTipo != null ? ((int)filtro.usuarioTipo) : 1
                });
            }
        }
    }
}
