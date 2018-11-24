using System.Configuration;
using System.Data.SqlClient;

namespace BHJet_Repositorio
{
    public class RepositorioBase
    {
        public SqlConnection InstanciaConexao()
        {
            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbBHJetCourier"].ToString());
            con.Open();
            return con;
        }
    }
}
