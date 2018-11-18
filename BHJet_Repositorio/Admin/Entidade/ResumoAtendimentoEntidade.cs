using BHJet_Core.Enum;
using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class ResumoAtendimentoEntidade
    {
        public DateTime? DataRegistro { get; set; }
        public TipoProfissional? TipoProfissional { get; set; }
        public long? Quantidade { get; set; }
    }
}
