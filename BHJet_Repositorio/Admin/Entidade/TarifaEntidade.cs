using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class TarifaEntidade
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
        public int? intFranquiaHoras { get; set; }
        public decimal? decValorHoraAdicional { get; set; }
        public bool Ativo { get; set; }
        public string Observacao { get; set; }
    }

    public class TarifarioEntidade
    {
        public int idTarifario { get; set; }
        public int idTipoServico { get; set; }
        public int idTipoVeiculo { get; set; }
        public string vcDescricaoTarifario { get; set; }
        public DateTime? dtDataInicioVigencia { get; set; }
        public DateTime? dtDataFimVigencia { get; set; }
        public int? intFranquiaMinutosParados { get; set; }
        public decimal? decValorMinutoParado { get; set; }
        public decimal? decValorContrato { get; set; }
        public int? intFranquiaKM { get; set; }
        public decimal? decValorKMAdicional { get; set; }
        public int? intFranquiaHoras { get; set; }
        public decimal? decValorHoraAdicional { get; set; }
        public decimal? decValorPontoExcedente { get; set; }
        public bool bitAtivo { get; set; }
        public string vcObservacao { get; set; }
    }
}
