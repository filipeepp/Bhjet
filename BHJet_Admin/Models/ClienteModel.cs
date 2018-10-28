using BHJet_Core.Enum;
using System.Collections.Generic;

namespace BHJet_Admin.Models
{
    public class ClientesModel
    {
        public List<ClienteModel> ListModel { get; set; }
    }

    public class ClienteModel
    {
        public int ClienteID { get; set; }
        public string NomeRazaoSocial { get; set; }
        public TipoContrato TipoContrato { get; set; }
    }
}