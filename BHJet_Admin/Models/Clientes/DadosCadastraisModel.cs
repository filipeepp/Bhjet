using BHJet_Enumeradores;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models.Clientes
{
	public class DadosCadastraisModel
	{
		public int ID { get; set; }

        public bool ClienteAvulso { get; set; }

        [Required(ErrorMessage = "Nome ou Razão Social obrigatório.")]
        public string NomeRazaoSocial { get; set; }

		[Required(ErrorMessage = "Nome Fantasia orbigatório.")]
		public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "CPF/CNPJ Completo obrigatório.")]
        [StringLength(18, ErrorMessage = "Formato de CPF/CNPJ inválido.", MinimumLength = 14)]
        [RegularExpression(@"([0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})", ErrorMessage = "Formato de CPF/CNPJ inválido.")]
        public string CPFCNPJ { get; set; }

        [Required(ErrorMessage = "Inscrição Estadual obrigatória.")]
        public string InscricaoEstadual { get; set; }

        public bool ISS { get; set; }

        [Required(ErrorMessage = "Nome da rua obrigatório.")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Numero do endereço obrigatório.")]
        public string NumeroEndereco { get; set; }

		public string Complemento { get; set; }

        [Required(ErrorMessage = "Bairro obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Cidade obrigatório.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Estado obrigatório.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "CEP obrigatório.")]
        [RegularExpression(@"[0-9]{5}-[0-9]{3}", ErrorMessage = "Formato de CEP inválido.")]
        public string CEP { get; set; }

		public string Observacoes { get; set; }

		public string HomePage { get; set; }
		


	}
}