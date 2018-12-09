using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class ItemFaturamentoDetalheEntidade
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
