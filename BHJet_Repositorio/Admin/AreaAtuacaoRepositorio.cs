using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System;
using System.Collections.Generic;

namespace BHJet_Repositorio.Admin
{
    public class AreaAtuacaoRepositorio : RepositorioBase
    {
        /// <summary>
		/// Busca Area Atuacao Ativas
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AreaAtuacaoEntidade> BuscaAreaAtuacaoAtiva()
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select * from [tblAreaAtuacao] where [bitDisponivel] = 1";

                // Execução
                return sqlConnection.Query<AreaAtuacaoEntidade>(query);
            }
        }


        /// <summary>
        /// Atualiza Areas Atuacao Ativas
        /// </summary>
        /// <param name="filtro">AreaAtuacaoEntidade</param>
        /// <returns></returns>
        public void AtualizaAreaAtuacaoAtiva(AreaAtuacaoEntidade[] filtro)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {

                    try
                    {
                        // Query
                        string query = @"delete from [tblAreaAtuacao] where [bitDisponivel] = 1";
                        
                        // Delete
                        trans.Connection.Execute(query, transaction: trans);

                        // Carrega areas
                        foreach(var geo in filtro)
                        {
                            // Query de inserção
                            string queryInsert = @"INSERT INTO [dbo].[tblAreaAtuacao]
                                                          ([vcGeoVertices]
                                                          ,[bitDisponivel])
                                                    VALUES (@geoLocation , 1)";
                            // Insere
                            trans.Connection.ExecuteScalar(queryInsert, new
                            {
                                geoLocation = geo.vcGeoVertices
                            }, transaction: trans);
                        }

                        // Commit
                        trans.Commit();
                    }
                    catch(Exception e)
                    {
                        // Desfaz
                        trans.Rollback();
                        throw;
                    }
                }

               
            }
        }




    }
}
