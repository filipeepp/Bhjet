using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.ClienteAvulso
{
	public class ListaClienteAvulsoModel
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

		public List<ComponenteClienteAvulsoModel> ListClienteAvulso { get; set; }
	}

	public class ComponenteClienteAvulsoModel
	{
		public long? ClienteID { get; set; }
		public string NomeRazaoSocial { get; set; }
	}
}