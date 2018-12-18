using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.ClienteAvulso
{
	public class ClienteAvulsoModel
	{
		public long ID { get; set; }
		public DadosCadastraisClienteAvulsoModel DadosCadastrais { get; set; }
		public List<OsClienteAvulsoModel> Contato { get; set; }

		public ClienteAvulsoModel()
		{
			DadosCadastrais = new DadosCadastraisClienteAvulsoModel() { };
		}
	}
}