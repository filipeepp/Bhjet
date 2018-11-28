using BHJet_Core.Enum;
using System;
using System.Collections.Generic;

namespace BHJet_DTO.Faturamento
{
    public class ConsultarFaturamentoDTO
    {
        public IEnumerable<long> IdClienteFiltro { get; set; }
        public DateTime DataInicioFaturamentoFiltro { get; set; }
        public DateTime DataFimFaturamentoFiltro { get; set; }
        public TipoContrato? TipoContratoFiltro { get; set; }
    }
}
