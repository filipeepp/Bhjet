using BHJet_Core.Enum;

namespace BHJet_DTO.Dashboard
{
    public class ResumoAtendimentoModel
    {
        public int Mes { get; set; }
        public long? QtdAtendimentoMotorista { get; set; }
        public long? QtdAtendimentoMotociclista { get; set; }
    }
}
