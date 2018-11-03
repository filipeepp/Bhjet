using BHJet_Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models.Motorista
{
    public class NovoMotoristaModel
    {
        [Required(ErrorMessage = "Nome Completo obrigatório.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "CPF/CNPJ Completo obrigatório.")]
        [StringLength(18, ErrorMessage = "Formato de CPF/CNPJ inválido.", MinimumLength = 14)]
        [RegularExpression(@"([0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})", ErrorMessage = "Formato de CPF/CNPJ inválido.")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "Número CNH obrigatório.")]
        public string CNH { get; set; }

        [Required(ErrorMessage = "Tipo de carteira obrigatório.")]
        public TipoCarteira TipoCarteiraMotorista { get; set; }

        [Required(ErrorMessage = "Endereço obrigatório.")]
        public string Endereco { get; set; }

        [RegularExpression(@"^(\(11\) [9][0-9]{4}-[0-9]{4})|(\(1[2-9]\) [5-9][0-9]{3}-[0-9]{4})|(\([2-9][1-9]\) [5-9][0-9]{3}-[0-9]{4})$", ErrorMessage = "Formato de Telefone inválido")]
        public string TelefoneResidencial { get; set; }

        [RegularExpression(@"^(\(11\) [9][0-9]{4}-[0-9]{4})|(\(1[2-9]\) [5-9][0-9]{3}-[0-9]{4})|(\([2-9][1-9]\) [5-9][0-9]{3}-[0-9]{4})$", ErrorMessage = "Formato de Celular inválido")]
        public string TelefoneCelular { get; set; }

        public bool? CelularWhatsapp { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tipo de contrato obrigatório.")]
        public RegimeContratacao TipoRegimeContratacao { get; set; }

        public string Observacao { get; set; }
    }
}