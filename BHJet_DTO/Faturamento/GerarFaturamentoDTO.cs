using System;
using System.Collections.Generic;

namespace BHJet_DTO.Faturamento
{
    
    public class GerarFaturamentoDTO
    {
        public IEnumerable<long> IdCliente { get; set; }
        public DateTime DataInicioFaturamento { get; set; }
        public DateTime DataFimFaturamento { get; set; }
    }
}
