using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models
{
    public class ResumoModel
    {
        public long ChamadosAvulsosAguardandoMoto { get; set; }
        public long ChamadosAvulsosAguardandoCarro { get; set; }
        public long MotociclistasDisponiveis { get; set; }
        public long CarrosDisponiveis { get; set; }

        [Required(ErrorMessage = "Número da OS obrigatório.")]
        public string PesquisaOSCliente { get; set; }

        [Required(ErrorMessage = "Número do Motorista obrigatório.")]
        public string PesquisaMotociclista { get; set; }

        public string Mensagem { get; set; }
    }
}