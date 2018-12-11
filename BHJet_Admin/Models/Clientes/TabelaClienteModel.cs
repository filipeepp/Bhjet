using BHJet_Core.Enum;
using System.Collections.Generic;

namespace BHJet_Admin.Models
{

    public class TabelaClienteModel
    {
		private IEnumerable<long> _ClienteSelecionado;
		public IEnumerable<long> ClienteSelecionado
		{
			get
			{
				return _ClienteSelecionado;
			}
			set
			{
				_ClienteSelecionado = value;
			}
		}

		public List<LinhaClienteModel> ListModel { get; set; }
    }

    public class LinhaClienteModel
    {
        public long? ClienteID { get; set; }
        public string NomeRazaoSocial { get; set; }
        public TipoContrato TipoContrato { get; set; }
    }
}