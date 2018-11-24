using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.Clientes
{
	public class ClienteModel
	{
		public int? ID { get; set; }
		public DadosCadastraisModel DadosCadastrais { get; set; }
		public IEnumerable<ContatoModel> Contato { get; set; }
		public IEnumerable<ValorModel> Valor { get; set; }
	}
}