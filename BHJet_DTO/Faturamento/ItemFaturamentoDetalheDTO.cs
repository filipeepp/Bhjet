using System;

namespace BHJet_DTO.Faturamento
{
    public class ItemFaturamentoDetalheDTO
    {
        public string NomeCliente { get; set; }
        public string Tipo { get; set; }
        public DateTime Data { get; set; }
        public long OS { get; set; }
        public string Profissional { get; set; }
        public int KM { get; set; }
        public decimal Valor { get; set; }
    }
}
