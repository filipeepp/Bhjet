using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.ClienteAvulso
{
	public class OsClienteAvulsoModel
	{
		public long ID { get; set; }
		public string Data { get; set; }
		public string NomeMotorista { get; set; }
		public string Valor { get; set; }
	}
}