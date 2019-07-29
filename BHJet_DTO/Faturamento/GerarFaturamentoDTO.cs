using System;
using System.Collections.Generic;

namespace BHJet_DTO.Faturamento
{
    
    public class GerarFaturamentoDTO
    {
        public bool Faturar { get; set; }
        public IEnumerable<long> IdCliente { get; set; }
        public IEnumerable<long> IdClienteNaoFaturar { get; set; }
        public IEnumerable<long> IDOs { get; set; }
        public DateTime DataInicioFaturamento { get; set; }
        public DateTime DataFimFaturamento { get; set; }
    }
}
