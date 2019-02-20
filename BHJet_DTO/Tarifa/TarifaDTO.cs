using System;

namespace BHJet_DTO.Tarifa
{
    public class TarifaDTO
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataInicioVigencia { get; set; }
        public DateTime? DataFimVigencia { get; set; }
        public decimal? ValorContrato { get; set; }
        public int? FranquiaKM { get; set; }
        public decimal? ValorKMAdicional { get; set; }
        public int? MinutosParados { get; set; }
        public decimal? ValorMinutosParados { get; set; }
        public bool Ativo { get; set; }
        public string Observacao { get; set; }
    }
}
