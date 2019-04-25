using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models
{
    public class PagamentoModel
    {
        [Required(ErrorMessage = "Nome Cartão Crédito obrigatório.")]
        public string NomeCartaoCredito { get; set; }

        [Required(ErrorMessage = "Número Cartão Crédito obrigatório.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Digite apenas números.")]
        [StringLength(16, ErrorMessage = "Número de cartão deve conter 16 caracteres númericos.", MinimumLength = 16)]
        public string NumeroCartaoCredito { get; set; }

        [Required(ErrorMessage = "Válidade obrigatório.")]
        [RegularExpression(@"^((0[1-9])|(1[0-2]))\/((2009)|(20[1-2][0-9]))$", ErrorMessage = "Formato de válidade inválido")]
        public string Validade { get; set; }

        [Required(ErrorMessage = "Código Verificação obrigatório.")]
        public int? CodigoVerificacao { get; set; }
    }
}