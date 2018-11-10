using System.Configuration;
using System.Data.SqlClient;

namespace BHJet_Repositorio
{
    public class RepositorioBase
    {
        public SqlConnection InstanciaConexao()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["DbBHJetCourier"].ToString());
        }
    }
}
