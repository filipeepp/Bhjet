using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.Clientes
{
	public class ContatoModel
	{
        [Required(ErrorMessage = "Nome do Contato obrigatório.")]
        public string Contato { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        public string TelefoneComercial { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório.")]
        [RegularExpression(@"\([0-9]{2}\)[\s][0-9]{4}-[0-9]{4,5}", ErrorMessage = "Formato de Celular inválido")]
        public string TelefoneCelular { get; set; }

		public string Setor { get; set; }

        [Required(ErrorMessage = "Data de aniversário obrigatória.")]
		public DateTime DataNascimento { get; set; }
	}
}