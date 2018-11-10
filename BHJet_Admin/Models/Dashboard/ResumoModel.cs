using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models
{
    public class ResumoModel
    {
        public int ChamadosAvulsosAguardandoMoto { get; set; }
        public int ChamadosAvulsosAguardandoCarro { get; set; }
        public int MotociclistasDisponiveis { get; set; }
        public int CarrosDisponiveis { get; set; }

        [Required(ErrorMessage = "Número da OS obrigatório.")]
        public string PesquisaOSCliente { get; set; }

        [Required(ErrorMessage = "Número do Motorista obrigatório.")]
        public string PesquisaMotociclista { get; set; }

        public string Mensagem { get; set; }
    }
}