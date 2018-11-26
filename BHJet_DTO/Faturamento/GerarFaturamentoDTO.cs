using System;

namespace BHJet_DTO.Faturamento
{
    
    public class GerarFaturamentoDTO
    {
        public long? IdCliente { get; set; }
        public DateTime DataInicioFaturamento { get; set; }
        public DateTime DataFimFaturamento { get; set; }
    }
}
