using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.Clientes
{
	public class ContatoModel
	{
		public bool ContatoRemovido { get; set; }
		public int ID { get; set;}
        public string Contato { get; set; }
        public string Email { get; set; }
        public string TelefoneComercial { get; set; }
		public string TelefoneCelular { get; set; }
		public string Setor { get; set; }
		public string DataNascimento { get; set; }
	}
}