using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models
{
    public class PagamentoModel
    {
        [Required(ErrorMessage = "Nome Cartao Credito obrigatório.")]
        public string NomeCartaoCredito { get; set; }

        [Required(ErrorMessage = "Número Cartao Credito obrigatório.")]
        public string NumeroCartaoCredito { get; set; }

        [Required(ErrorMessage = "Validade obrigatório.")]
        public string Validade { get; set; }

        [Required(ErrorMessage = "Codigo Verificacao obrigatório.")]
        public int? CodigoVerificacao { get; set; }
    }
}