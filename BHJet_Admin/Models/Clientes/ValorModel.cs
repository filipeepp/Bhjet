using BHJet_Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.Clientes
{
	public class ValorModel
	{
		public int ID { get; set; }
		public bool ValorRemovido { get; set; }
		public bool ValorAtivado { get; set; }
        public string ValorUnitario { get; set; }
        public TipoTarifa TipoTarifa { get; set; }
		public string VigenciaInicio { get; set; }
        public string VigenciaFim { get; set; }
        public string Franquia { get; set; }
        public string FranquiaAdicional { get; set; }
		public string Observacao { get; set; }
	}
}