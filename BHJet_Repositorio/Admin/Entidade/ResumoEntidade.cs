using BHJet_Core.Enum;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class ResumoEntidade
    {
        public TipoProfissional TipoProfissional { get; set; }
        public long Quantidade { get; set; }
    }

    public class ResumoDetalheEntidade
    {
        public long ChamadosAguardandoMotorista { get; set; }
        public long ChamadosAguardandoMotociclista { get; set; }
        public long MotociclistaDisponiveis { get; set; }
        public long MotoristasDisponiveis { get; set; }
    }
}
