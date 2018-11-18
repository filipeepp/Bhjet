using BHJet_Core.Enum;
using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class ResumoChamados
    {
        public StatusCorrida? Status { get; set; }
        public long? Quantidade { get; set; }
        public DateTime? DataRegistro { get; set; }
    }
}
