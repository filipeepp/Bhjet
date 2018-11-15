using BHJet_Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.Clientes
{
	public class ValorModel
	{
		public TipoTarifa TipoTarifa { get; set; }
		public decimal ValorUnitario { get; set; }
		public DateTime VigenciaInicio { get; set; }
		public DateTime VigenciaFim { get; set; }
		public string Franquia { get; set; }
		public string FranquiaAdicional { get; set; }
		public string Observacao { get; set; }
	}
}