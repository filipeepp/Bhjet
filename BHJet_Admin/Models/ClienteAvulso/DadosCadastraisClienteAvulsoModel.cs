using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.ClienteAvulso
{
	public class DadosCadastraisClienteAvulsoModel
	{
		public string NomeRazaoSocial { get; set; }
		public string CPFCNPJ { get; set; }
		public string TelefoneResidencial { get; set; }
		public string TelefoneCelular { get; set; }
		public bool TelefoneWhatsapp { get; set; }
		public string Endereco { get; set; }
		public string NumeroEndereco { get; set; }
		public string Complemento { get; set; }
		public string Bairro { get; set; }
		public string Cidade { get; set; }
		public string Estado { get; set; }
		public string CEP { get; set; }
		public string Email { get; set; }
		public string DataNascimento { get; set; }
	}
}