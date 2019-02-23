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

    public class TarifarioDTO
    {
        public int idTarifario { get; set; }
        public int idTipoServico { get; set; }
        public int idTipoVeiculo { get; set; }
        public string DescricaoTarifario { get; set; }
        public DateTime? DataInicioVigencia { get; set; }
        public DateTime? DataFimVigencia { get; set; }
        public int? FranquiaMinutosParados { get; set; }
        public decimal? ValorMinutoParado { get; set; }
        public decimal? ValorContrato { get; set; }
        public int? FranquiaKM { get; set; }
        public decimal? ValorKMAdicional { get; set; }
        public int? FranquiaHoras { get; set; }
        public decimal? ValorHoraAdicional { get; set; }
        public decimal? ValorPontoExcedente { get; set; }
        public bool Ativo { get; set; }
        public string Observacao { get; set; }
    }
}
